using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myweb.Models;
using PagedList;
using PagedList.Mvc;
namespace myweb.Controllers
{
    [System.Runtime.InteropServices.ComVisible(true)]

    public class HomeController : Controller
    {
        dbQLSPORTDataContext data = new dbQLSPORTDataContext();
        private List<PRODUCT>laysanpham(int count)
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            return data.PRODUCTs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        private List<PRODUCT> laytatca()
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            return data.PRODUCTs.OrderBy(a => a.TenSP).ToList();
        }

        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sp = laysanpham(7);
            return View(sp.ToPagedList(pageNum,pageSize));
           
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Category(int ? page)
        {

            int pageSize = 12;
            int pageNum = (page ?? 1);
            var sp = laytatca();
            return View(sp.ToPagedList(pageNum, pageSize));

        }

        public ActionResult Loai()
        {
            var  loai = from l in data.LOAIs select l;
            return PartialView(loai);
        }
        public ActionResult SPLoai(int id, int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
        var sp = from s in data.PRODUCTs where s.MaLoai == id select s;
            return View(sp.ToPagedList(pageNum,pageSize));
        }
        public ActionResult Brands()
        {
            var brand = from b in data.BRANDs select b;
            return PartialView(brand);
        }
        public ActionResult SPBrands(int id,int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sp = from s in data.PRODUCTs where s.MaBrands == id select s;
            return View(sp.ToPagedList(pageNum, pageSize));
        }
        public ViewResult SingleProduct(int id)
        {
            PRODUCT sp = data.PRODUCTs.SingleOrDefault(n => n.MaSP == id);
            if(sp==null)
            {
                Response.StatusCode= 404;
                return null;
            }
            return View(sp);
        }
        
    }
}