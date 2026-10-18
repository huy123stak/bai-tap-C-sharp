using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GymMemberManager;

public class MemberModel : INotifyPropertyChanged
{
    string memberId, fullName, phone, package;
    DateTime startDate;
    int fee;

    public string MemberId { get => memberId; set => Set(ref memberId, value); }
    public string FullName { get => fullName; set => Set(ref fullName, value); }
    public string Phone { get => phone; set => Set(ref phone, value); }
    public string Package { get => package; set => Set(ref package, value); }
    public DateTime StartDate { get => startDate; set => Set(ref startDate, value); }
    public int Fee { get => fee; set => Set(ref fee, value); }

    public event PropertyChangedEventHandler PropertyChanged;

    void Set<T>(ref T field, T value, [CallerMemberName] string name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
