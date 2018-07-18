using Camposur.Model.ViewModel;
using System.Collections.Generic;

namespace Camposur.BusinessLogic.Logic.Interfaces
{
    public interface IOrderLogic
    {
        List<OrderViewModel> GetOrders();

        List<OrderViewModel> GetOrdersByCustomer(int customerId);

        OrderViewModel GetOrder(int orderId);

        bool CreateOrder(OrderViewModel orderVM);

        bool UpdateOrder(OrderViewModel orderVM);

        bool DeleteOrder(int orderId);
    }
}
