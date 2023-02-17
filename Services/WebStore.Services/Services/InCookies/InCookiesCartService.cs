using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;
using WebStore.Services.Mapping;

namespace WebStore.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IProductData _ProductData;
        private readonly string _CartName;

        private Cart Cart
        {
            get 
            {
                var context = _HttpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;
                var cart_cookies = context.Request.Cookies[_CartName];

                if(cart_cookies is null)
                {
                    var cart = new Cart();
                    cookies.Append(_CartName,JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookies(cookies, cart_cookies);
                return JsonConvert.DeserializeObject<Cart>(cart_cookies);
            }
            set => ReplaceCookies(_HttpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCookies(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cookie);
        }

        public InCookiesCartService(IHttpContextAccessor HttpContextAccessor, IProductData ProductData)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _ProductData = ProductData;
            var user = HttpContextAccessor.HttpContext?.User;
            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WebStore.Cart{user_name}";
        }
        public void Add(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(item => item.ProductId == id);
            if (item == null)
                cart.Items.Add(new CartItem { ProductId = id });
            else
                item.Quantity++;

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(item => item.ProductId == id);
            if (item == null) return;
            
            if(item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity == 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(items => items.ProductId).ToArray()
            });

            var productViewModel = products.FromDTO().ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items
                .Where(item => productViewModel.ContainsKey(item.ProductId))
                .Select(item => (productViewModel[item.ProductId],item.Quantity))
            };
        }

        public void Remove(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(item => item.ProductId == id);
            if (item == null) return;

            cart.Items.Remove(item);

            Cart = cart;
        }
    }
}
