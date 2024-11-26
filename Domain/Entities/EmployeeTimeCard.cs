namespace Domain.Entities
{
    public class EmployeeTimeCard
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Month { get; set; }
        public int AvailableWorkingHoursPerMonth { get; set; }
        public ICollection<WorkingState> WorkingStates { get; set; } = new List<WorkingState>();
    }
}

