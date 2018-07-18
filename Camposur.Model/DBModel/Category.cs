using Camposur.Model.ViewModel;

namespace Camposur.Model.DBModel
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }

        public Category(CategoryViewModel categoryVM)
        {
        }
    }
}