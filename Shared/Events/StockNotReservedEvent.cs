﻿using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StockNotReservedEvent : IStockNotReservedEvent
    {
        public StockNotReservedEvent(Guid corelationId)
        {
            CorrelationId = corelationId;
        }
        public string Reason { get; set; }

        public Guid CorrelationId { get; }
    }
}
