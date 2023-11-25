using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_153503_BOBKO.Domain.Models;
using WEB_153503_BOBKO.Extensions;

namespace WEB_153503_BOBKO.Components
{
    public class CartViewComponent: ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
