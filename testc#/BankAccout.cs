using System;

public class BankAccount
{
    private static long _nextAccountNumber = 1000000001;

    private const decimal MinimumBalance = 50_000m;

    private decimal _balance;

    public long accountNumber { get; init; }

    private string _accountHolder = string.Empty;
    public string AccountHolder
    {
        get => _accountHolder;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ten chu tai khoan ko the de trong ");
            }
            _accountHolder = value;
        }
    }

    public decimal Balance
    {
        get => _balance;
        private set => _balance = value;
    }

    public BankAccount(string accountHolder, decimal initialBalance)
    {
        if (initialBalance < MinimumBalance)
        {
            throw new ArgumentException($"So du khoi tao phai lon hon hoac bang {MinimumBalance} VND");
        }
        AccountHolder = accountHolder;
        Balance = initialBalance;
        accountNumber = _nextAccountNumber;
        _nextAccountNumber++;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("So tien nap vao phai lon hon 0");
        }
        Balance += amount;

        Console.WriteLine($"Nap {amount} VND vao tai khoan {accountNumber}. So du hien tai: {Balance} VND");
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("So tien rut ra phai lon hon 0");
        }
        if (Balance - amount < MinimumBalance)
        {
            Console.WriteLine($"Khong the rut {amount} VND tu tai khoan {accountNumber}. So du hien tai: {Balance} VND. So du toi thieu la {MinimumBalance} VND");
            return false;
        }
        Balance -= amount;
        Console.WriteLine($"Rut {amount} VND tu tai khoan {accountNumber}. So du hien tai: {Balance} VND");
        return true;
    }

    public void DisplayAccountInfo()
    {
        Console.WriteLine($"Thong tin tai khoan: ");
        Console.WriteLine($"So tai khoan: {accountNumber}");
        Console.WriteLine($"Ten chu tai khoan: {AccountHolder}");
        Console.WriteLine($"So du hien tai: {Balance} VND");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("tao tai khoan ngan hang");
        var account1 = new BankAccount("Nguyen Van A", 100_000m);
        var account2 = new BankAccount("Nguyen Van B", 200_000m);
        account1.DisplayAccountInfo();
        account2.DisplayAccountInfo();

        try
        {
            var invalidAccount = new BankAccount("Nguyen Van C", 10_000m);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"loi: {ex.Message}");
        }

        Console.WriteLine("giao dich rut nap");
        account1.Deposit(50_000m);
        account2.Withdraw(100_000m);

        account1.Withdraw(500_000m);

        try
        {
            account2.Deposit(-5000m);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"loi giao dich khong hop le: {ex.Message}");
        }

        Console.WriteLine("thong tin tai khoan sau giao dich");
        account1.DisplayAccountInfo();
        account2.DisplayAccountInfo();
    }
}
