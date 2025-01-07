namespace LostAndFound.API.Models.Dtos
{
    public class DashboardStats
    {
        public int TotalItems { get; set; }
        public int LostItems { get; set; }
        public int FoundItems { get; set; }
        public int PendingItems { get; set; }
    }
}