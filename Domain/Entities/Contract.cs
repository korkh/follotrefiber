namespace Domain.Entities
{
    public class Contract
    {
        public string Position { get; set; }
        public string ContractNumber { get; set; }
        public DateTime AcceptionDate { get; set; }
        public DateTime DismissalDate { get; set; }
        public int EmploymentDays => (DismissalDate - AcceptionDate).Days;
        public int AnnualHolidays { get; set; }
        public int FatherHolidays { get; set; }
        public int UnpaidHolidays { get; set; }
        public int TruancyDays { get; set; }
        public int AllowedAbsenceDays { get; set; }
        public int UnusedHolidays =>
            ((EmploymentDays + 1 - TruancyDays - UnpaidHolidays) * 20 / 365) - AnnualHolidays;
    }
}
