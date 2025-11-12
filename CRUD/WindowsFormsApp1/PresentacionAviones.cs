using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class PresentacionAviones : Form
    {
        //USING SYSTEM CONFIGURATION
        
        List<Avion> listaAviones = new List<Avion>();
        public PresentacionAviones() //CONSTRUCTOR DE LA CLASE
        {
            InitializeComponent();
        }
        private void CargarListBox() //CARGAR LISTBOX
        {
            listBox1.Items.Clear();
            foreach (Avion avion in listaAviones)
            {
                listBox1.Items.Add(avion);
            }
        }
        private List<Avion> GetAviones() //SERÍA COMO UN SELECT
        {
            List<Avion> result = new List<Avion>();
            result.Clear();
            using (SqlConnection connection = Db.GetConnection())
            {
                string sql = "SELECT * FROM Aviones";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Avion avion = new Avion
                            {
                                id = reader.GetInt32(0),
                                modelo = reader.GetString(1),
                                cantidadPasajeros = reader.GetInt32(2)
                            };
                            result.Add(avion);
                        }
                    }
                }
            }
            return result;
        }
        private void UpdateListBox() //ACTUALIZAR EL LISTBOX
        {
            listaAviones = GetAviones();
            CargarListBox();
        }
        private void UpdateDataGrid() //ACTUALIZAR EL DATAGRID
        {
            avionesBindingSource.DataSource = null;
            avionesBindingSource.DataSource = GetAviones();
        }
        private void Clean()
        {
            txtCantidad.Clear();
            txtModelo.Clear();
        }
        private void PresentacionAviones_Load(object sender, EventArgs e) //AL INICIAR EL FORMULARIO
        {
            UpdateDataGrid();
            UpdateListBox();
        }


        //-----------------BOTONES-----------------
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string modelo = txtModelo.Text;
            int cantidadPasajeros = int.Parse(txtCantidad.Text);
            using (SqlConnection connection = Db.GetConnection())
            {
                string sql = "INSERT INTO Aviones (modelo, cantidadPasajeros) VALUES (@modelo, @cantidad)";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    cmd.Parameters.Add("@modelo", System.Data.SqlDbType.NVarChar).Value = modelo;
                    cmd.Parameters.Add("@cantidad", System.Data.SqlDbType.Int).Value = cantidadPasajeros;
                    cmd.ExecuteNonQuery();
                }
                Clean();
                UpdateDataGrid();
                UpdateListBox();
            }
            
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int idSeleccionado = int.Parse(labelId.Text);
            using (SqlConnection connection = Db.GetConnection())
            {
                string sql = "DELETE FROM Aviones WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = idSeleccionado;
                    cmd.ExecuteNonQuery();
                }
                Clean();
                UpdateDataGrid();
                UpdateListBox();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            int idSeleccionado = int.Parse(labelId.Text);
            string modelo = txtModelo.Text;
            int cantidadPasajeros = int.Parse(txtCantidad.Text);
            using (SqlConnection connection = Db.GetConnection())
            {
                string sql = "UPDATE Aviones SET modelo = @modelo, cantidadPasajeros = @cantidad WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = idSeleccionado;
                    cmd.Parameters.Add("@modelo", System.Data.SqlDbType.NVarChar).Value = modelo;
                    cmd.Parameters.Add("@cantidad", System.Data.SqlDbType.Int).Value = cantidadPasajeros;
                    cmd.ExecuteNonQuery();
                }
                Clean();
                UpdateDataGrid();
                UpdateListBox();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Clean();
            txtFiltro.Clear();
            UpdateDataGrid();
            UpdateListBox();
        }

        private void btnFiltro_Click(object sender, EventArgs e) //FILTRO CON LINQ
        {
            listaAviones = GetAviones();
            if (string.IsNullOrWhiteSpace(txtFiltro.Text)) return;
            int cantidadPasajeros = int.Parse(txtFiltro.Text);
            listaAviones = listaAviones.Where(a => a.cantidadPasajeros == cantidadPasajeros).ToList(); //<--- LINQ
            CargarListBox(); //VA A FILTRAR EL LISTBOX
        }

        private void btnSorpresa_Click(object sender, EventArgs e)
        {
            Sorpresa sorpresa = new Sorpresa();
            sorpresa.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)//PARA SELECCIONAR DESDE EL LISTBOX
        {
            Avion avion = (Avion)listBox1.SelectedItem;
            txtModelo.Text = avion.modelo;
            txtCantidad.Text = avion.cantidadPasajeros.ToString(); //AL SER UN INT, NO OLVIDARSE DE PONER UN TOSTRING PARA MOSTRAR LOS DATOS.
            labelId.Text = avion.id.ToString(); 
            //PARA TOMAR DATOS DESDE LOS LABELS-TEXTBOX = INT.PARSE
            //PARA MOSTRAR DATOS = TOSTRING()
        }
    }
}
