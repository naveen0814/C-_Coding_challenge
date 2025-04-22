using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoanManagementSystem.entity;
public interface ILoanRepository
{
    int ApplyLoan(int customerId, decimal principal, double interestRate, int term, string loanType);
    void InsertHomeLoanDetails(int loanId, string propertyAddress, int propertyValue);
    void InsertCarLoanDetails(int loanId, string carModel, int carValue);

    decimal CalculateInterest(int loanId);
    decimal CalculateInterest(decimal principal, double interestRate, int term);

    string CheckLoanStatus(int loanId);

    decimal CalculateEMI(int loanId);
    decimal CalculateEMI(decimal principal, double rate, int term);

    void LoanRepayment(int loanId, decimal amount);

    List<Loan> GetAllLoans();
    Loan GetLoanById(int loanId);

    void DisplayAllTables();
}
