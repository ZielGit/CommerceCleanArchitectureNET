using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.Interfaces;
using CommerceCleanArchitectureNET.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products.DeleteProduct
{
    public class DeleteProductUseCase : IDeleteProductUseCase
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductUseCase(
            IProductRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> ExecuteAsync(
            Guid id,
            CancellationToken ct = default)
        {
            try
            {
                var exists = await _repository.ExistsAsync(id, ct);

                if (!exists)
                    return Result<bool>.Failure("Product not found");

                await _repository.DeleteAsync(id, ct);
                await _unitOfWork.CommitAsync(ct);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting product: {ex.Message}");
            }
        }
    }
}
