using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    interface IOrderRepository
    {
        IEnumerable<Order> Orders { get; }
        void SaveOrder(Order order);

    }
}
