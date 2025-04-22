using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.entity
{
    public class Loan
    {
        public int LoanId { get; set; }
        public Customer Customer { get; set; }
        public decimal PrincipalAmount { get; set; }
        public double InterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }

        public Loan() { }
        public Loan(int id, Customer customer, decimal principal, double rate, int term, string type, string status)
        {
            LoanId = id; Customer = customer; PrincipalAmount = principal; InterestRate = rate; LoanTerm = term; LoanType = type; LoanStatus = status;
        }
    }
}
