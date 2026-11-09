namespace RaceDay.Web.ViewModels
{
    public class MyEnrolmentsViewModel
    {
        public int EnrolmentID { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string CategoryName { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Confirmed"
    }
}