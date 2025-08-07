using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using NotesApp.Models;

namespace NotesApp.Data
{
    public class NotesDatabase
    {
        readonly SQLiteAsyncConnection database;

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

        public Task<Notes> GetNoteByFilenameAsync(string filename)
        {
            return database.Table<Notes>()
                .Where(n => n.Filename == filename)
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
    }
}