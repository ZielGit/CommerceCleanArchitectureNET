using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.DTOs
{
    public record ProductDto(
        Guid Id,
        string Name,
        decimal Price,
        string Currency,
        int Stock,
        bool IsActive
    );
}
