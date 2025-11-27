using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.DTOs
{
    public record UpdateProductStockDto(
        Guid ProductId,
        int Stock
    );
}
