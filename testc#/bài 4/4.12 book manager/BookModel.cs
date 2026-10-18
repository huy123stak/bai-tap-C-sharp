using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookManager;

public class BookModel : INotifyPropertyChanged
{
    string bookId, title, author, coverPath;
    decimal price;
    int quantity;

    public string BookId { get => bookId; set => Set(ref bookId, value); }
    public string Title { get => title; set => Set(ref title, value); }
    public string Author { get => author; set => Set(ref author, value); }
    public decimal Price { get => price; set => Set(ref price, value); }
    public int Quantity { get => quantity; set => Set(ref quantity, value); }

    public string CoverPath
    {
        get => coverPath;
        set
        {
            if (coverPath == value) return;
            coverPath = value;
            Raise(nameof(CoverPath));
            Raise(nameof(CoverFile));
        }
    }

    public string CoverFile => string.IsNullOrEmpty(coverPath) ? "" : Path.GetFileName(coverPath);
    public decimal TotalValue => Price * Quantity;

    public event PropertyChangedEventHandler PropertyChanged;

    void Raise(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    void Set<T>(ref T field, T value, [CallerMemberName] string name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        Raise(name);
        if (name == nameof(Price) || name == nameof(Quantity)) Raise(nameof(TotalValue));
    }
}
