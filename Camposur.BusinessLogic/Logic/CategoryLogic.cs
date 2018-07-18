using Camposur.BusinessLogic.Logic.Interfaces;
using Camposur.DataAccess.Repositories.Interfaces;
using Camposur.Model.DBModel;
using Camposur.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Camposur.BusinessLogic.Logic
{
    public class CategoryLogic : ICategoryLogic
    {
        private readonly ICollectionRepository<Category> _categoryRepository;

        public CategoryLogic(ICollectionRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public bool CreateCategory(CategoryViewModel categoryVM)
        {
            var category = new Category(categoryVM);
            int categoryId = _categoryRepository.Add(category);

            return categoryId > 0;
        }

        public bool DeleteCategory(int categoryId)
        {
            try
            {
                _categoryRepository.Delete(categoryId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public CategoryViewModel GetCategory(int categoryId)
        {
            try
            {
                var category = _categoryRepository.List(t => t.Id == categoryId).SingleOrDefault();
                return new CategoryViewModel(category);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CategoryViewModel> GetCategories()
        {
            return _categoryRepository.List().Select(t => new CategoryViewModel(t)).ToList();
        }

        public bool UpdateCategory(CategoryViewModel categoryVM)
        {
            var category = new Category(categoryVM);

            try
            {
                _categoryRepository.Update(category);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}