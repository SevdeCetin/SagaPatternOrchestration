using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.Messages
{
    public interface IStockRollBack
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
