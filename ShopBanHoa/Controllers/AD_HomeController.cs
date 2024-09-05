using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanHoa.Models;
using System.IO;

namespace ShopBanHoa.Controllers
{
    public class AD_HomeController : Controller
    {
        //
        // GET: /AD_Home/
        public string tenDangNhap;
        public string anhDaiDien;
        public ActionResult TrangChu(int page = 1)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            using (var db = new DB_ShopBanHoaDataContext())
            {

                //page
                int NoOfRecordPerPage = 8;
                int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(db.Hoas.Count()) / Convert.ToDouble(NoOfRecordPerPage)));
                int NoOfReCordToSkip = (page - 1) * NoOfRecordPerPage;
                ViewBag.Page = page;
                ViewBag.NoOfPages = NoOfPages;

                return View(db.Hoas.Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList());
            }
        }

        [HttpPost]
        public ActionResult Search(string keyword,int page=1)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
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


        public ActionResult QL_TaiKhoan(int page = 1)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
       
            using (var db = new DB_ShopBanHoaDataContext())
            {
                int NoOfRecordPerPage = 6;
                int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(db.DangKies.Count()) / Convert.ToDouble(NoOfRecordPerPage)));
                int NoOfReCordToSkip = (page - 1) * NoOfRecordPerPage;
                ViewBag.Page = page;
                ViewBag.NoOfPages = NoOfPages;
                var tk = db.DangKies.ToList();
                tk = tk.Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList();
                return View(tk);
            }

        }

        [HttpPost]
        public ActionResult Search_QLTK(string taikhoan,int page=1)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
          
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            int NoOfRecordPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(db.DangKies.Count()) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfReCordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            List<DangKy> listSearch = new List<DangKy>();
            if (taikhoan != null)
            {
                listSearch = db.DangKies.Where(m => m.TenDangNhap.Contains(taikhoan)).Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList();
                return View("QL_TaiKhoan", listSearch);
            }
            else
                return View("QL_TaiKhoan");
        }
        [HttpPost]
        public ActionResult BlockOrUn(string taikhoan, string action)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
        
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            var capnhat = db.DangKies.SingleOrDefault(m => m.TenDangNhap == taikhoan);

            if (capnhat != null)
            {
                if (action == "lock" && capnhat.TrangThai == "Duoc Dung")
                {
                    capnhat.TrangThai = "Chan";
                }
                else if (action == "unlock" && capnhat.TrangThai == "Chan")
                {
                    capnhat.TrangThai = "Duoc Dung";
                }

                db.SubmitChanges();
            }

            return RedirectToAction("QL_TaiKhoan");
        }

        [HttpPost]
        public ActionResult Delete_TK(string taikhoan)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
     
            using (DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext())
            {
                var dangKy = db.DangKies.SingleOrDefault(m => m.TenDangNhap == taikhoan);

                if (dangKy != null)
                {
                    // Kiểm tra các quan hệ khóa ngoại
                    bool canDelete = CanDeleteAccount(dangKy);

                    if (canDelete)
                    {
                        // Xóa tài khoản
                        db.DangKies.DeleteOnSubmit(dangKy);
                        db.SubmitChanges();

                        // Hiển thị thông báo JavaScript
                        ViewBag.Script = "alert('Xóa tài khoản thành công!');";
                    }
                    else
                    {
                        ViewBag.Error = "Không thể xóa tài khoản vì có dữ liệu liên quan.";
                    }
                }
                else
                {
                    ViewBag.Error = "Tài khoản không tồn tại.";
                }
            }

            // Hiển thị trang QL_TaiKhoan với thông báo lỗi hoặc thành công
            return RedirectToAction("QL_TaiKhoan", "AD_Home");
        }


        private bool CanDeleteAccount(DangKy dangKy)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
      
            // Kiểm tra liên kết với đơn đặt hàng
            if (dangKy.DonDatHangs.Any())
            {
                ViewBag.Error = "Không thể xóa tài khoản vì có đơn đặt hàng liên quan.";
                return false;
            }

            // Kiểm tra liên kết với bình luận
            if (dangKy.BinhLuans.Any())
            {
                ViewBag.Error = "Không thể xóa tài khoản vì có bình luận liên quan.";
                return false;
            }

            // Kiểm tra liên kết với giỏ hàng
            if (dangKy.GioHangs.Any())
            {
                ViewBag.Error = "Không thể xóa tài khoản vì có giỏ hàng liên quan.";
                return false;
            }

            // Thêm các kiểm tra khác tùy thuộc vào mô hình dữ liệu của bạn

            return true;
        }

        [HttpPost]
        public ActionResult DeleteSelected(string[] selectedAccounts)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
        
            if (selectedAccounts != null && selectedAccounts.Any())
            {
                using (DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext())
                {
                    List<string> accountsWithForeignKey = new List<string>();

                    foreach (var account in selectedAccounts)
                    {
                        var dangKy = db.DangKies.SingleOrDefault(d => d.TenDangNhap == account);

                        // Kiểm tra các quan hệ khóa ngoại
                        if (!CanDeleteAccount(dangKy))
                        {
                            accountsWithForeignKey.Add(account);
                        }
                    }

                    if (accountsWithForeignKey.Any())
                    {
                        // Nếu có tài khoản có khóa ngoại, trả về một phản hồi JSON để thông báo về tình trạng
                        return Json(new { success = false, message = "Không thể xóa tài khoản vì có dữ liệu liên quan.", accountsWithForeignKey });
                    }

                    // Nếu không có vấn đề với khóa ngoại, thực hiện xóa
                    var accountsToDelete = db.DangKies.Where(d => selectedAccounts.Contains(d.TenDangNhap)).ToList();
                    db.DangKies.DeleteAllOnSubmit(accountsToDelete);
                    db.SubmitChanges();
                }
            }

           
            return Json(new { success = true });
        }

      
        public ActionResult QL_SanPham(int page=1)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
         
            using (var db = new DB_ShopBanHoaDataContext())
            {
                int NoOfRecordPerPage = 6;
                int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(db.Hoas.Count()) / Convert.ToDouble(NoOfRecordPerPage)));
                int NoOfReCordToSkip = (page - 1) * NoOfRecordPerPage;
                ViewBag.Page = page;
                ViewBag.NoOfPages = NoOfPages;
                return View(db.Hoas.Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList());
              
            }
        }
        [HttpPost]
        public ActionResult Delete_SP(string masp)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
          
            int MaSP = int.Parse(masp);
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();

            // Kiểm tra trước khi xóa
            string kiemTraMessage = KiemTraXoaHoa(MaSP);

            if (!string.IsNullOrEmpty(kiemTraMessage))
            {
                // Nếu có ảnh hưởng từ các bảng khác, bạn có thể xử lý thông báo lỗi ở đây
                TempData["ErrorMessage"] = kiemTraMessage;
            }
            else
            {
                // Nếu không có ảnh hưởng từ các bảng khác, thực hiện xóa
                var delete = db.Hoas.SingleOrDefault(m => m.MaHoa == MaSP);
                if (delete != null)
                {
                    db.Hoas.DeleteOnSubmit(delete);
                    db.SubmitChanges();
                    TempData["SuccessMessage"] = "Xóa sản phẩm thành công.";
                }
            }

            return RedirectToAction("QL_SanPham");
        }
        private string KiemTraXoaHoa(int maHoa)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            string resultMessage = "";
            if (db.ChiTietDonDatHangs.Any(c => c.MaHoa == maHoa))
            {
                resultMessage = "Không thể xóa vì có chi tiết đơn đặt hàng liên quan.";
            }
            else if (db.BinhLuans.Any(b => b.MaHoa == maHoa))
            {
                resultMessage = "Không thể xóa vì có bình luận liên quan.";
            }       
            else if (db.GioHangs.Any(g => g.MaHoa == maHoa))
            {
                resultMessage = "Không thể xóa vì có sản phẩm trong giỏ hàng liên quan.";
            }

       
            return resultMessage;
        }

        [HttpPost]
        public ActionResult Search_QLSP(string tenhoa, int page = 1)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            int NoOfRecordPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(db.Hoas.Count()) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfReCordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            List<Hoa> listSearch = new List<Hoa>();
            if (tenhoa != null)
            {
                listSearch = db.Hoas.Where(m => m.TenHoa.Contains(tenhoa)).Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList();
                
                return View(listSearch);
            }
            else
                return View(db.Hoas.Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList());
        }

        [HttpPost]
        public ActionResult DeleteSelected_SP(string[] selectedAccounts)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            if (selectedAccounts != null && selectedAccounts.Any())
            {
                using (DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext())
                {
                    List<int> selectedIds = selectedAccounts.Select(int.Parse).ToList();

                    // Kiểm tra từng sản phẩm trước khi xóa
                    foreach (int maSP in selectedIds)
                    {
                        string kiemTraMessage = KiemTraXoaHoa(maSP);

                        if (!string.IsNullOrEmpty(kiemTraMessage))
                        {
                            // Nếu có ảnh hưởng từ các bảng khác, bạn có thể xử lý thông báo lỗi ở đây
                            TempData["ErrorMessage"] = kiemTraMessage;
                            return Json(new { success = false, message = kiemTraMessage });
                        }
                    }

                    // Nếu không có ảnh hưởng từ các bảng khác, thực hiện xóa
                    var accountsToDelete = db.Hoas.Where(d => selectedIds.Contains(d.MaHoa)).ToList();
                    db.Hoas.DeleteAllOnSubmit(accountsToDelete);
                    db.SubmitChanges();
                    TempData["SuccessMessage"] = "Xóa sản phẩm thành công.";
                }
            }

            return Json(new { success = true });
        }


        public ActionResult Xoa(int ma)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            var hoaxoa = db.Hoas.SingleOrDefault(m => m.MaHoa == ma);
            db.Hoas.DeleteOnSubmit(hoaxoa);
            db.SubmitChanges();
            return RedirectToAction("TrangChu");
        }

        public ActionResult ThemSanPham()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            return View();
        }
        [HttpPost]
        public ActionResult ThemSanPham(string tenHoa, string moTa, int giaBan, int slKho, string loaiHoa, string mauHoa, string kichThuoc, int khuyenMai, HttpPostedFileBase fileUpload)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            try
            {
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    string imagePath = SaveFileAndReturnPath(fileUpload);

                    using (DB_ShopBanHoaDataContext dbContext = new DB_ShopBanHoaDataContext())
                    {
                        Hoa newHoa = new Hoa
                        {
                            TenHoa = tenHoa,
                            MoTaHoa = moTa,
                            GiaBan = (int)giaBan,
                            SoLuongTonKho = (int)slKho,
                            LoaiHoa = loaiHoa,
                            MauHoa = mauHoa,
                            KichThuoc = kichThuoc,
                            KhuyenMai = (int)khuyenMai,
                            AnhHoa = imagePath
                        };

                        dbContext.Hoas.InsertOnSubmit(newHoa);
                        dbContext.SubmitChanges();

                        ViewBag.tb = "Thêm Thành Công!";
                        return View();
                    }
                }
                ViewBag.tb = "Thêm Thất Bại!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.tb = "Lỗi: " + ex.Message;
                return View();
            }
        }

        private string SaveFileAndReturnPath(HttpPostedFileBase fileUpload)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
            var path = Path.Combine(Server.MapPath("~/Content/Anh"), fileName);

            fileUpload.SaveAs(path);

            return fileName;
        }
        public ActionResult SuaSanPham(int ma)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            var a = db.Hoas.SingleOrDefault(m => m.MaHoa == ma);
            return View(a);
        }

        [HttpPost]
        public ActionResult SuaSanPham(string ma, string tenHoa, string moTa, int giaBan, int slKho, string loaiHoa, string mauHoa, string kichThuoc, int khuyenMai, HttpPostedFileBase fileUpload)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            try
            {
                int maHoa;
                if (int.TryParse(ma, out maHoa))
                {
                    DB_ShopBanHoaDataContext dbContext = new DB_ShopBanHoaDataContext();
                    Hoa existingHoa = dbContext.Hoas.SingleOrDefault(h => h.MaHoa == maHoa);

                    if (existingHoa != null)
                    {
                        // Không cần cập nhật MaHoa ở đây
                        existingHoa.TenHoa = tenHoa;
                        existingHoa.MoTaHoa = moTa;
                        existingHoa.GiaBan = giaBan;
                        existingHoa.SoLuongTonKho = slKho;
                        existingHoa.LoaiHoa = loaiHoa;
                        existingHoa.MauHoa = mauHoa;
                        existingHoa.KichThuoc = kichThuoc;
                        existingHoa.KhuyenMai = khuyenMai;

                        if (fileUpload != null && fileUpload.ContentLength > 0)
                        {
                            string imagePath = SaveFileAndReturnPath(fileUpload);
                            existingHoa.AnhHoa = imagePath;
                        }

                        dbContext.SubmitChanges();

                        return RedirectToAction("QL_SanPham", "AD_Home");
                    }
                    else
                    {
                        ViewBag.tb = "Sản phẩm không tồn tại!";
                        return View(); // hoặc Redirect nếu muốn chuyển hướng ngay lập tức
                    }
                }
                else
                {
                    ViewBag.tb = "Mã không hợp lệ!";
                    return View(); // hoặc Redirect nếu muốn chuyển hướng ngay lập tức
                }
            }
            catch (Exception ex)
            {
                ViewBag.tb = "Lỗi: " + ex.Message;
                return View();
            }
        }

                        
        


        //
        public ActionResult GiaoDien_AD()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;

            return View();
            

            
        }

        public ActionResult AD_CapNhatDonHang()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            var don = db.DonDatHangs.Where(m => m.TrangThaiDonHang != "Đã Thanh Toán" & m.TrangThaiDonHang != "Đã Hủy").ToList();
            return View(don);
        }

        public ActionResult AD_DonHangHuy(int page=1)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();

            //
            int NoOfRecordPerPage = 10;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(db.DonDatHangs.Where(m => m.TrangThaiDonHang == "Đã Hủy").Count()) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfReCordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            return View(db.DonDatHangs.Where(m => m.TrangThaiDonHang == "Đã Hủy").Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList());
        }
        public ActionResult AD_XemChiTietDonHang(int ma)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            var a = db.ChiTietDonDatHangs.Where(m => m.MaDonHang == ma).ToList();
            List<Hoa> hoa = db.Hoas.ToList();
            ViewBag.hoa = hoa;
            return View(a);
        }

        public ActionResult AD_XoaChiTietDonHang(int ma)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
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

                        if (donDatHang != null)
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
        public void UpDateTrangThai(int ma)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db=new DB_ShopBanHoaDataContext();
            DonDatHang a=db.DonDatHangs.SingleOrDefault(m=>m.MaDonHang==ma);
            string d1="Đã Đi Đơn";
            string d2="Đang Giao Hàng";
            string d3="Sắp Đến Bạn";
            string d4="Đã Thanh Toán";
            if(a.TrangThaiDonHang=="Chưa Xử Lý")
            {
                a.TrangThaiDonHang=d1;
            }else if(a.TrangThaiDonHang==d1)
            {
                a.TrangThaiDonHang=d2;
            }else if(a.TrangThaiDonHang==d2)
            {
                a.TrangThaiDonHang=d3;
            }else if(a.TrangThaiDonHang==d3){
                a.TrangThaiDonHang=d4;
            }
            db.SubmitChanges();
        }

        [HttpPost]
        public ActionResult UpdateTrangThaiDonHang(int maDonHang)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            UpDateTrangThai(maDonHang);

          
            return Json(new { success = true });
        }

        public ActionResult AD_XoaDonHang(int ma)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DonDatHang dh = db.DonDatHangs.SingleOrDefault(m => m.MaDonHang == ma);

            ChiTietDonDatHang ct = db.ChiTietDonDatHangs.FirstOrDefault(m => m.MaDonHang == ma);
            if(ct!=null)
            {
                db.ChiTietDonDatHangs.DeleteOnSubmit(ct);
            }
            db.DonDatHangs.DeleteOnSubmit(dh);
            db.SubmitChanges();
            return RedirectToAction("AD_CapNhatDonHang", "AD_Home");
        }

        public ActionResult AD_DonHangDaThanhToan(int page=1)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
           
            //
            int NoOfRecordPerPage = 10;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(db.DonDatHangs.Where(m=>m.TrangThaiDonHang=="Đã Thanh Toán").Count()) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfReCordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            return View(db.DonDatHangs.Where(m=>m.TrangThaiDonHang=="Đã Thanh Toán").Skip(NoOfReCordToSkip).Take(NoOfRecordPerPage).ToList());
        }
        public ActionResult AD_DoiThongTin()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            string tdn = tenDangNhap;
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy a = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tdn);
            ViewBag.tt = a;
            return View();
        }

        [HttpPost]
        public ActionResult AD_DoiThongTin(string ten, string dc, string sdt, string em, HttpPostedFileBase fileUpload)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                Session["TenDangNhap"] = "";
            }
            tenDangNhap = Session["TenDangNhap"] as string;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                Session["AnhDaiDien"] = "avt.jpg";
            }
            anhDaiDien = Session["AnhDaiDien"] as string;
           
            ViewBag.anhDaiDien = anhDaiDien;
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
                    string fileName = AD_SaveFileAndReturnPath_pf(fileUpload);
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
                return RedirectToAction("AD_DoiThongTin","AD_Home");
            }

            ViewBag.tb = "Cập Nhật Thất Bại";
            return RedirectToAction("AD_DoiThongTin", "AD_Home");
        }


        private string AD_SaveFileAndReturnPath_pf(HttpPostedFileBase fileUpload)
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
            var path = Path.Combine(Server.MapPath("~/Content/USER"), fileName);

            fileUpload.SaveAs(path);

            return fileName;
        }

        public ActionResult AD_DoiMatKhau()
        {
            ViewBag.anhDaiDien = "avt.jpg";
            HttpCookie cookietk = Request.Cookies["TenDangNhap"];
            tenDangNhap = cookietk != null ? cookietk.Value : null;

            if (tenDangNhap == "" || tenDangNhap == null)
            {
                tenDangNhap = "";
            }
            Session["TenDangNhap"] = tenDangNhap;
            ViewBag.tenDangNhap = tenDangNhap;
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
            Session["AnhDaiDien"] = anhDaiDien;
            ViewBag.anhDaiDien = anhDaiDien;
            //
            string tdn = tenDangNhap;
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy a = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tdn);
            ViewBag.tt = a;
            return View();
        }

        [HttpPost]
        public ActionResult AD_DoiMatKhau(string tk,string mkc,string mkm,string xnmkm)
        {
            Function a=new Function();
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy chk= db.DangKies.SingleOrDefault(m => m.TenDangNhap == tk);
           
            string mk=a.CalculateMD5Hash(mkc);
            if(mk==chk.MatKhau)
            {
                chk.MatKhau = a.CalculateMD5Hash(mkm);
                db.SubmitChanges();
                ViewBag.tt = chk;
                ViewBag.tb = "Đổi Mật Khẩu Thành Công";
                return View();
            }else
            {
                ViewBag.tb = "Sai Mật Khẩu";
                ViewBag.tt = chk;
            }
            ViewBag.tt = chk;
            return View();
        }

        public ActionResult AD_DangKyTaiKhoan()
        {
            return View();
        }
   
        [HttpPost]
        public ActionResult AD_DangKyTaiKhoan(string tk, string mk, string xnmk)
        {
            Function a = new Function();
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            DangKy dk = db.DangKies.SingleOrDefault(m => m.TenDangNhap == tk);

            if (dk != null)
            {
                ViewBag.tb = "Tài Khoản Đã Tồn Tại";
                return View();
            }

            if (tk.Length < 6 || mk.Length < 6)
            {
                ViewBag.tb = "Tài Khoản, Mật Khẩu Ít Nhất Phải Có 6 Ký Tự";
                return View();
            }

            if (mk != xnmk)
            {
                ViewBag.tb = "Xác Nhận Mật Khẩu Không Đúng";
                return View();
            }

            DangKy tkm = new DangKy();
            tkm.TenDangNhap = tk;
            tkm.MatKhau = a.CalculateMD5Hash(mk);
            tkm.VaiTroNguoiDung = "Admin";
            tkm.TrangThai = "Duoc Dung";
            tkm.TenKhachHang = "Chưa Cập Nhật";
            tkm.DiaChi = "Chưa Cập Nhật";
            tkm.SoDienThoai = "Chưa Cập Nhật";
            tkm.Email = "Chưa Cập Nhật";
            tkm.AnhDaiDien = "avt.jpg";

            db.DangKies.InsertOnSubmit(tkm);
            db.SubmitChanges();

            ViewBag.tb = "Đăng Ký Thành Công";

            return View();
        }
        public ActionResult TK_DoanhThu()
        {
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            int t = db.DonDatHangs.ToList().Sum(m => Convert.ToInt32(m.TongGiaTriDonHang));

            return View("TK_DoanhThu", (object)t);
        }

        public ActionResult TK_SoDon()
        {
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            int t = db.DonDatHangs.ToList().Count();

            return View("TK_SoDon", (object)t);
        }

        public ActionResult TK_DonHuy()
        {
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            int t = db.DonDatHangs.Where(m=>m.TrangThaiDonHang=="Đã Hủy").ToList().Count();

            return View("TK_DonHuy", (object)t);
        }

       

        public ActionResult TK_TaiKhoan()
        {
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            int t = db.DangKies.ToList().Count();

            return View("TK_TaiKhoan", (object)t);
        }

        public ActionResult TK_DonCho()
        {
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            int t = db.DonDatHangs.Where(m => m.TrangThaiDonHang != "Đã Thanh Toán" & m.TrangThaiDonHang != "Đã Hủy").ToList().Count();

            return View("TK_DonCho", (object)t);
        }

        public ActionResult AD_TenDangNhap()
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
            return View("AD_TenDangNhap", (object)tdn);

        }

        public ActionResult AD_AnhDaiDien()
        {
           
            HttpCookie cookieanh = Request.Cookies["AnhDaiDien"];
            anhDaiDien = cookieanh != null ? cookieanh.Value : null;
            if (anhDaiDien == "" || anhDaiDien == null)
            {
                anhDaiDien = "avt.jpg";
            }
          
            //
            string tdn = anhDaiDien;
            return View("AD_AnhDaiDien", (object)tdn);
        }

        public ActionResult AD_DangXuat()
        {
            Response.Cookies["TenDangNhap"].Expires = DateTime.Now.AddDays(-1);

            // Xóa cookie AnhDaiDien
            Response.Cookies["AnhDaiDien"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("V_DangNhap", "Form");
        }
	}
}