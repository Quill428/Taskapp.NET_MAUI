using SQLite;

namespace NotesApp.Models
{
    public class Notes
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CategoryId { get; set; } // Foreign key to Category

        public string Filename { get; set; }
        public string Category { get; set; }

        public string Text { get; set; }
        public DateTime Date { get; set; }

        public bool IsCompleted { get; set; }

        //public void Save()
        //{
        //    var content = $"{Category ?? "Uncategorized"}\n{Text}";
        //    File.WriteAllText(Filename, content);
        //} //might need to change this

        //public void Load()
        //{
        //    if (File.Exists(Filename))
        //    {
        //        var lines = File.ReadAllLines(Filename);
        //        if (lines.Length >= 2)
        //        {
        //            Category = lines[0];
        //            IsCompleted = bool.TryParse(lines[1], out var parsed) && parsed;
        //            Text = string.Join('\n', lines.Skip(1));
        //        }
        //        else
        //        {
        //            Text = File.ReadAllText(Filename);
        //            Category = "Uncategorized";
        //           IsCompleted = false;
        //           }
        //        Date = File.GetLastWriteTime(Filename);
        //    }
        //}

    }
}
