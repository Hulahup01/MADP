using Microsoft.AspNetCore.Mvc;

namespace WEB_153503_BOBKO.ViewComponents
{
    public class CartComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
