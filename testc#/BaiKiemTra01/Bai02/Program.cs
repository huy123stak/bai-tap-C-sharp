using System;
using System.Text;

namespace AutoSpeed
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // TC01: nam san xuat sai
            Console.WriteLine("=== TC01: kiem tra validation nam san xuat ===");
            try
            {
                OTo loi = new OTo("OT00", "Toyota", 1850, 800000000m, 5, 2.0);
                Console.WriteLine("tao duoc doi tuong (SAI)");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("loi: " + ex.Message);
            }

            // TC02:  o to 5 cho
            Console.WriteLine("\n=== TC02: gia lan banh o to ===");
            OTo oTo = new OTo("OT01", "Mercedes", 2023, 1000000000m, 5, 2.0);
            Console.WriteLine($"gia lan banh: {oTo.TinhGiaLanBanh():N0} VNĐ (mong đợi 1,420,000,000)");

            // TC03: xe may 150cc
            Console.WriteLine("\n=== TC03: gia lan banh xe may ===");
            XeMay xeMay = new XeMay("XM01", "Honda", 2024, 50000000m, 150);
            Console.WriteLine($"gia lan banh: {xeMay.TinhGiaLanBanh():N0} VNĐ (mong đợi 51,000,000)");

            // TC04: da hinh
            Console.WriteLine("\n=== TC04: da hinh List<PhuongTien> ===");
            QuanLyPhuongTien ql = new QuanLyPhuongTien();
            ql.AddPhuongTien(oTo);
            ql.AddPhuongTien(xeMay);
            ql.DisplayAll();

            // TC05: tim max
            Console.WriteLine("\n=== TC05: tim gia lan banh cao nhat ===");
            PhuongTien max = ql.FindMaxGiaLanBanh();
            Console.WriteLine(max.GetInfo());
            Console.WriteLine($"Gia lan banh: {max.TinhGiaLanBanh():N0} VNĐ");

            // test tim kiem
            Console.WriteLine("\n=== tim theo ten hang: 'hon' ===");
            foreach (PhuongTien pt in ql.SearchByName("hon"))
                Console.WriteLine(pt.GetInfo());

            
        }
    }
}
