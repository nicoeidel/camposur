using Camposur.Model.ViewModel;
using System.Collections.Generic;

namespace Camposur.BusinessLogic.Logic.Interfaces
{
    public interface IProductLogic
    {
        List<ProductViewModel> GetProducts();

        List<ProductViewModel> GetProductsByCategory(int categoryId);

        ProductViewModel GetProduct(int productId);

        bool CreateProduct(ProductViewModel productVM);

        bool UpdateProduct(ProductViewModel productVM);

        bool DeleteProduct(int productId);
    }
}