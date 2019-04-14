using Microsoft.AspNet.Identity;
using myweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;


namespace myweb.Controllers
{

    public class UserController : Controller
    {
        dbQLSPORTDataContext data = new dbQLSPORTDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var dienthoai = collection["Dienthoai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var ngaysinh = String.Format("{0:dd/MM/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Full name don't allow null";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "User name don't allow null";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Password don't allow null";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Confirm password don't allow null";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi5"] = "Input your phone number";

            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Don't allow null";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi7"] = "Email don't allow null";
            }
            
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();

                if (kh != null)
                {
                    ViewBag.Thongbao = "Great! Register successful";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Dangnhap", "User");
                }
                else
                {
                    return View();

                }
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "User name don't allow null";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Password don't allow null";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);

                if (kh != null)
                {
                    ViewBag.Thongbao = "Great! Login successful";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "Home");
                }
                else { 
                    ViewBag.Thongbao = "Username or Password uncorrect. You don't have account? Please click Register Now";
                    
                }
            }
            return View();

        }
        public PartialViewResult ID()
        {
            if (Session["Taikhoan"] != null)
            {
                KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
                ViewBag.ThongBao = kh.HoTen;
            }
            return PartialView();
        }
        public ActionResult Dangxuat()
        {
            Session.Remove("Taikhoan");
            Session.Remove("ID");
            return RedirectToAction("Index", "Home");
        }
    }
}