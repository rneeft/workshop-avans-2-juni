using System.Reflection;
using Microsoft.Data.SqlClient;

namespace InsuranceDetails.Api.Database;

public static class DatabaseInitialisation
{
    public static void CreateTheDatabase(string connectionString, string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);
        
        var sqlScript = reader.ReadToEnd();

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = new SqlCommand(sqlScript, connection);
        command.ExecuteNonQuery();
    }
}