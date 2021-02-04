using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Examen1CBhering
{
    public partial class Fenetre : Form
    {
        //=====================================
        // Pour empecher la suppression d'un composant qui participe 
        // dans une commande, nous devons definir la clé étrangère 
        // aussi entre les DataTables. Alors, les deux DataTable "composants"
        // et "commandes" doivent être disponibles en mémoire, pour faire 
        // marcher la clé étrengère correctment, même si on veut utiliser
        // seulement une table pour afficher les commandes.
        //====================================

        // un adapter pour "composants" et l'autre pour "commandes" 
        private SqlDataAdapter adapterClients;
        private SqlDataAdapter adapterCommandes;

        // Initialiser ds une seule fois dans le programme
        // pas à chaque fois qu'on montre "composants" ou "commandes"
        private DataSet ds = new DataSet();

        private String examen1ConnectionString;

        internal string Examen1ConnectionString { get => examen1ConnectionString; }


        public Fenetre()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Dock = DockStyle.Fill;
        }

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource1.DataSource = CoucheDonnees.Client.GetClients();
            bindingSource1.Sort = "ClientId";
            dataGridView1.DataSource = bindingSource1;
        }

        private void commandesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource2.DataSource = CoucheDonnees.Commandes.GetCommandes();
            bindingSource2.Sort = "ComId";
            dataGridView1.DataSource = bindingSource2;

            dataGridView1.Columns["ComId"].HeaderText = "Commande ID";
            dataGridView1.Columns["ClientId"].HeaderText = "Client ID";
            dataGridView1.Columns["ComId"].DisplayIndex = 0;
            dataGridView1.Columns["Description"].DisplayIndex = 1;
            dataGridView1.Columns["Prix"].DisplayIndex = 2;
            dataGridView1.Columns["ClientId"].DisplayIndex = 3;
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            CoucheDonnees.Client.UpdateClients();
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            if (-2 == CoucheAffaires.Commandes.UpdateCommandes())
            {
                MessageBox.Show("Commande rejetée: inférieur à 10.00 CDN$");
                CoucheDonnees.DataTables.getDataSet().Tables["Commandes"].RejectChanges();
            }
        }
    }
}
