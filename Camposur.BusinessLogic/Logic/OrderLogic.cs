using Camposur.BusinessLogic.Logic.Interfaces;
using Camposur.Model.ViewModel;
using System;
using System.Collections.Generic;

namespace Camposur.BusinessLogic.Logic
{
    public class OrderLogic : IOrderLogic
    {
        public bool CreateOrder(OrderViewModel orderVM)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public OrderViewModel GetOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public List<OrderViewModel> GetOrders()
        {
            throw new NotImplementedException();
        }

        public List<OrderViewModel> GetOrdersByCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateOrder(OrderViewModel orderVM)
        {
            throw new NotImplementedException();
        }
    }
}
