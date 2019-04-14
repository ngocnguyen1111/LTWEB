using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using myweb.Models;
namespace myweb.Models
{
    public class Giohang
    {
        dbQLSPORTDataContext data = new dbQLSPORTDataContext();
        public int iMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sAnh { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get
            {
                return iSoluong * dDongia;

            }
        }
        //Khoi tao gio hàng theo Masach duoc truyen vao voi Soluong mac dinh la 1
        public Giohang(int MaSP)
        {
            iMaSP = MaSP;
            PRODUCT sp = data.PRODUCTs.Single(n => n.MaSP == iMaSP);
            sTenSP = sp.TenSP;
            sAnh = sp.Anh;
            dDongia = double.Parse(sp.Giaban.ToString());
            iSoluong = 1;
        }
    }
}