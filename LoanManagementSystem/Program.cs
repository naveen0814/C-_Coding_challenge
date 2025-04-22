using LoanManagementSystem.entity;
using System;
class MainModule
{
    static void Main(string[] args)
    {
        ILoanRepository repo = new LoanRepositoryImpl();
        while (true)
        {
            Console.WriteLine("\n1. Apply Loan\n2. Get All Loans\n3. Get Loan by ID\n4. Repay Loan\n5. Calculate Interest\n6. Check Loan Status\n7. Calculate EMI\n8. Display All Tables\n9. Exit");
            Console.Write("Enter choice: ");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.Write("Enter Customer ID: ");
                    int custId = int.Parse(Console.ReadLine());

                    Console.Write("Enter Principal Amount: ");
                    decimal principal = decimal.Parse(Console.ReadLine());

                    Console.Write("Enter Interest Rate: ");
                    double rate = double.Parse(Console.ReadLine());

                    Console.Write("Enter Loan Tenure (months): ");
                    int term = int.Parse(Console.ReadLine());

                    Console.Write("Enter Loan Type (HomeLoan/CarLoan): ");
                    string loanType = Console.ReadLine();

                    Console.Write("Confirm apply loan? (Yes/No): ");
                    string confirm = Console.ReadLine();

                    if (confirm.ToLower() == "yes")
                    {
                        int loanId = repo.ApplyLoan(custId, principal, rate, term, loanType);

                        if (loanType == "HomeLoan")
                        {
                            Console.Write("Enter Property Address: ");
                            string address = Console.ReadLine();

                            Console.Write("Enter Property Value: ");
                            int value = int.Parse(Console.ReadLine());

                            repo.InsertHomeLoanDetails(loanId, address, value);
                        }
                        else if (loanType == "CarLoan")
                        {
                            Console.Write("Enter Car Model: ");
                            string model = Console.ReadLine();

                            Console.Write("Enter Car Value: ");
                            int value = int.Parse(Console.ReadLine());

                            repo.InsertCarLoanDetails(loanId, model, value);
                        }

                        Console.WriteLine("Loan application submitted!");
                    }
                    break;

                case 2:
                    foreach (var l in repo.GetAllLoans())
                        Console.WriteLine($"LoanId: {l.LoanId}, Type: {l.LoanType}, Status: {l.LoanStatus}, Amount: {l.PrincipalAmount}");
                    break;
                case 3:
                    Console.Write("Enter Loan ID: ");
                    int lid = int.Parse(Console.ReadLine());
                    var loanData = repo.GetLoanById(lid);
                    Console.WriteLine($"LoanId: {loanData.LoanId}, Type: {loanData.LoanType}, Status: {loanData.LoanStatus}, Amount: {loanData.PrincipalAmount}");
                    break;
                case 4:
                    Console.Write("Enter Loan ID: ");
                    int repayId = int.Parse(Console.ReadLine());
                    Console.Write("Enter Amount: ");
                    decimal amount = decimal.Parse(Console.ReadLine());
                    repo.LoanRepayment(repayId, amount);
                    break;
                case 5:
                    Console.Write("Enter Loan ID to calculate interest: ");
                    int intId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Interest: " + repo.CalculateInterest(intId));
                    break;
                case 6:
                    Console.Write("Enter Loan ID to check status: ");
                    int statId = int.Parse(Console.ReadLine());
                    repo.CheckLoanStatus(statId);
                    break;
                case 7:
                    Console.Write("Enter Loan ID to calculate EMI: ");
                    int emiId = int.Parse(Console.ReadLine());
                    Console.WriteLine("EMI: " + repo.CalculateEMI(emiId));
                    break;
                case 8:
                    repo.DisplayAllTables();
                    break;
                case 9:
                    return;
            }
        }
    }
}
