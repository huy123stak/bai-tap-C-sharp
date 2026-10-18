using System.Globalization;
using System.Text;

namespace BookManager;

public static class CsvService
{
    const string Header = "BookId,Title,Author,Price,Quantity,CoverPath";

    public static void Save(string path, IEnumerable<BookModel> books)
    {
        var sb = new StringBuilder();
        sb.AppendLine(Header);
        foreach (var b in books)
        {
            sb.AppendLine(string.Join(",",
                Escape(b.BookId),
                Escape(b.Title),
                Escape(b.Author),
                b.Price.ToString(CultureInfo.InvariantCulture),
                b.Quantity.ToString(CultureInfo.InvariantCulture),
                Escape(b.CoverPath)));
        }
        File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
    }

    public static List<BookModel> Load(string path)
    {
        var result = new List<BookModel>();
        foreach (var line in File.ReadAllLines(path, Encoding.UTF8))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var f = ParseLine(line);
            if (f.Count < 5) continue;
            if (f[0].Equals("BookId", StringComparison.OrdinalIgnoreCase)) continue;
            if (!decimal.TryParse(f[3], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price)) continue;
            if (!int.TryParse(f[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out int qty)) continue;
            result.Add(new BookModel
            {
                BookId = f[0],
                Title = f[1],
                Author = f[2],
                Price = price,
                Quantity = qty,
                CoverPath = f.Count > 5 ? f[5] : ""
            });
        }
        return result;
    }

    static string Escape(string v)
    {
        v ??= "";
        if (v.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0)
            return "\"" + v.Replace("\"", "\"\"") + "\"";
        return v;
    }

    static List<string> ParseLine(string line)
    {
        var fields = new List<string>();
        var sb = new StringBuilder();
        bool quoted = false;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (quoted)
            {
                if (c == '"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '"') { sb.Append('"'); i++; }
                    else quoted = false;
                }
                else sb.Append(c);
            }
            else if (c == '"') quoted = true;
            else if (c == ',') { fields.Add(sb.ToString()); sb.Clear(); }
            else sb.Append(c);
        }
        fields.Add(sb.ToString());
        return fields;
    }
}
