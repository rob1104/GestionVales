using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace GestionValesRdz
{
    public static class AyudanteDeConexion
    {
        public static string CrearCadenaDeConexion(string dataSource, string baseDatos)
        {
            const string appName = "EntityFramework";
            const string providerName = "System.Data.SqlClient";
            const string ENTITY_FRAMEWORK_METADATOS = "res://*/ValesRdzModel.csdl|res://*/ValesRdzModel.ssdl|res://*/ValesRdzModel.msl";

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = dataSource,
                InitialCatalog = baseDatos,
                MultipleActiveResultSets = true,
                UserID = "sa",
                Password = "9753186400",
                ApplicationName = appName
            };

            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder()
            {
                Metadata = ENTITY_FRAMEWORK_METADATOS,
                Provider = providerName,
                ProviderConnectionString = sqlBuilder.ConnectionString
            };

            return efBuilder.ConnectionString;
        }

        public static string CrearCadenaSencilla(string dataSource, string baseDatos)
        {
            return string.Format("Server={0};Database={1};User Id=sa;Password=9753186400;", dataSource, baseDatos);
        }

        public static ValesRdzDatosEntities CreareConexion(string dataSource, string baseDatos)
        {
            return new ValesRdzDatosEntities(CrearCadenaDeConexion(dataSource, baseDatos));
        }
    }
}
