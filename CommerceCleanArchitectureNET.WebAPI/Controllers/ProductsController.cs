using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.UseCases.Products;
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
        private readonly ICreateProductUseCase _createProduct;
        private readonly IGetProductByIdUseCase _getProductById;

        public ProductsController(
            ICreateProductUseCase createProduct,
            IGetProductByIdUseCase getProductById)
        {
            _createProduct = createProduct;
            _getProductById = getProductById;
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
    }
}
