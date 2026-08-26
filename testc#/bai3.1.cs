using System;
using System.ComponentModel.Design;
using System.Timers;
class Program
{
    static void Main()
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