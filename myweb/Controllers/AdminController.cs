using myweb.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return View();
        }
        public ActionResult Sanpham(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
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
                    Session["Taikhoanadmin"] = ad;
                    ViewBag.Thongbao = "Great! Login successful";
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "User or Password uncorrect";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Createnew()
        {

            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaBrands = new SelectList(db.BRANDs.ToList().OrderBy(n => n.TenBrands), "MaBrands", "TenBrands");
            return View();
        }
        [HttpPost]
        public ActionResult Createnew(PRODUCT sp, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaBrands = new SelectList(db.BRANDs.ToList().OrderBy(n => n.TenBrands), "MaBrands", "TenBrands");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Please choose images for this product";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Images has existed";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sp.Anh = fileName;
                    //Luu vao CSDL
                    db.PRODUCTs.InsertOnSubmit(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sanpham");
            }
        }
        public ActionResult Chitietsp(int id)
        {
            //Lay ra doi tuong sach theo ma
            PRODUCT sp = db.PRODUCTs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Masach = sp.MaSP;
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
            //Lay ra doi tuong sach can xoa theo ma
            PRODUCT sp = db.PRODUCTs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("Remove")]
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
            db.PRODUCTs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Sanpham");
        }
        [HttpGet]
        public ActionResult Suasp(int id)
        {
            //Lay ra doi tuong sach theo ma
            PRODUCT sp = db.PRODUCTs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sp.MaLoai);
            ViewBag.MaBrands = new SelectList(db.BRANDs.ToList().OrderBy(n => n.TenBrands), "MaBrands", "TenBrands", sp.MaBrands);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasp(PRODUCT sp, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaBrands = new SelectList(db.BRANDs.ToList().OrderBy(n => n.TenBrands), "MaBrands", "TenBrands");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Please choose images for product";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "This images existed";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sp.Anh = fileName;
                    //Luu vao CSDL   
                    UpdateModel(sp);
                    db.SubmitChanges();

                }
                return RedirectToAction("Sanpham");
            }
        }
    }
}