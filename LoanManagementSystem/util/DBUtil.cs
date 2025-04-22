using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
public class DBUtil
{
    private static string connectionString = "Server=NAVEEN;Database=LOANMANAGEMENTSYSTEM;Integrated Security=True;TrustServerCertificate=True";
    public static SqlConnection GetDBConnection()
    {
        return new SqlConnection(connectionString);
    }
}