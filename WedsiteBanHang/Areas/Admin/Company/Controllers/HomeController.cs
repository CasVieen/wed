using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WedsiteBanHang.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = "Company")]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        // GET: /Company/Home/Index (Trang Dashboard chính)
        public IActionResult Index()
        {
            // Có thể query từ DB và truyền dữ liệu thống kê ra View
            ViewBag.TotalProducts = 12;
            ViewBag.PendingOrders = 5;
            ViewBag.TotalRevenue = 15500000;

            return View();
        }
    }
}