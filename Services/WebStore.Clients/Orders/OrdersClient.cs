using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain.DTO;
using WebStore.Interfaces;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(IConfiguration Configuration) : base(Configuration, WebAPI.Orders)
        {
        }

        public async Task<OrderDTO> CreateOrder(string UserName, CreateOrderModel OrderModel)
        {
            var response = await PostAsync($"{Address}/{UserName}", OrderModel);

            return await response.Content.ReadAsAsync<OrderDTO>();
        }

        public async Task<OrderDTO> GetOrderById(int id) => await GetAsync<OrderDTO>($"{Address}/{id}");

        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName) => 
            await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{UserName}");
    }
}
