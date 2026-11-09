namespace RaceDay.Web.ViewModels
{
    public class EventDetailsViewModel
    {
        public EventViewModel Event { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }

    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public int EventID { get; set; }
        public string CategoryName { get; set; }
        public double DistanceKm { get; set; }
        public decimal EntryFee { get; set; } // Changed from Price
        public int MaxParticipants { get; set; }
    }
}