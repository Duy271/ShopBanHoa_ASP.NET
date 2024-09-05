using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopBanHoa.Models;

namespace ShopBanHoa.Models
{
    public class View_GioHang
    {
        public int MaGioHang { get; set; }
        public int MaKhachHang { get; set; }
        public int MaHoa { get; set; }
        public string TenHoa { get; set; }

        public string AnhHoa { get; set; }
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public int ThanhTien { get; set; }
    }
}