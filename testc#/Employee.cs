using System;

public class person
{
    public string id { get; init; }
    public string fullName { get; set; }
    public int birthDay { get; set; }

    // methods 
    public person(string id, string fullName, int birthDay)
    {
        this.id = id;
        this.fullName = fullName;
        this.birthDay = birthDay;
    }

    public int getAge()
    {
        return DateTime.Now.Year - birthDay;
    }

}

// class con : employee

public class employee : person
{

    public decimal baseSalary { get; set; }
    // methods 
    public employee(string id, string fullName, int birthDay, decimal baseSalary) : base(id, fullName, birthDay)
    {
        this.baseSalary = baseSalary;
    }

    public virtual decimal CalculateIncome()
    {
        return baseSalary;
    }
}

public sealed class manager : employee
{
    public decimal ResponsibilityAllowance { get; set; }

    // methods

    public manager(string id, string fullName, int birthDay, decimal baseSalary, decimal ResponsibilityAllowance) : base(id, fullName, birthDay, baseSalary)
    {
        this.ResponsibilityAllowance = ResponsibilityAllowance;
    }
    public override decimal CalculateIncome()
    {
        return baseSalary + ResponsibilityAllowance;
    }
}

public class program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int currentYear = DateTime.Now.Year;

        employee emp1 = new employee("NV01", "Nguyen Van A", 1995, 10_000_000m);

        manager mng1 = new manager("QL01", "Tran Van B", 1998, 15_000_000m, 5_000_000m);

        Console.WriteLine(" phieu luong : ");
        Console.WriteLine($"Employee: {emp1.fullName}, Income: {emp1.CalculateIncome()}");
        Console.WriteLine($"Manager: {mng1.fullName}, Income: {mng1.CalculateIncome()}");
    }
}