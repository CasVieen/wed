using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WedsiteBanHang.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.PendingOrders = 15;   // Đơn hàng chờ duyệt
            ViewBag.ShippingOrders = 8;    // Đơn hàng đang giao
            ViewBag.CompletedToday = 24;  // Đơn hàng hoàn tất hôm nay

            return View();
        }
    }
}