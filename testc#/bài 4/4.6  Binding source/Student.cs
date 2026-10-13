using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bai4_6
{
    public class Student : INotifyPropertyChanged
    {
        private string _id, _name, _gender, _avatar;
        private int _age;

        public string StudentId { get => _id; set { if (_id != value) { _id = value; Notify(); } } }
        public string FullName { get => _name; set { if (_name != value) { _name = value; Notify(); } } }
        public int Age { get => _age; set { if (_age != value) { _age = value; Notify(); } } }
        public string Gender { get => _gender; set { if (_gender != value) { _gender = value; Notify(); } } }
        public string AvatarPath { get => _avatar; set { if (_avatar != value) { _avatar = value; Notify(); } } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
