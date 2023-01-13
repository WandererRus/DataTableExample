using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace AdoNetSample2
{
    public partial class Form1 : Form
    {
        private SqlDataReader reader;
        private DataTable table;
        private SqlConnection conn;
        private SqlCommand comm;

        string cs = "";

        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection();
            cs = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            conn.ConnectionString = cs;
        }

        private void show_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = tbRequest.Text;
                    comm.Connection = conn;

                    dataGridView1.DataSource = null;

                    conn.Open();

                    table = new DataTable();
                    reader = comm.ExecuteReader();
                    int line = 0;

                    do
                    {
                        
                        while (reader.Read())
                        {
                            if (line == 0)
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    table.Columns.Add(reader.GetName(i));
                                }
                                line++;
                            }
                            DataRow row = table.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader[i];
                            }
                            table.Rows.Add(row);
                        }
                    } while (reader.NextResult());
                    dataGridView1.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Probably wrong request syntax");
                }
                finally
                {
                    // Close the connection
                    if (conn != null)
                    {
                        conn.Close();
                    }
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
