using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;

namespace _23._1News.Services.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _db;


        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Category> GetAllCategories()
        {
            return _db.Categories.ToList();
        }


        public void AddCategory(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
        }

        public bool UpdateCategoty(Category category)
        {
            try
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public Category GetCategotyById(int id)
        {
            return _db.Categories.Find(id);
        }

        public bool DeleteCategoty(int id) 
        {
            try
            {
                var data = this.GetCategotyById(id);
                if (data == null) 
                {
                    return false;
                }
                _db.Categories.Remove(data);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

      




    }
}
