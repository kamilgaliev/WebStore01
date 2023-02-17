using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Interfaces;
using WebStore.Services.Mapping;

namespace WebStore.Services.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public async Task<OrderDTO> CreateOrder(string UserName, CreateOrderModel OrderModel)
        {
            var user = await _UserManager.FindByNameAsync(UserName);

            if (user is null)
                throw new InvalidOperationException($"Пользователь {UserName} не найден в БД");

            await using var transaction =await _db.Database.BeginTransactionAsync().ConfigureAwait(false);

            var order = new Order
            {
                Name = OrderModel.Order.Name,
                Address = OrderModel.Order.Address,
                Phone = OrderModel.Order.Phone,
                User = user,
            };

            //var product_ids = Cart.Items.Select(item => item.Product.Id).ToArray();

            //var cart_products = await _db.Products
            //    .Where(p => product_ids.Contains(p.Id)).ToArrayAsync();

            //order.Items = Cart.Items.Join(
            //    cart_products, cart_item => cart_item.Product.Id,
            //    product => product.Id,
            //    (cart_item, product) => new OrderItem
            //    {
            //        Order = order,
            //        Product = product,
            //        Quantity = cart_item.Quantity,
            //        Price = product.Price,
            //    }).ToArray();

            foreach (var item in OrderModel.Items)
            {
                var product = await _db.Products.FindAsync(item.Id);

                if (product is null) continue;

                var order_item = new OrderItem
                {
                    Id = item.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Product = product,
                };
                order.Items.Add(order_item);
            }

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return order.ToDTO();
        }

        public async Task<OrderDTO> GetOrderById(int id) => (await _db.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id)).ToDTO();

        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName) => (await _db.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .Where(order => order.User.UserName == UserName)
            .ToArrayAsync()).Select(order => order.ToDTO());
    }
}
