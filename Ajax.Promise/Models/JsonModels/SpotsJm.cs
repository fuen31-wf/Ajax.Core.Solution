namespace Ajax.Promise.Models.JsonModels
{
    public class SpotsJm
    {
        public int totalPages { get; set; }
        public int currentPage { get; set; } = 1;
        public List<SpotImagesSpot> spots { get; set; } = new List<SpotImagesSpot>();
        public int totalCount { get; set; }
    }
}
