using System;
using System.Collections.Generic;

public static class discountCalculator
{   
    // discount 5%
    public static decimal ApplyDiscount(decimal totalAmount )
    {
        return totalAmount - (totalAmount * 0.05m);
    }

    // discount chosen by user

    public static decimal ApplyDiscount(decimal totalAmount,  double discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(discountPercentage), "Phan tram giam gia phai tu 0 den 100%");
        }

        decimal totalDiscount = totalAmount * (decimal)(discountPercentage / 100);
        return totalAmount - totalDiscount;
    }

    // fixedvoucher + minimum order 
    public static decimal ApplyVoucherDiscount(decimal totalAmount, decimal fixedVoucherAmount, decimal minimumOrderAmount)
    {
        if (totalAmount >= minimumOrderAmount)
        {
            decimal result = totalAmount - fixedVoucherAmount;
            return result >= 0 ? result : 0; // ko de am tien
        }
        return totalAmount; // ko du dieu kien de ap dung voucher
    }
}

public class deliveryService
{
    public string oderId { get; set; }
    public double distance { get; set; } // km

    public deliveryService(string oderId, double distance)
    {
        this.oderId = oderId;
        this.distance = distance;
    }

    public virtual decimal CalculateDeliveryFee()
    {
        return (decimal)distance * 5000m; // phi theo khoang cach
    }
}

public class expressDeliveryService : deliveryService
{
    public expressDeliveryService(string oderId, double distance) : base(oderId, distance)
    {
    }
    public override decimal CalculateDeliveryFee()
    {
        decimal baseFee = base.CalculateDeliveryFee();
        return baseFee * 1.5m +20_000m; // phi tang 50% cho dich vu express
    }
}

public class ecoDeliveryService : deliveryService
{
    public ecoDeliveryService(string oderId, double distance) : base(oderId, distance)
    {
    }
    public override decimal CalculateDeliveryFee()
    {
        decimal baseFee = base.CalculateDeliveryFee();

        if (distance > 10)
        {
            return baseFee * 0.9m; // giam 20% cho don hang tren 10km
        }
        return baseFee;
    }

}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // test overloading method
        Console.WriteLine(" test overloading method");

        decimal order1 = 100000m;

        Console.WriteLine($"giam mac dinh 5% :{order1}->{discountCalculator.ApplyDiscount(order1):N0} VND");

        decimal order2 = 100000m;
        Console.WriteLine($"giam 20% tuy chinh :{order2}->{discountCalculator.ApplyDiscount(order2, 20):N0} VND");

        decimal order3 = 50000m;
        Console.WriteLine($"voucher 50.000 (don toi thieu 30.000):{order3}->{discountCalculator.ApplyVoucherDiscount(order3, 50000m, 30000m):N0} VND");
        decimal order4 = 20000m;
        Console.WriteLine($"voucher 50.000 (don toi thieu 30.000) nhung ko du dieu kien :{order4}->{discountCalculator.ApplyVoucherDiscount(order4, 50000m, 30000m):N0} VND");

        // test overriding method

        Console.WriteLine("test overriding method");

        List<deliveryService> deliveries = new List<deliveryService>
        {
            new expressDeliveryService("DH01", 8),
            new ecoDeliveryService("DH02", 15),
            new deliveryService("DH03", 12)  // giao thuong    
        };

        foreach (var delivery in deliveries)
        {
            decimal fee = delivery.CalculateDeliveryFee();
            Console.WriteLine($"don ID: {delivery.oderId}, khoang cach : {delivery.distance} km, phi ship: {fee:N0} VND");
        }

    }
}