using NotesApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Android.Provider.ContactsContract.CommonDataKinds;

namespace NotesApp.Data
{
    public class NotesDatabase
    {
        SQLiteAsyncConnection database;

        public NotesDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Notes>().Wait();
            database.CreateTableAsync<Category>().Wait();
        }

        public Task<List<Notes>> GetNotesAsync()
        {
            return database.Table<Notes>().ToListAsync();
        }

        // get note by its name (might delete)
        public Task<Notes> GetNoteByFilenameAsync(string filename)
        {
            return database.Table<Notes>()
                .Where(n => n.Filename == filename)
                .FirstOrDefaultAsync();
        }
        // get single note by Id
        public Task<Notes> GetNoteByIdAsync(int id)
        {
            return database.Table<Notes>()
                           .Where(n => n.Id == id)
                           .FirstOrDefaultAsync();
        }

        public Task<List<Notes>> GetNotesByCategoryAsync(string category)
        {
            return database.Table<Notes>()
                .Where(n => n.Category == category)
                .ToListAsync();
        }

        public Task<int> SaveNoteAsync(Notes note)
        {
            if (note.Id != 0)
                return database.UpdateAsync(note);
            else
                return database.InsertAsync(note);
        }

        public Task<int> DeleteNoteAsync(Notes note)
        {
            return database.DeleteAsync(note);
        }

        public Task<int> DeleteAllNotesAsync()
        {
            return database.DeleteAllAsync<Notes>();
        }
        // -------------------- CATEGORY METHODS --------------------

        public Task<List<Category>> GetCategoriesAsync()
        {
            return database.Table<Category>().ToListAsync();
        }

        public Task<int> AddCategoryAsync(Category category)
        {
            return database.InsertAsync(category);
        }

        public Task<int> UpdateCategoryAsync(Category category)
        {
            return database.UpdateAsync(category); 
            //return _database.Table<Category>().ToListAsync();
        }
        //public async Task<double> GetCategoryProgressAsync(int categoryId)
        //{
        //    var notes = await _database.Table<Note>().Where(n => n.CategoryId == categoryId).ToListAsync();
        //    if (!notes.Any()) return 0;

        //    int completed = notes.Count(n => n.IsCompleted);
        //    return (double)completed / notes.Count * 100;
        //}

  


        public async Task<double> GetCategoryProgressAsync(int categoryId)
        {
            var notes = await database.Table<Notes>()
                                       .Where(n => n.CategoryId == categoryId)
                                       .ToListAsync();

            if (notes.Count == 0)
                return 0;

            int completedCount = notes.Count(n => n.IsCompleted);
            return (double)completedCount / notes.Count * 100;
        }

        public async Task<int> DeleteCategoryAsync(int categoryId)
        {
            // Delete all notes under this category first
            var notesToDelete = await database.Table<Notes>()
                .Where(n => n.CategoryId == categoryId)
                .ToListAsync();

            foreach (var note in notesToDelete)
                await database.DeleteAsync(note);

            // Delete the category itself
            return await database.DeleteAsync<Category>(categoryId);
        }
        // delete all notes by category name (when deleting a category)
        public Task<int> DeleteNotesByCategoryAsync(string categoryName)
        {
            // executes a SQL delete; returns number of rows affected
            return database.ExecuteAsync("DELETE FROM Notes WHERE Category = ?", categoryName);
        }
    }
}