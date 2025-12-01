using CommerceCleanArchitectureNET.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products.DeleteProduct
{
    public interface IDeleteProductUseCase
    {
        Task<Result<bool>> ExecuteAsync(Guid id, CancellationToken ct = default);
    }
}
