using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products
{
    public interface ICreateProductUseCase
    {
        Task<Result<ProductDto>> ExecuteAsync(CreateProductDto dto, CancellationToken ct = default);
    }
}
