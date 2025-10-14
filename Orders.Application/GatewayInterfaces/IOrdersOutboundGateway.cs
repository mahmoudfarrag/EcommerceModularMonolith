using Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.GatewayInterfaces;
public interface IOrdersOutboundGateway
{
    Task ProcessAfterOrderCreated(Order order);
}
