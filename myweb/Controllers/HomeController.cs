using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myweb.Models;
namespace myweb.Controllers
{
    public class HomeController : Controller
    {
        dbQLSPORTDataContext data = new dbQLSPORTDataContext();
        private List<PRODUCT> laysanpham(int count)
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            return data.PRODUCTs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HomePage()
        {
            //Lấy top 5 Album bán chạy nhất
            var sanphammoi = laysanpham(5);
            return View(sanphammoi);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Category()
        {
            return View();
        }
    }
}