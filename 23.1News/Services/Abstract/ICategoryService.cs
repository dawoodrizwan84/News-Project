using _23._1News.Models.Db;

namespace _23._1News.Services.Abstract
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();

        void AddCategory(Category category);

        bool UpdateCategoty(Category category);
        bool DeleteCategoty(int id);

        Category GetCategotyById(int id);

       

    }
}
