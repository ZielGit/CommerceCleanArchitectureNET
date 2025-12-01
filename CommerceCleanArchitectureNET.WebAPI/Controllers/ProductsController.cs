using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.UseCases.Products;
using CommerceCleanArchitectureNET.Application.UseCases.Products.DeleteProduct;
using CommerceCleanArchitectureNET.Application.UseCases.Products.GetAllProducts;
using CommerceCleanArchitectureNET.Application.UseCases.Products.UpdateProduct;
using CommerceCleanArchitectureNET.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommerceCleanArchitectureNET.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IGetAllProductsUseCase _getAllProducts;
        private readonly ICreateProductUseCase _createProduct;
        private readonly IGetProductByIdUseCase _getProductById;
        private readonly IUpdateProductUseCase _updateProduct;
        private readonly IDeleteProductUseCase _deleteProduct;

        public ProductsController(
            IGetAllProductsUseCase getAllProducts,
            ICreateProductUseCase createProduct,
            IGetProductByIdUseCase getProductById,
            IUpdateProductUseCase updateProductStock,
            IDeleteProductUseCase deleteProduct)
        {
            _getAllProducts = getAllProducts;
            _createProduct = createProduct;
            _getProductById = getProductById;
            _updateProduct = updateProductStock;
            _deleteProduct = deleteProduct;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProducts(CancellationToken ct)
        {
            var result = await _getAllProducts.ExecuteAsync(ct);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Error!));

            return Ok(result.Value);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(
            [FromBody] CreateProductDto dto,
            CancellationToken ct)
        {
            var result = await _createProduct.ExecuteAsync(dto, ct);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Error!));

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = result.Value!.Id },
                result.Value);
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(Guid id, CancellationToken ct)
        {
            var result = await _getProductById.ExecuteAsync(id, ct);

            if (!result.IsSuccess)
                return NotFound(new ErrorResponse(result.Error!));

            return Ok(result.Value);
        }

        /// <summary>
        /// Update product stock
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductStock(
            Guid id,
            [FromBody] UpdateProductDto dto,
            CancellationToken ct)
        {
            var result = await _updateProduct.ExecuteAsync(id, dto, ct);

            if (!result.IsSuccess)
            {
                if (result.Error!.Contains("not found"))
                    return NotFound(new ErrorResponse(result.Error));

                return BadRequest(new ErrorResponse(result.Error));
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Delete product
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken ct)
        {
            var result = await _deleteProduct.ExecuteAsync(id, ct);

            if (!result.IsSuccess)
            {
                if (result.Error!.Contains("not found"))
                    return NotFound(new ErrorResponse(result.Error));

                return BadRequest(new ErrorResponse(result.Error));
            }

            return NoContent();
        }
    }
}
