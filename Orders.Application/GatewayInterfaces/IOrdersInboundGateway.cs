using Catalog.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.GatewayInterfaces;
public interface IOrdersInboundGateway
{
    Task<ProductDto?> GetProductAsync(Guid id);
    Task<CategoryDto?> GetCategoryAsync(Guid id);
}
