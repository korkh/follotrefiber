namespace Domain.Entities
{
    public class WorkingState
    {
        public int Id { get; set; }
        public Guid EmployeeTimeCardId { get; set; }
        public EmployeeTimeCard EmployeeTimeCard { get; set; }
        public int Day { get; set; }
        public bool Holiday { get; set; }
        public string State { get; set; }
    }
}