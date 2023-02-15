using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Interfaces;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase,IOrderService
    {
        private readonly IOrderService _OrderService;

        public OrdersApiController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpPost("{UserName}")]
        public Task<OrderDTO> CreateOrder(string UserName,[FromBody] CreateOrderModel OrderModel)
        {
            return _OrderService.CreateOrder(UserName, OrderModel);
        }

        [HttpGet("{id}")]
        public Task<OrderDTO> GetOrderById(int id)
        {
            return _OrderService.GetOrderById(id);
        }

        [HttpGet("user/{UserName}")]
        public Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName)
        {
            return _OrderService.GetUserOrders(UserName);
        }
    }
}
