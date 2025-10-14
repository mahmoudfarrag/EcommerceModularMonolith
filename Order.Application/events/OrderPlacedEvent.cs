using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.events;
public record OrderPlacedEvent(Guid OrderId, Guid ProductId, decimal Price) : INotification;
