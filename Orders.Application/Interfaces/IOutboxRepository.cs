using ECommerce.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Interfaces;
public interface IOutboxRepository
{
    Task AddAsync(IIntegrationEvent @event);
}
