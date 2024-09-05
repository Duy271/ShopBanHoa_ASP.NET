using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanHoa.Models;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;


namespace ShopBanHoa.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        //public ActionResult Index()
        //{
        //    return View();
        //}
        string tenDangNhap;
        string anhDaiDien;
        public ActionResult TrangChu(int page = 1)
        {         
            using (DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext())
            {
                var hoa = db.Hoas.ToList();

                //page
                int NoOfRecordPerPage = 8;
                int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(hoa.Count()) / Convert.ToDouble(NoOfRecordPerPage)));
                int NoOfReCordToSkip = (page - 1) * NoOfRecordPerPage;
                ViewBag.Page = page;
                ViewBag.NoOfPages = NoOfPages;
                hoa = hoa.Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList();
                return View(hoa);
            }
        }
        //static string RemoveDiacritics(string text)
        //{
        //    string normalizedString = text.Normalize(NormalizationForm.FormD);
        //    var regex = new Regex("[^a-zA-Z0-9 ]");
        //    return regex.Replace(normalizedString, "");
        //}
        [HttpPost]
      
        public ActionResult Search(string keyword, int page = 1)
        {
           
            IEnumerable<ShopBanHoa.Models.Hoa> result;

            using (var db = new DB_ShopBanHoaDataContext())
            {
                int NoOfRecordPerPage = 8;
                int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(db.Hoas.Count()) / Convert.ToDouble(NoOfRecordPerPage)));
                int NoOfReCordToSkip = (page - 1) * NoOfRecordPerPage;
                ViewBag.Page = page;
                ViewBag.NoOfPages = NoOfPages;
                if (!string.IsNullOrEmpty(keyword))
                {
                    decimal minValue = 0;
                    decimal maxValue = 0;
                    bool isPriceRange = false;

                    if (keyword.Contains("-"))
                    {
                        string[] priceRange = keyword.Split('-');
                        if (priceRange.Length == 2 && decimal.TryParse(priceRange[0], out minValue) && decimal.TryParse(priceRange[1], out maxValue))
                        {
                            isPriceRange = true;
                        }
                    }
                    else
                    {
                        decimal specificPrice;
                        if (decimal.TryParse(keyword, out specificPrice))
                        {
                            minValue = specificPrice;
                            maxValue = specificPrice;
                            isPriceRange = true;
                        }
                    }

                    if (isPriceRange)
                    {
                        result = db.Hoas.Where(hoa => hoa.GiaBan >= minValue && hoa.GiaBan <= maxValue)
                                        .OrderBy(hoa => hoa.GiaBan).Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage)
                                        .ToList();
                    }
                    else
                    {
                        result = db.Hoas.Where(hoa => hoa.TenHoa.Contains(keyword))
                                        .OrderBy(hoa => hoa.GiaBan).Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage)
                                        .ToList();
                    }
                }
                else
                {
                    result = db.Hoas.OrderBy(hoa => hoa.GiaBan).Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList();
                }
            }

            return PartialView("P_Search", result);
        }
        public ActionResult GioHang()
        {
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }        
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }         
            string tdn = tenDangNhap;

            if(tdn==""||tdn==null)
            {
                return RedirectToAction("V_DangNhap", "Form");
            }
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy a = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tdn);
            int t = a.MaKhachHang;
            ViewBag.tenkh = a.TenKhachHang;
            ViewBag.diachikh = a.DiaChi;
            ViewBag.sdtkh = a.SoDienThoai;
            var gh = db.GioHangs.Select(m => new View_GioHang
            {
                MaGioHang = m.MaGioHang,
                MaKhachHang = (int)m.MaKhachHang,
                MaHoa = (int)m.MaHoa,
                TenHoa = m.Hoa.TenHoa,
                AnhHoa = m.Hoa.AnhHoa,
                DonGia = (int)m.Hoa.GiaBan,
                SoLuong = (int)m.SoLuong, 
                ThanhTien = (int)m.Hoa.GiaBan * (int)m.SoLuong
            }).Where(m=>m.MaKhachHang==t).ToList();
            
            return View(gh);
        }

        
        public ActionResult ThemGioHang(int maHoa)
        {
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
     
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
      
            int soLuong = 1;

            string tdn = tenDangNhap;
            if (tdn == "" || tdn == null)
            {
                return Json(new { success = false, redirect = "Form/V_DangNhap" });
            }

            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy a = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tdn);
            int kh = a.MaKhachHang;

            var hoa = db.Hoas.SingleOrDefault(h => h.MaHoa == maHoa);

            if (hoa != null)
            {
                
                var gioHangItem = db.GioHangs.SingleOrDefault(g => g.MaHoa == maHoa&g.MaKhachHang==kh);

                if (gioHangItem != null)
                {
                    
                    gioHangItem.SoLuong += soLuong;
                }
                else
                {

                    GioHang gioHang = new GioHang
                    {
                        MaHoa = maHoa,
                        SoLuong = soLuong,
                        MaKhachHang = (int)kh
                };
                    db.GioHangs.InsertOnSubmit(gioHang);
                }

                db.SubmitChanges();
            }

            return Json(new { success = true });

        }

        public ActionResult ChiTietSanPham(int masp)
        {


           
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            Hoa hoa = new Hoa();
            hoa = db.Hoas.SingleOrDefault(m => m.MaHoa == masp);
       
            ViewBag.anh1 = "hh1.jpg";
            ViewBag.anh2 = "hh2.jpg";
            ViewBag.anh3 = "hh3.jpg";
            ViewBag.anh4 = "hh4.jpg";
            var h = hoa;
            return View(h);
        }

        [HttpPost]
        public ActionResult DeleteSelected(int[] selectedAccounts)
        {
            try
            {
                if (selectedAccounts != null && selectedAccounts.Any())
                {
                    using (DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext())
                    {
                        var accountsToDelete = db.GioHangs.Where(d => selectedAccounts.Contains(d.MaGioHang));

                        db.GioHangs.DeleteAllOnSubmit(accountsToDelete);
                        db.SubmitChanges();
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        
        [HttpPost]
        public ActionResult DatHangVaXoaGioHang(int[] selectedProducts)
        {

            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
         
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
         
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            try
            {
                DangKy dk=db.DangKies.SingleOrDefault(m=>m.TenDangNhap==tenDangNhap);
                int makh=dk.MaKhachHang;
                // Lấy thông tin chi tiết đơn hàng từ giỏ hàng
                var chiTietDonHang = db.GioHangs
                    .Where(g => selectedProducts.Contains(g.MaGioHang))
                    .Select(g => new View_GioHang
                    {
                        MaGioHang = g.MaGioHang,
                        MaKhachHang = (int)g.MaKhachHang,
                        MaHoa = (int)g.MaHoa,
                        TenHoa = g.Hoa.TenHoa,
                        AnhHoa = g.Hoa.AnhHoa,
                        DonGia = (int)g.Hoa.GiaBan,
                        SoLuong = (int)g.SoLuong,
                        ThanhTien = (int)g.Hoa.GiaBan * (int)g.SoLuong
                        // Các thông tin khác nếu cần
                    }).Where(m=>m.MaKhachHang==makh)
                    .ToList();

        
                DonDatHang donHang = new DonDatHang
                {
                    // Gán các thông tin từ chiTietDonHang hoặc thêm logic lấy từng thông tin
                    MaKhachHang = chiTietDonHang.FirstOrDefault() != null ? chiTietDonHang.FirstOrDefault().MaKhachHang : 0,

                    NgayDatHang = DateTime.Now,
                    TenKhachHang=dk.TenKhachHang,
                    DiaChiGiaoHang=dk.DiaChi,
                    SoDienThoai=dk.SoDienThoai,
                    TrangThaiDonHang="Chưa Xử Lý",
                    TongGiaTriDonHang=chiTietDonHang.Sum(m=>m.ThanhTien)
                
                    // Các thông tin khác nếu cần
                };
                Session["DonHang"] = donHang;
                db.DonDatHangs.InsertOnSubmit(donHang);
                db.SubmitChanges();

                // Thêm thông tin chi tiết đơn hàng vào bảng ChiTietDonHang
                foreach (var chiTiet in chiTietDonHang)
                {
                    ChiTietDonDatHang chiTietDon = new ChiTietDonDatHang
                    {
                        MaDonHang = donHang.MaDonHang,
                        MaHoa = chiTiet.MaHoa,
                        SoLuong = chiTiet.SoLuong,
                        GiaBanLe = chiTiet.DonGia,
                        TongTien = chiTiet.ThanhTien
                        // Các thông tin khác nếu cần
                    };

                    db.ChiTietDonDatHangs.InsertOnSubmit(chiTietDon);
                }

                // Xóa sản phẩm đã đặt hàng khỏi giỏ hàng
                foreach (var maGioHang in selectedProducts)
                {
                    var gioHangItem = db.GioHangs.SingleOrDefault(g => g.MaGioHang == maGioHang);
                    if (gioHangItem != null)
                    {
                        db.GioHangs.DeleteOnSubmit(gioHangItem);
                    }
                }

                db.SubmitChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult ThongTinDatHang()
        {
            DonDatHang dondathang = Session["DonHang"] as DonDatHang;
            ViewBag.DoDatHang = dondathang;
            int ddh = dondathang.MaDonHang;
            ViewBag.TongTien = dondathang.TongGiaTriDonHang;
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            var dsctdh = db.ChiTietDonDatHangs.Where(m => m.MaDonHang == ddh).ToList();
            //ChiTietDonDatHang anh = db.ChiTietDonDatHangs.FirstOrDefault(m => m.MaDonHang == ddh);
            //
            List<Hoa> lst_a = db.Hoas.ToList();
            ViewBag.lst_a = lst_a;


            int a1 = 1;
            int a2 = 2;
            int a3 = 3;
            int a4 = a3 + 1;
          
            
          
          
            Hoa anh1 = db.Hoas.SingleOrDefault(m => m.MaHoa == a1);
            Hoa anh2 = db.Hoas.SingleOrDefault(m => m.MaHoa == a2);
            Hoa anh3 = db.Hoas.SingleOrDefault(m => m.MaHoa == a3);
            Hoa anh4 = db.Hoas.SingleOrDefault(m => m.MaHoa == a4);
         
            ViewBag.anh1 = anh1;
            ViewBag.anh2 = anh2;
            ViewBag.anh3 = anh3;
            return View(dsctdh);
        }

        [HttpPost]
        public ActionResult CapNhatThongTinGiaoHang(string ten,string dc,string sdt)
        {
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DonDatHang dondathang = Session["DonHang"] as DonDatHang;
            var update_tt = db.DonDatHangs.SingleOrDefault(m => m.MaDonHang == dondathang.MaDonHang);
            update_tt.TenKhachHang = ten;
            update_tt.DiaChiGiaoHang = dc;
            update_tt.SoDienThoai = sdt;
            var update_dk = db.DangKies.SingleOrDefault(m => m.MaKhachHang == dondathang.MaKhachHang);
            update_dk.TenKhachHang = ten;
            update_dk.DiaChi = dc;
            update_dk.SoDienThoai = sdt;
            db.SubmitChanges();
            return RedirectToAction("TrangChu", "Home");
        }

        public ActionResult XemDonHang()
        {
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
         
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
      
            if(tenDangNhap==null||tenDangNhap=="")
            {
                return RedirectToAction("V_DangNhap", "Form");
            }
            
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy dk = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tenDangNhap);
            int mkh = dk.MaKhachHang;
            var donhang = db.DonDatHangs.Where(m =>m.MaKhachHang==mkh&m.TrangThaiDonHang != "Sắp Đến Bạn" & m.TrangThaiDonHang != "Đã Thanh Toán"&m.MaKhachHang==mkh&m.TrangThaiDonHang!="Đã Hủy").ToList();
            return View(donhang);
        }

        public ActionResult DonSapDen()
        {
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }

            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }

            if (tenDangNhap == null || tenDangNhap == "")
            {
                return RedirectToAction("V_DangNhap", "Form");
            }

            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy dk = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tenDangNhap);
            int mkh = dk.MaKhachHang;
            var donhang = db.DonDatHangs.Where(m => m.TrangThaiDonHang == "Sắp Đến Bạn"&m.MaKhachHang==mkh).ToList();
            return View(donhang);
        }

        public ActionResult XemChiTietDonHang(int ma)
        {
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            var a = db.ChiTietDonDatHangs.Where(m => m.MaDonHang == ma).ToList();
            List<Hoa> hoa = db.Hoas.ToList();
            ViewBag.hoa = hoa;
            return View(a);
        }

        public ActionResult XoaChiTietDonHang(int ma)
        {
            try
            {
                using (DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext())
                {
                    // Lấy chi tiết đơn hàng cần xóa
                    var chiTietDonHang = db.ChiTietDonDatHangs.SingleOrDefault(ct => ct.MaChiTietDonHang == ma);

                    if (chiTietDonHang != null)
                    {
                        // Kiểm tra trạng thái đơn hàng
                        var donDatHang = db.DonDatHangs.SingleOrDefault(dh => dh.MaDonHang == chiTietDonHang.MaDonHang);

                        if (donDatHang != null && donDatHang.TrangThaiDonHang == "Chưa Xử Lý")
                        {
                            // Xóa chi tiết đơn hàng
                            db.ChiTietDonDatHangs.DeleteOnSubmit(chiTietDonHang);
                            db.SubmitChanges();

                            return Json(new { success = true, message = "Xóa chi tiết đơn hàng thành công." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Không thể xóa chi tiết đơn hàng do đơn hàng đã được xử lý." });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy chi tiết đơn hàng." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }
        public ActionResult LichSuMuaHang()
        {
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
         
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
      
            if (tenDangNhap == null || tenDangNhap == "")
            {
                return RedirectToAction("V_DangNhap", "Form");
            }

            //
            
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy dk = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tenDangNhap);
            int mkh = dk.MaKhachHang;
            var donhang = db.DonDatHangs.Where(m =>m.TrangThaiDonHang == "Đã Thanh Toán" & m.MaKhachHang == mkh).ToList();
            return View(donhang);

          
        }

        public ActionResult HuyDonHang(int ma)
        {
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DonDatHang dh = db.DonDatHangs.SingleOrDefault(m => m.MaDonHang == ma);
            if(dh.TrangThaiDonHang=="Chưa Xử Lý")
            {
                dh.TrangThaiDonHang = "Đã Hủy";
                db.SubmitChanges();
            }
            
            return RedirectToAction("XemDonHang", "Home");
        }
        public ActionResult XoaHoaTrongGio(int ma)
        {
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            GioHang gh = db.GioHangs.SingleOrDefault(m => m.MaGioHang == ma);
            db.GioHangs.DeleteOnSubmit(gh);
            db.SubmitChanges();
            return RedirectToAction("GioHang", "Home");
        }
        //
        public ActionResult DoiThongTin()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
         
       
        
            //
            string tdn = tenDangNhap;
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy a = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tdn);
            ViewBag.tt = a;
            return View();
        }
        [HttpPost]
        public ActionResult DoiThongTin(string ten, string dc, string sdt, string em, HttpPostedFileBase fileUpload)
        {
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }

            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }

            if (tenDangNhap == null || tenDangNhap == "")
            {
                return RedirectToAction("V_DangNhap", "Form");
            }
     
            //
            string tdn = tenDangNhap;
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();

            // Tìm thông tin tài khoản cần cập nhật
            var taiKhoan = db.DangKies.SingleOrDefault(d => d.TenDangNhap == tdn);

            if (taiKhoan != null)
            {
                // Kiểm tra và cập nhật thông tin nếu có dữ liệu mới
                if (!string.IsNullOrEmpty(ten))
                {
                    taiKhoan.TenKhachHang = ten;
                }

                if (!string.IsNullOrEmpty(dc))
                {
                    taiKhoan.DiaChi = dc;
                }

                if (!string.IsNullOrEmpty(sdt))
                {
                    taiKhoan.SoDienThoai = sdt;
                }

                if (!string.IsNullOrEmpty(em))
                {
                    taiKhoan.Email = em;
                }

                // Kiểm tra và lưu ảnh nếu có
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    string fileName = SaveFileAndReturnPath_pf(fileUpload);
                    taiKhoan.AnhDaiDien = fileName;
                }


                db.SubmitChanges();
                DangKy dkk = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tdn);
                string newanh = dkk.AnhDaiDien;



                // Nếu cookie tồn tại, cập nhật giá trị mới
                if (cookieanh != null)
                {
                    cookieanh.Value = newanh; // Sử dụng newanh thay vì newAnhDaiDien nếu đó là giá trị bạn muốn cập nhật
                    Response.SetCookie(cookieanh); // Sử dụng Response.SetCookie() để cập nhật cookie ngay lập tức
                }
                return RedirectToAction("DoiThongTin", "Home");
            }

            ViewBag.tb = "Cập Nhật Thất Bại";
            return RedirectToAction("DoiThongTin", "Home");
        }


        private string SaveFileAndReturnPath_pf(HttpPostedFileBase fileUpload)
        {

            //
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
            var path = Path.Combine(Server.MapPath("~/Content/USER"), fileName);

            fileUpload.SaveAs(path);

            return fileName;
        }
        public ActionResult DangXuat()
        {
            Response.Cookies["TenDangNhap"].Expires = DateTime.Now.AddDays(-1);

            // Xóa cookie AnhDaiDien
            Response.Cookies["AnhDaiDien"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("V_DangNhap", "Form");
        }

        public ActionResult DoiMatKhau()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
          
            //
            string tdn = tenDangNhap;
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy a = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tdn);
            ViewBag.tt = a;
            return View();
        }

        [HttpPost]
        public ActionResult DoiMatKhau(string tk, string mkc, string mkm, string xnmkm)
        {
            Function a = new Function();
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy chk = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tk);

            string mk = a.CalculateMD5Hash(mkc);
            if (mk == chk.MatKhau)
            {
                chk.MatKhau = a.CalculateMD5Hash(mkm);
                db.SubmitChanges();
                ViewBag.tt = chk;
                ViewBag.tb = "Đổi Mật Khẩu Thành Công";
                return View();
            }
            else
            {
                ViewBag.tb = "Sai Mật Khẩu";
                ViewBag.tt = chk;
            }
            ViewBag.tt = chk;
            return View();
        }

        public ActionResult TB_GioHang()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }

            int gh = 0;
            string tdn = tenDangNhap;
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy dk=db.DangKies.SingleOrDefault(m=>m.TenDangNhap==tdn);
            if(dk!=null)
            {
                int mkh = dk.MaKhachHang;
                gh = db.GioHangs.Where(m => m.MaKhachHang == mkh).Count();
            }
           
            return View(gh);
        }
        public ActionResult TB_DonHang()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            int gh = 0;
            //
            string tdn = tenDangNhap;
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy dk = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tdn);
            if(dk!=null)
            {
                int mkh = dk.MaKhachHang;
                gh = db.DonDatHangs.Where(m => m.MaKhachHang == mkh & m.TrangThaiDonHang != "Sắp Đến Bạn" & m.TrangThaiDonHang != "Đã Thanh Toán" & m.TrangThaiDonHang != "Đã Hủy").Count();
            }
            
            return View(gh);
        }
        public ActionResult TB_SapDen()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            int gh = 0;
            //
            string tdn = tenDangNhap;
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy dk = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tdn);
            if(dk!=null)
            {
                int mkh = dk.MaKhachHang;
                gh = db.DonDatHangs.Where(m => m.MaKhachHang == mkh & m.TrangThaiDonHang == "Sắp Đến Bạn").Count();
            }
           
            return View(gh);
        }
        public ActionResult TenDangNhap()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
         
            //
            string tdn = tenDangNhap;
            return View("TenDangNhap", (object)tdn);

        }
        public ActionResult DatTuChiTiet(int ma, int slm)
        {
            try
            {
                int sl = slm;
                ViewBag.anhDaiDien = "avt.jpg";
                HttpCookie cookietk = Request.Cookies["TenDangNhap"];
                tenDangNhap = cookietk != null ? cookietk.Value : null;

                if (string.IsNullOrEmpty(tenDangNhap))
                {
                    // Handle the case where tenDangNhap is null or empty
                    throw new Exception("Username is null or empty.");
                }

                DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
                Hoa sp = db.Hoas.SingleOrDefault(m => m.MaHoa == ma);
                DangKy dk = db.DangKies.SingleOrDefault(m => m.TenKhachHang == tenDangNhap);

                if (sp == null || dk == null)
                {
                    // Handle the case where a record is not found in the database
                    throw new Exception("Product or user not found in the database.");
                }

                DonDatHang dh = new DonDatHang
                {
                    MaKhachHang = dk.MaKhachHang,
                    NgayDatHang = DateTime.Now,
                    TenKhachHang = dk.TenKhachHang,
                    DiaChiGiaoHang = dk.DiaChi,
                    SoDienThoai = dk.SoDienThoai,
                    TrangThaiDonHang = "Chưa Xử Lý",
                    TongGiaTriDonHang = sp.GiaBan * sl
                };

                db.DonDatHangs.InsertOnSubmit(dh);
                db.SubmitChanges();

                ChiTietDonDatHang ct = new ChiTietDonDatHang
                {
                    MaDonHang = dh.MaDonHang,
                    MaHoa = ma,
                    SoLuong = sl,
                    GiaBanLe = sp.GiaBan,
                    TongTien = sl * sp.GiaBan
                };

                db.ChiTietDonDatHangs.InsertOnSubmit(ct);
                db.SubmitChanges();

                return RedirectToAction("TrangChu", "Home");
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return RedirectToAction("Error", "Home"); // Redirect to an error page or take appropriate action
            }
        }

        public ActionResult AnhDaiDien()
        {
           
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
          
            //
            string tdn = anhDaiDien;
            return View("AnhDaiDien", (object)tdn);
        }
    }
}