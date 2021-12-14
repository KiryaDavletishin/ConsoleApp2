using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    public struct Note
    {
        public String Text;
        public String Name;
        public DateTime Date;
    }

    public class NoteStorage
    {
        public List<Note> NoteList = new List<Note>();
        public void ReadFromDisk(String path)
        {
            try
            {
                String[] lines = File.ReadAllLines(path);
                for (int counter = 0; counter < lines.Length; counter += 3)
                {
                    Note not = new();
                    not.Text = new String(lines[counter]);
                    not.Name = new String(lines[counter + 1]);
                    not.Date = Convert.ToDateTime(lines[counter + 2]);
                    NoteList.Add(not);
                }

            }
            catch (FileNotFoundException)
            {
                File.Create(path);
                Console.WriteLine("FileNotFoundException in method ReadFromDisk in class NoteStorage");
            }
            catch (DirectoryNotFoundException)
            {
                File.Create("File/Data.txt");
                Console.WriteLine("DirectoryNotFoundException in method ReadFromDisk in class NoteStorage");
            }
        }
        public void SaveOnDisk(String path)
        {
            using StreamWriter stream = new(path, true);
            stream.WriteLine(NoteList[NoteList.Count - 1].Text);
            stream.WriteLine(NoteList[NoteList.Count - 1].Name);
            stream.WriteLine(Convert.ToString(NoteList[NoteList.Count - 1].Date));
        }


        public void ShowData()
        {
            foreach (var index in NoteList)
            {
                Console.WriteLine(index.Name);
                Console.WriteLine(index.Text);
                Console.WriteLine(Convert.ToString(index.Date));
            }
        }
    }

    public class NoteController
    {
        private NoteStorage Notes = new NoteStorage();
        private String path;

        public NoteController(String path)
        {
            this.path = path;
            Notes.ReadFromDisk(this.path);
        }

        public List<Note> FindNotesByName(String mask)
        {
            List<Note> equals = new List<Note>();
            foreach (var index in Notes.NoteList)
            {
                if (String.Compare(index.Name, mask) == 0)
                    equals.Add(index);
            }
            return equals;
        }


        public List<Note> FindNotesByText(String text)
        {
            List<Note> equals = new List<Note>();
            foreach (var index in Notes.NoteList)
            {
                if (String.Compare(index.Text, text) == 0)
                    equals.Add(index);
            }
            return equals;
        }

        public List<Note> FindNotesByDate(DateTime date)
        {
            List<Note> equals = new List<Note>();
            foreach (var index in Notes.NoteList)
            {
                if (DateTime.Compare(index.Date, date) == 0)
                    equals.Add(index);
            }
            return equals;
        }

        public void AddNote(Note note)
        {
            Notes.NoteList.Add(note);
            Notes.SaveOnDisk(path);
        }
        public void EditNote(Note note, String Text, String Name)
        {
            for (int counter = 0; counter < Notes.NoteList.Count; counter++)
            {
                if (DateTime.Compare(Notes.NoteList[counter].Date, note.Date)
                    + String.Compare(Notes.NoteList[counter].Text, note.Text)
                    + String.Compare(Notes.NoteList[counter].Name, note.Name) == 0)
                {
                    Note not = Notes.NoteList[counter];
                    not.Text = Text;
                    not.Name = Name;
                    not.Date = DateTime.Now;
                    Notes.NoteList[counter] = not;
                }
            }
            FillFile();
        }

        private void FillFile()
        {
            foreach (var index in Notes.NoteList)
            {
                using StreamWriter stream = new(path, false);
                stream.WriteLine(index.Text);
                stream.WriteLine(index.Name);
                stream.WriteLine(Convert.ToString(index.Date));
            }
        }

        public void DeleteNote(Note note)
        {
            Notes.NoteList.Remove(note);
            FillFile();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            Note not = new();
            not.Text = new String("asd");
            not.Name = new String("asdfg");
            not.Date = Convert.ToDateTime("15.12.2021 22:43:47");
            NoteController obj = new NoteController("File/Data.txt");
            obj.EditNote(not, "Last method", "Kirilla");
            //List<Note> massiv = new List<Note>();
            /*foreach (var index in massiv)
            {
                Console.WriteLine(index.Text);
                Console.WriteLine(index.Name);
                Console.WriteLine(Convert.ToString(index.Date));
            }*/
        }
    }
}

