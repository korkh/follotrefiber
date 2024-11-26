namespace Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public Address RegistrationAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TransportInfo TransportInfo { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFired { get; set; }
        public Contract ContractData { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
        public ICollection<EmployeeTimeCard> EmployeeTimeCards { get; set; } = new List<EmployeeTimeCard>();
        public
        ICollection<BusinessTrip> BusinessTrips
        { get; set; } = new List<BusinessTrip>();
    }
}
