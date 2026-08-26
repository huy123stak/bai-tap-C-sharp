using System;
using System.ComponentModel.Design;
using System.Timers;
class Program
{
    static void Main()
    {
        

        void timx(double dt, double a,double b, double c)
        {
            double x1 = (-b + Math.Sqrt(dt)) / (2 * a);
            double x2 = (-b - Math.Sqrt(dt)) / (2 * a);
            double kep=-b/(2*a);
            if (dt < 0)
            {
                Console.WriteLine("phuong trinh vo nghiem ");
            }
            if (dt == 0)
            {
                Console.WriteLine($"phuong trinh co nghiem kep x1 = x2 = {kep}");
            }
            if(dt > 0&& a == 0 && b != 0)
            {
                Console.WriteLine($"phuong trinh co 2 nghiem ");
                Console.WriteLine($" x1 = {x1}");
                Console.WriteLine($" x2 = {x2}");
            }
        } 

        double a,b,c;
        double kq;
        Console.WriteLine("ax2+bx+c=0");
        Console.Write("nhap so nguyen duong a:");
        a = double.Parse(Console.ReadLine());  
        Console.Write("nhap so nguyen duong b:");
        b = double.Parse(Console.ReadLine());
        Console.Write("nhap so nguyen duong c:");
        c = double.Parse(Console.ReadLine());
        if (a == 0 && b == 0 && c == 0)
        {
            Console.WriteLine("phuong trinh vo so nghiem ");
        }
        if (a == 0 && b != 0)
        {
            kq = -c / b;
            Console.WriteLine($" x = {kq}");
        }
        double dt = (b * b) - (4 * a * c);
        timx(dt,a,b,c);
    }
}
