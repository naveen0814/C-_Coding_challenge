using LoanManagementSystem.entity;
using LoanManagementSystem.exception;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class LoanRepositoryImpl : ILoanRepository
{
    public int ApplyLoan(int customerId, decimal principal, double interestRate, int term, string loanType)
    {
        using (SqlConnection conn = DBUtil.GetDBConnection())
        {
            conn.Open();
            string sql = "INSERT INTO Loan (CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus) OUTPUT INSERTED.LoanId VALUES (@CustomerId, @Principal, @Rate, @Term, @Type, 'Pending')";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CustomerId", customerId);
            cmd.Parameters.AddWithValue("@Principal", principal);
            cmd.Parameters.AddWithValue("@Rate", interestRate);
            cmd.Parameters.AddWithValue("@Term", term);
            cmd.Parameters.AddWithValue("@Type", loanType);
            return (int)cmd.ExecuteScalar();
        }
    }

    public void InsertHomeLoanDetails(int loanId, string propertyAddress, int propertyValue)
    {
        using (SqlConnection conn = DBUtil.GetDBConnection())
        {
            conn.Open();
            string sql = "INSERT INTO HomeLoan (LoanId, PropertyAddress, PropertyValue) VALUES (@LoanId, @Address, @Value)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            cmd.Parameters.AddWithValue("@Address", propertyAddress);
            cmd.Parameters.AddWithValue("@Value", propertyValue);
            cmd.ExecuteNonQuery();
        }
    }

    public void InsertCarLoanDetails(int loanId, string carModel, int carValue)
    {
        using (SqlConnection conn = DBUtil.GetDBConnection())
        {
            conn.Open();
            string sql = "INSERT INTO CarLoan (LoanId, CarModel, CarValue) VALUES (@LoanId, @Model, @Value)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            cmd.Parameters.AddWithValue("@Model", carModel);
            cmd.Parameters.AddWithValue("@Value", carValue);
            cmd.ExecuteNonQuery();
        }
    }

    public decimal CalculateInterest(int loanId)
    {
        Loan loan = GetLoanById(loanId);
        return CalculateInterest(loan.PrincipalAmount, loan.InterestRate, loan.LoanTerm);
    }

    public decimal CalculateInterest(decimal principal, double rate, int term)
    {
        return (principal * (decimal)rate * term)/12 ;
    }

    public string CheckLoanStatus(int loanId)
    {
        Loan loan = GetLoanById(loanId);

        if (loan == null)
        {
            throw new InvalidLoanException("Loan ID not found.");
        }

        if (loan.Customer.CreditScore > 650)
        {
            loan.LoanStatus = "Approved";
        }
        else
        {
            loan.LoanStatus = "Rejected";
        }

        using (SqlConnection conn = DBUtil.GetDBConnection())
        {
            conn.Open();
            string sql = "UPDATE Loan SET LoanStatus = @Status WHERE LoanId = @LoanId";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Status", loan.LoanStatus);
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            cmd.ExecuteNonQuery();
        }

        return loan.LoanStatus;
    }

    public decimal CalculateEMI(int loanId)
    {
        Loan loan = GetLoanById(loanId);
        return CalculateEMI(loan.PrincipalAmount, loan.InterestRate, loan.LoanTerm);
    }

    public decimal CalculateEMI(decimal principal, double rate, int term)
    {
        double monthlyRate = rate / 12 / 100;
        double emi = (double)principal * monthlyRate * Math.Pow(1 + monthlyRate, term) / (Math.Pow(1 + monthlyRate, term) - 1);
        return (decimal)emi;
    }

    public void LoanRepayment(int loanId, decimal amount)
    {
        Loan loan = GetLoanById(loanId);
        decimal emi = CalculateEMI(loanId);
        if (amount < emi)
        {
            Console.WriteLine("Amount is less than EMI. Cannot proceed.");
            return;
        }
        int emisPaid = (int)(amount / emi);
        loan.PrincipalAmount -= amount;

        using (SqlConnection conn = DBUtil.GetDBConnection())
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Loan SET PrincipalAmount = @Principal WHERE LoanId = @LoanId", conn);
            cmd.Parameters.AddWithValue("@Principal", loan.PrincipalAmount);
            cmd.Parameters.AddWithValue("@LoanId", loan.LoanId);
            cmd.ExecuteNonQuery();
        }

        Console.WriteLine($"Number of EMIs paid: {emisPaid}");
        Console.WriteLine($"Remaining Principal: {loan.PrincipalAmount}");
    }

    public List<Loan> GetAllLoans()
    {
        List<Loan> loans = new List<Loan>();
        using (SqlConnection conn = DBUtil.GetDBConnection())
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Loan", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                loans.Add(new Loan
                {
                    LoanId = reader.GetInt32(0),
                    PrincipalAmount = reader.GetDecimal(2),
                    InterestRate = reader.GetDouble(3),
                    LoanTerm = reader.GetInt32(4),
                    LoanType = reader.GetString(5),
                    LoanStatus = reader.GetString(6)
                });
            }
        }
        return loans;
    }

    public Loan GetLoanById(int loanId)
    {
        using (SqlConnection conn = DBUtil.GetDBConnection())
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT l.*, c.* FROM Loan l JOIN Customer c ON l.CustomerId = c.CustomerId WHERE l.LoanId = @LoanId", conn);
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Customer customer = new Customer(reader.GetInt32(7), reader.GetString(8), reader.GetString(9), reader.GetString(10), reader.GetString(11), reader.GetInt32(12));
                return new Loan
                {
                    LoanId = reader.GetInt32(0),
                    Customer = customer,
                    PrincipalAmount = reader.GetDecimal(2),
                    InterestRate = reader.GetDouble(3),
                    LoanTerm = reader.GetInt32(4),
                    LoanType = reader.GetString(5),
                    LoanStatus = reader.GetString(6)
                };
            }
            else throw new InvalidLoanException("Loan not found.");
        }
    }


    public void DisplayAllTables()
    {
        using (SqlConnection conn = DBUtil.GetDBConnection())
        {
            conn.Open();
            string[] tables = { "Customer", "Loan", "HomeLoan", "CarLoan" };
            foreach (string table in tables)
            {
                Console.WriteLine($"\n--- {table} Table ---");
                SqlCommand cmd = new SqlCommand($"SELECT * FROM {table}", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader[i]}  ");
                    }
                    Console.WriteLine();
                }
                reader.Close();
            }
        }
    }
}