using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSpeed
{
    public class QuanLyPhuongTien
    {
        private List<PhuongTien> _danhSach = new List<PhuongTien>();

        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt == null)
                throw new ArgumentNullException("pt");
            _danhSach.Add(pt);
        }

        public void DisplayAll()
        {
            if (_danhSach.Count == 0)
            {
                Console.WriteLine("Danh sach trong.");
                return;
            }

            foreach (PhuongTien pt in _danhSach)
            {
                Console.WriteLine(pt.GetInfo());
                Console.WriteLine($"   => gia lan banh: {pt.TinhGiaLanBanh():N0} VNĐ");
            }
        }

        public PhuongTien FindMaxGiaLanBanh()
        {
            if (_danhSach.Count == 0)
                return null;

            PhuongTien max = _danhSach[0];
            foreach (PhuongTien pt in _danhSach)
            {
                if (pt.TinhGiaLanBanh() > max.TinhGiaLanBanh())
                    max = pt;
            }
            return max;
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<PhuongTien>();

            return _danhSach
                .Where(pt => pt.TenHang.ToLower().Contains(keyword.Trim().ToLower()))
                .ToList();
        }
    }
}
