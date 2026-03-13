using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.DTOs
{
    public record CreateProductDto(
        string Name,
        decimal Price,
        string Currency,
        int Stock
    );
}
