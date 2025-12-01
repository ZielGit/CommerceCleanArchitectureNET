using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products.UpdateProduct
{
    public interface IUpdateProductUseCase
    {
        Task<Result<ProductDto>> ExecuteAsync(Guid id, UpdateProductDto dto, CancellationToken ct = default);
    }
}
