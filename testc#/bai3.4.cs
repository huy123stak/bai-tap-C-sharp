using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace testc_
{
    internal class bai34
    {
        static void chedo1()
        {
            {
                char op;
                double a, b, kq;
                Console.Write("nhap so thu nhat a: ");
                a = double.Parse(Console.ReadLine());
                Console.Write("nhap so thu hai b: ");
                b = double.Parse(Console.ReadLine());
                Console.WriteLine("chon phep tinh: + | - | * | / ");
                op = char.Parse(Console.ReadLine());
                switch (op)
                {
                    case '+': kq = a + b; break;
                    case '-': kq = a - b; break;
                    case '*': kq = a * b; break;
                    case '/': kq = b != 0 ? a / b : 0; break;
                    default:
                        {
                            Console.WriteLine("phep tinh ko hop le"); return;
                        }
                }
                Console.WriteLine($"ket qua {a} {op} {b} = {kq}");
            }
        }

        static void chedo2()
        {
            void timx(double dt, double a, double b, double c)
            {
                double x1 = (-b + Math.Sqrt(dt)) / (2 * a);
                double x2 = (-b - Math.Sqrt(dt)) / (2 * a);
                double kep = -b / (2 * a);
                if (dt < 0)
                {
                    Console.WriteLine("phuong trinh vo nghiem ");
                    Console.WriteLine("bam phim bat ky de tiep tuc");
                    Console.ReadKey();
                }
                if (dt == 0)
                {
                    Console.WriteLine($"phuong trinh co nghiem kep x1 = x2 = {kep}");
                    Console.WriteLine("bam phim bat ky de tiep tuc");
                    Console.ReadKey();
                }
                if (dt > 0 && a == 0 && b != 0)
                {
                    Console.WriteLine($"phuong trinh co 2 nghiem ");
                    Console.WriteLine($" x1 = {x1}");
                    Console.WriteLine($" x2 = {x2}");
                    Console.WriteLine("bam phim bat ky de tiep tuc");
                    Console.ReadKey();
                }
            }

            double a, b, c;
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
             timx(dt, a, b, c);
            
        }

        static void chedo3()
        {
            int n;
            do
            {
                Console.WriteLine("nhap so nguyen duong n(n>0):");
                n = int.Parse(Console.ReadLine());
            } while (n <= 0);
            void Songuyen(int n) // so nguyen to
            {
                bool snt = true;
                for (int i = 2; i < Math.Sqrt(n); i++)
                {
                    if (n % i == 0)
                    {
                        snt = false;
                        break;
                    }
                }
                if (snt == true)
                {
                    Console.WriteLine($"{n} la so nguyen to");
                }
                else
                {
                    Console.WriteLine($"{n} ko phai la so nguyen to");
                }
            }

            void fibonacci(int n)
            {
                long a = 0, b = 1;
                Console.WriteLine("Day fibonacci:");
                Console.Write($"{a} {b} ");
                for (int i = 2; i < n; i++)
                {
                    long c = a + b;
                    if (c > n)
                        break;
                    Console.Write($"{c} ");
                    a = b;
                    b = c;
                }
                Console.WriteLine();
            }
            Songuyen(n);
            fibonacci(n);
        }

        static void Main()
        {
            int chedo;
            do
            {
                Console.WriteLine("--- MENU ---");
                Console.WriteLine("1.caculator");
                Console.WriteLine("2.phuong trinh bac 2");
                Console.WriteLine("3.so nguyen to & fibonacci ");
                Console.WriteLine("0.thoat chuong trinh");
                chedo = int.Parse(Console.ReadLine());
            } while (chedo < 0 || chedo > 3);

            switch (chedo)
            {
                case 0: Console.WriteLine("thoat chuong trinh"); break;
                case 1: chedo1(); break;
                case 2: chedo2(); break;
                case 3: chedo3(); break;
                default: Console.WriteLine("lua chon khong hop le "); break;
            }
            if (chedo != 0)
            {
                Console.WriteLine("nhan phim bat ky de tiep tuc");
                Console.ReadKey();
                Console.Clear();
                Main();
            }
        }   
    }
}