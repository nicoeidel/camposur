using Camposur.Model.ViewModel;
using System.Collections.Generic;

namespace Camposur.BusinessLogic.Logic.Interfaces
{
    public interface ICategoryLogic
    {
        List<CategoryViewModel> GetCategories();

        CategoryViewModel GetCategory(int categoryId);

        bool CreateCategory(CategoryViewModel categoryVM);

        bool UpdateCategory(CategoryViewModel categoryVM);

        bool DeleteCategory(int categoryId);
    }
}
