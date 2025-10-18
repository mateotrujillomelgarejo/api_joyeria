namespace api_joyeria.DTOs.Shared
{
    public class PaginacionResponse
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
    }
}
