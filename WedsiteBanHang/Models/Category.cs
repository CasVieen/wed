namespace WedsiteBanHang.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } // Hoặc CategoryName tùy project của bạn

        // THÊM DÒNG NÀY VÀO MODEL:
        public string? Description { get; set; }
    }
}