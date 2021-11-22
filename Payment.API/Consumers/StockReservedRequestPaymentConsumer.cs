using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.API.Consumers
{
    public class StockReservedRequestPaymentConsumer : IConsumer<IStockReservedRequestPayment>
    {
        private readonly ILogger<StockReservedRequestPaymentConsumer> _logger;
        private readonly IPublishEndpoint _publisherEndpoint;
        public StockReservedRequestPaymentConsumer(ILogger<StockReservedRequestPaymentConsumer> logger, IPublishEndpoint publisherEndpoint)
        {
            _logger = logger;
            _publisherEndpoint = publisherEndpoint;
        }
        public async Task Consume(ConsumeContext<IStockReservedRequestPayment> context)
        {
            var balance = 3000m;//müşteri bakiyesi belirledik
            if (balance > context.Message.payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.payment.TotalPrice} TL was withdrawn from credit card for user ID = { context.Message.BuyerId }");

                await _publisherEndpoint.Publish(
                new PaymentCompletedEvent(context.Message.CorrelationId));
            }
            else
            {
                _logger.LogInformation($"{context.Message.payment.TotalPrice} TL was not withdrawn from credit card for user ID = { context.Message.BuyerId }");

                await _publisherEndpoint.Publish(
                new PaymentFailedEvent(context.Message.CorrelationId)
                {
                    Reason = "not enough balance",
                    OrderItems = context.Message.OrderItems
                });
            }
        }
    }
}
