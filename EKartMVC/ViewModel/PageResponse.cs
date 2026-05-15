namespace EKartMVC.ViewModel
{
    public class PagedResponse<T>
    {
        public bool Success { get; set; }
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
