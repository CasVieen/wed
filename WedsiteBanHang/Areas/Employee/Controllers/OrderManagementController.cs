using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WedsiteBanHang.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class OrderManagementController : Controller
    {
        // GET: /Employee/OrderManagement/Index
        public IActionResult Index()
        {
            // Sau này bạn query danh sách Đơn hàng từ DB ra đây
            return View();
        }

        // GET: /Employee/OrderManagement/Details/5
        public IActionResult Details(int id)
        {
            // Xem chi tiết đơn hàng
            return View();
        }

        // POST: /Employee/OrderManagement/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int orderId, string status)
        {
            // Cập nhật trạng thái đơn (VD: "Đã xác nhận", "Đang giao", "Đã giao")
            TempData["Message"] = $"Cập nhật đơn hàng #{orderId} sang trạng thái '{status}' thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}