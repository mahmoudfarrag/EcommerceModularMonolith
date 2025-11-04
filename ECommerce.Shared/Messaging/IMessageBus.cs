using ECommerce.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Messaging;
public interface IMessageBus
{
    Task PublishAsync(IIntegrationEvent @event);
}
