using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace CoucheAffaires
{
    internal class Commandes
    {
        private static DataSet ds = CoucheDonnees.DataTables.getDataSet();
        static internal int UpdateCommandes()
        {
            DataTable dt = ds.Tables["Commandes"].GetChanges(DataRowState.Added);
            if (dt == null)
            {
                dt = ds.Tables["Commandes"].GetChanges(DataRowState.Modified);
            }
            if ((dt != null) && (dt.Rows.Count == 1))
            {
                DataRow r = dt.Rows[0];
                if (Convert.ToDouble(r["Prix"]) > 10.0)
                {
                    return CoucheDonnees.Commandes.UpdateCommandes();
                }
                else
                {
                    return -2;
                }
            }
            else
            {
                return CoucheDonnees.Commandes.UpdateCommandes();
            }
        }
    }
}
