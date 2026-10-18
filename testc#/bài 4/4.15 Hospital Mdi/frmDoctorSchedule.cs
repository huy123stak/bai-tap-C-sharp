namespace HospitalMdi;

public class frmDoctorSchedule : ListFormBase
{
    public frmDoctorSchedule() : base("Lịch trực của Bác sĩ")
    {
        AddColumn(nameof(DutyShift.Date), "Ngày trực", 14, "dd/MM/yyyy");
        AddColumn(nameof(DutyShift.Doctor), "Bác sĩ", 28);
        AddColumn(nameof(DutyShift.Department), "Khoa", 22);
        AddColumn(nameof(DutyShift.Shift), "Ca trực", 26);
        dgv.DataSource = DataStore.Shifts;
    }
}
