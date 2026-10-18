namespace HospitalMdi;

public class frmDepartmentCatalog : ListFormBase
{
    public frmDepartmentCatalog() : base("Danh mục Khoa/Phòng")
    {
        AddColumn(nameof(Department.DepartmentId), "Mã khoa", 15);
        AddColumn(nameof(Department.Name), "Tên khoa", 50);
        AddColumn(nameof(Department.Room), "Phòng", 20);
        dgv.DataSource = DataStore.Departments;
    }
}
