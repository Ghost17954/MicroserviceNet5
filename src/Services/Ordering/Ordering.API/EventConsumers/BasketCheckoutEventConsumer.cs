using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using System.Threading.Tasks;

namespace Ordering.API.EventConsumers
{
    public class BasketCheckoutEventConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<BasketCheckoutEventConsumer> _logger;

        public BasketCheckoutEventConsumer(IMapper mapper, IMediator mediator, ILogger<BasketCheckoutEventConsumer> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var checkoutOrder = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(checkoutOrder);

            _logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);
        }
    }
}
