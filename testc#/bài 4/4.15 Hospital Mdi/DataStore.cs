using System.ComponentModel;

namespace HospitalMdi;

public class Patient
{
    public string PatientId { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public string Diagnosis { get; set; }
    public string Department { get; set; }
}

public class Doctor
{
    public string DoctorId { get; set; }
    public string FullName { get; set; }
    public string Specialty { get; set; }
    public string Department { get; set; }
}

public class Department
{
    public string DepartmentId { get; set; }
    public string Name { get; set; }
    public string Room { get; set; }
}

public class DutyShift
{
    public DateTime Date { get; set; }
    public string Doctor { get; set; }
    public string Department { get; set; }
    public string Shift { get; set; }
}

public static class DataStore
{
    public static BindingList<Patient> Patients { get; } = new();
    public static BindingList<Doctor> Doctors { get; } = new();
    public static BindingList<Department> Departments { get; } = new();
    public static BindingList<DutyShift> Shifts { get; } = new();

    static DataStore()
    {
        Load();
    }

    public static void Load()
    {
        Departments.Clear();
        Departments.Add(new Department { DepartmentId = "K01", Name = "Nội tổng quát", Room = "P.101" });
        Departments.Add(new Department { DepartmentId = "K02", Name = "Ngoại khoa", Room = "P.201" });
        Departments.Add(new Department { DepartmentId = "K03", Name = "Nhi khoa", Room = "P.301" });
        Departments.Add(new Department { DepartmentId = "K04", Name = "Cấp cứu", Room = "P.001" });

        Doctors.Clear();
        Doctors.Add(new Doctor { DoctorId = "BS01", FullName = "BS. Nguyễn Văn Hùng", Specialty = "Nội tim mạch", Department = "Nội tổng quát" });
        Doctors.Add(new Doctor { DoctorId = "BS02", FullName = "BS. Trần Thị Lan", Specialty = "Phẫu thuật tổng quát", Department = "Ngoại khoa" });
        Doctors.Add(new Doctor { DoctorId = "BS03", FullName = "BS. Lê Minh Quân", Specialty = "Nhi sơ sinh", Department = "Nhi khoa" });
        Doctors.Add(new Doctor { DoctorId = "BS04", FullName = "BS. Phạm Thu Hà", Specialty = "Hồi sức cấp cứu", Department = "Cấp cứu" });

        Patients.Clear();
        Patients.Add(new Patient { PatientId = "BN001", FullName = "Nguyễn Văn An", Age = 45, Diagnosis = "Tăng huyết áp", Department = "Nội tổng quát" });
        Patients.Add(new Patient { PatientId = "BN002", FullName = "Trần Thị Bình", Age = 32, Diagnosis = "Viêm ruột thừa", Department = "Ngoại khoa" });
        Patients.Add(new Patient { PatientId = "BN003", FullName = "Lê Hoàng Cường", Age = 6, Diagnosis = "Viêm phế quản", Department = "Nhi khoa" });
        Patients.Add(new Patient { PatientId = "BN004", FullName = "Phạm Thu Dung", Age = 58, Diagnosis = "Chấn thương sọ não", Department = "Cấp cứu" });

        Shifts.Clear();
        string[] shiftNames = { "Sáng (07:00 - 13:00)", "Chiều (13:00 - 19:00)", "Đêm (19:00 - 07:00)" };
        for (int d = 0; d < 4; d++)
        {
            for (int i = 0; i < Doctors.Count; i++)
            {
                Shifts.Add(new DutyShift
                {
                    Date = DateTime.Today.AddDays(d),
                    Doctor = Doctors[i].FullName,
                    Department = Doctors[i].Department,
                    Shift = shiftNames[(i + d) % shiftNames.Length]
                });
            }
        }
    }
}
