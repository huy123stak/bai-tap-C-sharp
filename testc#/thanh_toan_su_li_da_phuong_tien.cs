using System;
// thanh toan
public interface IPayable
{
    bool ProcessPayment(decimal amount);
}

// hoan tien
public interface IRefundable
{
    bool ProcessRefund(decimal amount, string reason);
}


public abstract class PaymentGateway
{
    public string TransactionId { get; init; }
    public DateTime CreationDate { get; init; }

    public string Status { get; protected set; }

    protected PaymentGateway(string transactionId)
    {
        TransactionId = transactionId;
        CreationDate = DateTime.Now;
        Status = "Pending"; // mọi giao dịch mới tạo đều bắt đầu ở trạng thái chờ
    }

    public abstract void ValidateConnection();
    public virtual void LogTransaction(string message)
    {
        Console.WriteLine($"[LOG - {TransactionId}] {message}");
    }
}

public class MomoPayment : PaymentGateway, IPayable, IRefundable
{
    public string PhoneNumber { get; set; }

    public MomoPayment(string transactionId, string phoneNumber)
        : base(transactionId)
    {
        PhoneNumber = phoneNumber;
    }

    
    public override void ValidateConnection()
    {
        Console.WriteLine($"Dang kiem tra ket noi API MoMo cho giao dich {TransactionId}...");
        Console.WriteLine("Ket noi API MoMo: OK");
    }

    public bool ProcessPayment(decimal amount)
    {
        if (amount <= 0 || string.IsNullOrWhiteSpace(PhoneNumber))
        {
            LogTransaction("Thanh toan that bai: thong tin khong hop le");
            return false;
        }

        Status = "Success"; 
        LogTransaction($"Thanh toan thanh cong {amount:N0} VND qua so {PhoneNumber}");
        return true;
    }

    public bool ProcessRefund(decimal amount, string reason)
    {
        if (amount <= 0)
        {
            LogTransaction("Hoan tien that bai: so tien khong hop le");
            return false;
        }

        Status = "Refunded";
        LogTransaction($"Hoan tien {amount:N0} VND. Ly do: {reason}");
        return true;
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        MomoPayment momo = new MomoPayment("GD001", "0901234567");

        momo.ValidateConnection();
        momo.LogTransaction("Khoi tao giao dich moi");

        Console.WriteLine($"\nTrang thai ban dau: {momo.Status}");

        IPayable payService = momo;
        bool paymentResult = payService.ProcessPayment(500_000m);
        Console.WriteLine($"Ket qua thanh toan: {paymentResult}");
        Console.WriteLine($"Trang thai sau thanh toan: {momo.Status}");

        IRefundable refundService = momo;
        bool refundResult = refundService.ProcessRefund(500_000m, "Khach huy don hang");
        Console.WriteLine($"\nKet qua hoan tien: {refundResult}");
        Console.WriteLine($"Trang thai sau hoan tien: {momo.Status}");
    }
}