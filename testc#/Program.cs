using System;
class Program
{
    static void Main()
    {
        void Songuyen(int n) // so nguyen to
        {
            bool snt = true;
            for (int i = 2;i<Math.Sqrt(n);i++)
            {
                if (n % i == 0)
                {
                    snt= false;
                    break;
                }
            }
            if(snt==true)
            {
                Console.WriteLine($"{n} la so nguyen to");
            }
            else
            {
                Console.WriteLine($"{n} ko phai la so nguyen to");
            }
        }

        void Sohoanhao(int n)
        {
            int ktra = 0;
            for (int i = 1; i < n ; i++)
            {
                if (n % i == 0)
                {
                    ktra = ktra + i;
                }
            }
            if (ktra == n)
            {
                Console.WriteLine($"{n} la so hoan hao ");
                Console.Write($" cac uoc cua {n}:");
                for (int i = 1; i < n - 1; i++)
                {
                    if (n % i == 0)
                    {
                        Console.Write($"{i} ");
                    }

                }
            }
            else
            {
                Console.WriteLine($"{n} ko phai la so hoan hao ");
            }
        }

        int n;
        Console.Write("nhap so nguyen duong n(n>0):");
        do
        {
          n =int.Parse(Console.ReadLine()); 

        }
        while (n<=0);
        Songuyen(n);
        Sohoanhao(n);
    }
}
