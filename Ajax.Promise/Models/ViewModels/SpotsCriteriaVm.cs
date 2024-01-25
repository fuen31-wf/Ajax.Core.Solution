namespace Ajax.Promise.Models.ViewModels
{
    public class SpotsCriteriaVm
    {
        //搜尋相關
        public string? keyword { get; set; }

        public int? categoryId { get; set; } = 1;


        //排序相關
        public string? sortBy { get; set; }

        public string? sortType { get; set; } = "asc"; //desc

        //分頁相關
        public int? page { get; set; } = 1;

        public int? pageSize { get; set; } = 9;
    }
}
