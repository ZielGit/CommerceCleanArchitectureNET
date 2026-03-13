using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.DTOs
{
    /// <summary>
    /// DTO para búsqueda de productos con filtros
    /// </summary>
    public record ProductSearchDto(
        string? Name = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        bool? OnlyInStock = null,
        bool? OnlyActive = null
    );
}
