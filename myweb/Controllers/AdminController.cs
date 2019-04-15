using myweb.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace myweb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLSPORTDataContext db = new dbQLSPORTDataContext();
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
                return RedirectToAction("Login");
            return View();
        }
        public ActionResult Sanpham(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
                return RedirectToAction("Login");
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.PRODUCTs.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Don't allow null";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Input passworrd";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)        

                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    ViewBag.Thongbao = "Great! Login successful";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "User or Password uncorrect";
            }
            return View();
        }
        public PartialViewResult IDadmin()
        {
            if (Session["Taikhoanadmin"] != null)
            {
                ADMIN ad = (ADMIN)Session["Taikhoanadmin"];
                ViewBag.ThongBao = ad.HoTen;
            }
            return PartialView();
        }
        public ActionResult Logout()
        {
            Session.Remove("Taikhoanadmin");
            Session.Remove("HoTen");
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Createnew()
        {

            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaBrands = new SelectList(db.BRANDs.ToList().OrderBy(n => n.TenBrands), "MaBrands", "TenBrands");
            return View();
        }
        [HttpPost]
        public ActionResult Createnew(PRODUCT sp, HttpPostedFileBase fileUpload, FormCollection collection)
        {

            //Luu ten fie, luu y bo sung thu vien using System.IO;
            var filename = Path.GetFileName(fileUpload.FileName);
            //Luu duong dan cua file
            var path = Path.Combine(Server.MapPath("~/Content/images"), filename);
            //Kiem tra hình anh ton tai chua?
            if (System.IO.File.Exists(path))
            {
                ViewBag.Thongbao = "Images has existed";
            }
            else
            {
                fileUpload.SaveAs(path);
            }
            var tensp = collection["TenSP"];
            var giaban = collection["Giaban"];
            var mota = collection["Mota"];
            var ngaycapnhat = collection["Ngaycapnhat"];
            var slton = collection["Soluongton"];
            var maloai = collection["MaLoai"];
            var mabrands = collection["MaBrands"];
            sp.TenSP = tensp;
            sp.Anh = filename;
            sp.Giaban = Decimal.Parse(giaban);
            sp.Mota = Regex.Replace(mota, "<.*?>", String.Empty);
            sp.Ngaycapnhat = Convert.ToDateTime(ngaycapnhat);
            sp.Soluongton = Int32.Parse(slton);
            sp.MaLoai = Int32.Parse(maloai);
            sp.MaBrands = Int32.Parse(mabrands);
            db.PRODUCTs.InsertOnSubmit(sp);
            db.SubmitChanges();
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaBrands = new SelectList(db.BRANDs.ToList().OrderBy(n => n.TenBrands), "MaBrands", "TenBrands");
         return RedirectToAction("Sanpham");
        }
        public ActionResult Chitietsp(int id)
        {
            //Lay ra doi tuong sach theo ma
            PRODUCT sp = db.PRODUCTs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult Xoasp(int id)
        {
            //Lay ra doi tuong  can xoa theo ma
            PRODUCT sp = db.PRODUCTs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("delete")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            PRODUCT sp = db.PRODUCTs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var filePath = Path.Combine(Server.MapPath("~/Content/images/"), sp.Anh);
            
                db.PRODUCTs.DeleteOnSubmit(sp);
                db.SubmitChanges();
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            
            return RedirectToAction("Sanpham");
        }
        [HttpGet]
        public ActionResult Suasp(int id)
        {
            //Lay ra doi tuong theo ma
            PRODUCT sp = db.PRODUCTs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            ViewData["Image"] = sp.Anh;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu, sắp xep tang dan 
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaBrands = new SelectList(db.BRANDs.ToList().OrderBy(n => n.TenBrands), "MaBrands", "TenBrands");
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasp(PRODUCT sp, HttpPostedFileBase fileUpload, FormCollection col)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaBrands = new SelectList(db.BRANDs.ToList().OrderBy(n => n.TenBrands), "MaBrands", "TenBrands");
            //Kiem tra duong dan 
            var image = col["ImageP"];
            PRODUCT p = db.PRODUCTs.First(n => n.MaSP == sp.MaSP);

            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    if (Path.GetExtension(fileUpload.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(fileUpload.FileName).ToLower() == ".png"
                        || Path.GetExtension(fileUpload.FileName).ToLower() == ".gif"
                        || Path.GetExtension(fileUpload.FileName).ToLower() == ".jpeg"
                        )
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Thongbao = "Name image existed";
                        }
                        else
                        {
                            var newImageP = fileUpload.FileName;
                            fileUpload.SaveAs(path);
                            p.Anh = newImageP;
                            p.Ngaycapnhat = DateTime.Now;
                            UpdateModel(p);
                            db.SubmitChanges();
                            return RedirectToAction("Sanpham");
                        }
                    }
                    else
                    {
                        ViewBag.Thongbao = "Choose image";
                    }
                }

                else
                {
                    p.Anh = image;
                    p.Ngaycapnhat = DateTime.Now;
                    UpdateModel(p);
                    db.SubmitChanges();
                    return RedirectToAction("Sanpham");
                }
            }
            return this.Suasp(p.MaSP);
        }
        //--------------------------Kết thúc quản lý sản phẩm-----------------------
        public ActionResult QLUser(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
                return RedirectToAction("Login");
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.KHACHHANGs.ToList().OrderBy(t => t.MaKH).ToPagedList(pageNumber, pageSize));
        }
        //-----------------------------------Kết thúc quản lí khách hàng-------------
        public ActionResult QLCTDHang(int? page)
        {
            if (Session["Taikhoanadmin"] == null || Session["Taikhoanadmin"].ToString() == "")
                return RedirectToAction("Login");
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.CT_DONHANGs.ToList().OrderBy(t => t.MaDH).ToPagedList(pageNumber, pageSize));
        }
    }
}