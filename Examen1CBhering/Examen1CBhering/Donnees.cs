using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace CoucheDonnees
{
    internal class Connect
    {
        private String examen1ConnectionString;
        private SqlConnection con;

        private Connect()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "examen1";
            cs.UserID = "cbhering";
            cs.Password = "cbhering";
            this.examen1ConnectionString = cs.ConnectionString;
            this.con = new SqlConnection(this.examen1ConnectionString);
        }

        static private Connect singleton = new Connect();
        static internal SqlConnection Connection { get => singleton.con; }
        static internal String ConnectionString { get => singleton.examen1ConnectionString; }
    }

    internal class DataTables
    {
        private SqlDataAdapter adapterClient;
        private SqlDataAdapter adapterCommandes;

        private DataSet ds = new DataSet();

        private void chargerClient()
        {

            adapterClient = new SqlDataAdapter("SELECT * FROM client ORDER BY ClientId, " +
                "Nom", Connect.ConnectionString);

            adapterClient.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                      
            adapterClient.Fill(ds, "client");

            ds.Tables["Client"].Columns["ClientId"].AllowDBNull = false;
            ds.Tables["Client"].Columns["Nom"].AllowDBNull = false;


            ds.Tables["Client"].PrimaryKey = new DataColumn[1]
                  { ds.Tables["client"].Columns["ClientId"]};

            SqlCommandBuilder builder = new SqlCommandBuilder(adapterClient);
            adapterClient.UpdateCommand = builder.GetUpdateCommand();
        }

        private void chargerCommandes()
        {

            adapterCommandes = new SqlDataAdapter(
                 "SELECT * FROM commandes ORDER BY ComId",
                 Connect.ConnectionString);

            adapterCommandes.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterCommandes.Fill(ds, "Commandes");

            ForeignKeyConstraint myFK = new ForeignKeyConstraint("MyFK",
            new DataColumn[]{
                ds.Tables["client"].Columns["clientId"]
            },
            new DataColumn[] {
                ds.Tables["commandes"].Columns["clientId"]
            }
            );
            myFK.DeleteRule = Rule.None;
            myFK.UpdateRule = Rule.None;
            ds.Tables["commandes"].Constraints.Add(myFK);

            SqlCommandBuilder builder = new SqlCommandBuilder(adapterCommandes);
            adapterCommandes.UpdateCommand = builder.GetUpdateCommand();

        }

        private DataTables()
        {
  
            chargerClient();
            chargerCommandes();
        }

        static private DataTables singleton = new DataTables();

        internal static SqlDataAdapter getAdapterClients()
        {
            return singleton.adapterClient;
        }
        internal static SqlDataAdapter getAdapterCommandes()
        {
            return singleton.adapterCommandes;
        }
        internal static DataSet getDataSet()
        {
            return singleton.ds;
        }
    }

    internal class Client
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterClients();
        private static DataSet ds = DataTables.getDataSet();

        static internal DataTable GetClients()
        {
            return ds.Tables["Client"];
        }
        static internal int UpdateClients()
        {
            if (!ds.Tables["client"].HasErrors)
            {
                return adapter.Update(ds.Tables["client"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Commandes
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterCommandes();
        private static DataSet ds = DataTables.getDataSet();

        static internal DataTable GetCommandes()
        {
            ds.Tables["Commandes"].Clear();
            adapter.Fill(ds, "Commandes");
            return ds.Tables["Commandes"];
        }

        static internal int UpdateCommandes()
        {
            if (!ds.Tables["Commandes"].HasErrors)
            {
                return adapter.Update(ds.Tables["Commandes"]);
            }
            else
            {
                return -1;
            }
        }

    }  
}
