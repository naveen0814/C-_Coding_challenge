using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.entity
{
    public class HomeLoan : Loan
    {
        public string PropertyAddress { get; set; }
        public int PropertyValue { get; set; }

        public HomeLoan() { }
        public HomeLoan(int id, Customer customer, decimal principal, double rate, int term, string status, string address, int value)
            : base(id, customer, principal, rate, term, "HomeLoan", status)
        {
            PropertyAddress = address; PropertyValue = value;
        }
    }
}
