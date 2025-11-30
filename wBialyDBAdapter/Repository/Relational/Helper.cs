using System.Data;

namespace wBialyDBAdapter.Repository.Relational
{
    public static class DbCommandExtensions
    {
        public static void AddParam(this IDbCommand cmd, string name, object? value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(param);
        }
    }
}
