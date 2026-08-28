using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace EjemploSQLCommandInsertar
{
    public partial class AgregarCategoria : Window
    {
        // Cambia esta cadena de conexión según la que ya te esté funcionando en tu proyecto
        private string connectionString = "Server=localhost;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;";

        public AgregarCategoria()
        {
            InitializeComponent();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string nombreProducto = txtNombreProducto.Text.Trim();
            string categoria = txtCategoria.Text.Trim();

            if (string.IsNullOrEmpty(nombreProducto) || string.IsNullOrEmpty(txtPrecio.Text) || string.IsNullOrEmpty(categoria))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser un valor numérico válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    using (SqlCommand comando = new SqlCommand("dbo.SP_AgregarProductoConCategoria", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@ProductName", nombreProducto);
                        comando.Parameters.AddWithValue("@UnitPrice", precio);
                        comando.Parameters.AddWithValue("@CategoryName", categoria);

                        using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                        {
                            DataTable dt = new DataTable();
                            adaptador.Fill(dt);

                            // Mostrar el resultado en el DataGrid
                            dgResultado.ItemsSource = dt.DefaultView;
                        }
                    }
                }

                MessageBox.Show("¡Producto y categoría procesados con éxito!", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error Crítico", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}