namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments
{
    public class ProjectPayments
    {
        public string ProjectId { get; set; }
        public IEnumerable<Payment> Payments { get; set; }
    }
    public class Payment
    {
        public int? PaymentIndex { get; set; }
        public decimal? PaymentScheduleAmount { get; set; }
        public DateTime? PaymentScheduleDate { get; set; }
        public decimal? PaymentActualAmount { get; set; }
        public DateTime? PaymentActualDate { get; set; }
    }

}
