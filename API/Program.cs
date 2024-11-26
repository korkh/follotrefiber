using API.Extensions;
using API.Middleware;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
app.UseXContentTypeOptions();
app.UseReferrerPolicy(options => options.NoReferrer());
app.UseXXssProtection(Options => Options.EnabledWithBlockMode());
app.UseXfo(opt => opt.Deny()); //against click-hijacking
app.UseCsp(opt =>
    opt //all from wwwroot from client folder are allowed and for development to remove all issues use app.UseCspReportOnly but before production just app.UseCsp
    .BlockAllMixedContent()
        .StyleSources(s =>
            s.Self()
                .CustomSources(
                    "https://fonts.googleapis.com",
                    "sha256-47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="
                )
        )
        .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com", "data:"))
        .FormActions(s => s.Self())
        .FrameAncestors(s => s.Self())
        .ImageSources(s => s.Self().CustomSources("blob:", "data:", "https://res.cloudinary.com"))
);

//Add in developing mode Swagger and Swagger UI middleware to enable API documentation and exploration.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.Use(
        async (context, next) =>
        {
            context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000"); // 1 year
            context.Response.Headers.Append("Permissions-Policy", "camera=(), microphone=()");
            await next.Invoke();
        }
    );
}

// Referrer-Policy controls how much information about the referring page is sent with navigation requests.
app.UseReferrerPolicy(options => options.NoReferrer());

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();