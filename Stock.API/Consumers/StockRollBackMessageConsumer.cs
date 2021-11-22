using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces.Messages;
using Stock.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Consumers
{
    public class StockRollBackMessageConsumer : IConsumer<StockRollBackMessage>
    {

        private readonly ILogger<StockRollBackMessageConsumer> _logger;
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockRollBackMessageConsumer(ILogger<StockRollBackMessageConsumer> logger, AppDbContext context, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _context = context;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<StockRollBackMessage> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                if (stock != null)
                {
                    stock.Count += item.Count;
                    await _context.SaveChangesAsync();
                }


            }
            _logger.LogInformation("Stock was released ");
        }
    }
}
