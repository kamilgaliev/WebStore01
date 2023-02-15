using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Orders;

namespace WebStore.Services.Mapping
{
    public static class OrderMapper
    {
        public static OrderItemDTO ToDTO(this OrderItem Item) => Item is null
            ? null
            : new OrderItemDTO
            {
                Id= Item.Id,
                Price= Item.Price,
                Quantity= Item.Quantity,
            };

        public static OrderItem FromDTO(this OrderItemDTO Item) => Item is null
            ? null
            : new OrderItem
            {
                Id = Item.Id,
                Price = Item.Price,
                Quantity = Item.Quantity,
            };

        public static OrderDTO ToDTO(this Order Order) => Order is null
            ? null
            : new OrderDTO
            {
                Id = Order.Id,
                Name = Order.Name,
                Address = Order.Address,
                Phone = Order.Phone,
                Date = Order.Date,
                Items = Order.Items.Select(ToDTO), 
            };

        public static Order FromDTO(this OrderDTO Order) => Order is null
            ? null
            : new Order
            {
                Id = Order.Id,
                Name = Order.Name,
                Address = Order.Address,
                Phone = Order.Phone,
                Date = Order.Date,
                Items = Order.Items.Select(FromDTO).ToList(),
            };
    }
}
