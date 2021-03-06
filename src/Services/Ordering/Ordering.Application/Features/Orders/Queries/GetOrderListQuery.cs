using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries
{
    public class GetOrderListQuery:IRequest<List<OrderVm>>
    {
        public string UserName { get; set; }
        public GetOrderListQuery(string username)
        {
            UserName= username;
        }

    }
}
