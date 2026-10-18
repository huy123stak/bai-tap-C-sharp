namespace HospitalMdi;

public class frmDoctorCatalog : ListFormBase
{
    public frmDoctorCatalog() : base("Danh mục Bác sĩ")
    {
        AddColumn(nameof(Doctor.DoctorId), "Mã BS", 10);
        AddColumn(nameof(Doctor.FullName), "Họ tên", 28);
        AddColumn(nameof(Doctor.Specialty), "Chuyên môn", 30);
        AddColumn(nameof(Doctor.Department), "Khoa", 22);
        dgv.DataSource = DataStore.Doctors;
    }
}
