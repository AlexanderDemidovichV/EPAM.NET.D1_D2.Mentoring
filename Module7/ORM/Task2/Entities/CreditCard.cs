using System;

namespace Task2.Entities
{
    public class CreditCard
    {
        public int CreditCardID { get; set; }

        public string Number { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string CardHolderName { get; set; }

        public virtual Employee Holder { get; set; }
    }
}
