using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanHoa.Models;

namespace ShopBanHoa.Controllers
{
    public class FormController : Controller
    {
        //
        // GET: /Form/
        Function h = new Function();
        public ActionResult V_DangNhap()
        {
            
            if (Session["tbdangnhap"] != null)
            {
                ViewBag.tbdangnhap = Session["tbdangnhap"];
            }

            // Lấy thông tin từ TempData nếu có
            DangKyC dk = TempData["DangKyObject"] as DangKyC;

            // Gửi đối tượng dk đến view
            return View(dk);
        }

        [HttpPost]
        public ActionResult DangNhap(string taikhoan, string matkhau)
        {
          
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();
            var user = db.DangKies.FirstOrDefault(m => m.TenDangNhap == taikhoan && m.MatKhau == h.CalculateMD5Hash(matkhau));
            if (user != null)
            {
                if (user.VaiTroNguoiDung == "Admin")
                {
                    string tendangnhap = user.TenDangNhap;
                    string anhdaidien = user.AnhDaiDien;
                    HttpCookie cookietk = new HttpCookie("TenDangNhap", tendangnhap);
                    Response.Cookies.Add(cookietk);
                    HttpCookie cookieanh = new HttpCookie("AnhDaiDien", anhdaidien);
                    Response.Cookies.Add(cookieanh);
                    return RedirectToAction("GiaoDien_AD", "AD_Home");
                }
                else if (user.VaiTroNguoiDung == "User")
                {
                    if (user.TrangThai != "Chan")
                    {
                        string tendangnhap = user.TenDangNhap;
                        string anhdaidien = user.AnhDaiDien;
                        HttpCookie cookietk = new HttpCookie("TenDangNhap", tendangnhap);
                        Response.Cookies.Add(cookietk);
                        HttpCookie cookieanh = new HttpCookie("AnhDaiDien", anhdaidien);
                        Response.Cookies.Add(cookieanh);
                        return RedirectToAction("TrangChu", "Home");
                    }
                    else
                    {
                        Session["tbdangnhap"] = "Tài Khoản Này Đã Bị Chặn!";
                        return RedirectToAction("V_DangNhap");
                    }
                }
            }

            Session["tbdangnhap"] = "Tài Khoản Hoặc Mật Khẩu Sai!";
            return RedirectToAction("V_DangNhap");
        }

        [HttpPost]
        public ActionResult DangKyF(DangKyC dk)
        {
            Session["tbdangnhap"] = "";
            DB_ShopBanHoaDataContext db = new DB_ShopBanHoaDataContext();

            if (ModelState.IsValid)
            {
                var kt = db.DangKies.FirstOrDefault(m => m.TenDangNhap == dk.TaiKhoanC);
                if (kt == null)
                {
                    DangKy dky = new DangKy();
                    dky.TenDangNhap = dk.TaiKhoanC;
                    dky.MatKhau = h.CalculateMD5Hash(dk.MatKhauC);
                    dky.VaiTroNguoiDung = "User";
                    dky.TrangThai = "Duoc Dung";
                    dky.AnhDaiDien = "logoForD.png";
                    dky.TenKhachHang = "Chưa Cập Nhật";
                    dky.DiaChi = "Chưa Cập Nhật";
                    dky.SoDienThoai = "Chưa Cập Nhật";
                    dky.Email = "Chưa Cập Nhật";
                    db.DangKies.InsertOnSubmit(dky);
                    db.SubmitChanges();
                    Session["tbdangnhap"] = "Đăng Ký Thành Công!";
                }
                else
                {
                    Session["tbdangnhap"] = "Đăng Ký Thất Bại! Tài Khoản Đã Tồn Tại";
                    TempData["DangKyObject"] = dk; // Giữ lại đối tượng để hiển thị lại trên form
                }
            }
            else
            {
                Session["tbdangnhap"] = "Đăng Ký Không Thành Công!";
                TempData["DangKyObject"] = dk; // Giữ lại đối tượng để hiển thị lại trên form
            }

            return RedirectToAction("V_DangNhap", "Form");
        }
	}
}