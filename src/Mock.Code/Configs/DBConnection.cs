using Mock.Code.Security;

namespace Mock.Code.Configs
{
    public class DbConnection
    {
        public static bool Encrypt { get; set; }
        public DbConnection(bool encrypt)
        {
            Encrypt = encrypt;
        }
        public static string ConnectionString
        {
            get
            {
                string connection = System.Configuration.ConfigurationManager.ConnectionStrings["MockDbContext"].ConnectionString;
                if (Encrypt == true)
                {
                    return DesEncrypt.Decrypt(connection);
                }
                else
                {
                    return connection;
                }
            }
        }
    }
}
