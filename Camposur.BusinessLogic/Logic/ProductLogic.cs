using Camposur.BusinessLogic.Logic.Interfaces;
using Camposur.DataAccess.Repositories.Interfaces;
using Camposur.Model.DBModel;
using Camposur.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Camposur.BusinessLogic.Logic
{
    public class ProductLogic : IProductLogic
    {
        private readonly ICollectionRepository<Product> _productRepository;

        public ProductLogic(ICollectionRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public bool CreateProduct(ProductViewModel productVM)
        {
            var product = new Product(productVM);
            int productId = _productRepository.Add(product);

            return productId > 0;
        }

        public bool DeleteProduct(int productId)
        {
            try
            {
                _productRepository.Delete(productId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ProductViewModel GetProduct(int productId)
        {
            try
            {
                var product = _productRepository.List(p => p.Id == productId).SingleOrDefault();
                return new ProductViewModel(product);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProductViewModel> GetProducts()
        {
            return _productRepository.List().Select(p => new ProductViewModel(p)).ToList();
        }

        public List<ProductViewModel> GetProductsByCategory(int categoryId)
        {
            return _productRepository.List(p => p.CategoryId == categoryId).Select(p => new ProductViewModel(p)).ToList();
        }

        public bool UpdateProduct(ProductViewModel productVM)
        {
            var product = new Product(productVM);

            try
            {
                _productRepository.Update(product);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
