namespace CommerceCleanArchitectureNET.Application.DTOs
{
    public record PagedProductsDto(
        IEnumerable<ProductDto> Data,
        int Total,
        int Page,
        int PageSize,
        int TotalPages
    );
}
