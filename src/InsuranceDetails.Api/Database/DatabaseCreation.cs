using Microsoft.Data.SqlClient;

namespace InsuranceDetails.Api.Database;

public static class DatabaseCreation
{
    public static void MakeSureDatabaseExist(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "master"
        };
        
        using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        var sql = "IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'InsuranceDetailsDb') BEGIN CREATE DATABASE [InsuranceDetailsDb]; END";
        command.CommandText = sql; 
        
        command.ExecuteNonQuery();
    }
}