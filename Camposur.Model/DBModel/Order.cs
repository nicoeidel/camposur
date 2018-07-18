using System;
using System.Collections.Generic;
using static Camposur.Core.Enums;

namespace Camposur.Model.DBModel
{
    public class Order: BaseEntity
    {
        public string Description { get; set; }

        public DateTime ArrivalDate { get; set; }

        public decimal Amount { get; set; }

        public int AddressId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public OrderStatus Status { get; set; }

        public OrderAddress Address { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}
