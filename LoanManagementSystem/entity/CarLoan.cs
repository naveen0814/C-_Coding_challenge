using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.entity
{
    public class CarLoan : Loan
    {
        public string CarModel { get; set; }
        public int CarValue { get; set; }

        public CarLoan() { }
        public CarLoan(int id, Customer customer, decimal principal, double rate, int term, string status, string model, int value)
            : base(id, customer, principal, rate, term, "CarLoan", status)
        {
            CarModel = model; CarValue = value;
        }
    }
}
