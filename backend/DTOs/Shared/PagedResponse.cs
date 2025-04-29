namespace backend.DTOs.Shared
{
    public class PagedResponse<T>
    {
        public T data {  get; set; }

        public int pageNum { get; set; }

        public int pageSize { get; set; }

        public int totalCount { get; set; }
    }
    
}
