using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products.SearchProducts
{
    /// <summary>
    /// Contrato para el caso de uso de búsqueda de productos
    /// </summary>
    public interface ISearchProductsUseCase
    {
        Task<Result<IEnumerable<ProductDto>>> ExecuteAsync(ProductSearchDto searchDto, CancellationToken ct = default);
    }
}
