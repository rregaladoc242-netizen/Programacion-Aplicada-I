using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegistroImpresiones
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Variables globales para almacenar los totales de páginas impresas (espacios de memoria)
        int totalEscolar = 0;
        int totalUniversitario = 0;
        int totalOrganizacion = 0;

        public MainWindow()
        {
            InitializeComponent();

            // Colocar los valores estadísticos iniciales en cero al abrir la ventana
            txtEstEscolar.Text = "0";
            txtEstUniversitario.Text = "0";
            txtEstOrganizacion.Text = "0";
        }

        private void btnEstadistica_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validar que las entradas requeridas no estén vacías
            if (string.IsNullOrWhiteSpace(txtCliente.Text) || string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del cliente y la cantidad de páginas.", "Campos vacíos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Validar que la cantidad ingresada sea un número entero coherente
            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Por favor, ingrese una cantidad de páginas válida mayor a cero.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string tarifaNombre = "";
            double precioPorPagina = 0.0;
            double importeTotal = 0.0;

            // 3. Evaluar la tarifa seleccionada mediante los RadioButtons y acumular estadísticas
            if (rbEscolar.IsChecked == true)
            {
                tarifaNombre = "Escolar";
                precioPorPagina = 0.10; // Tarifa estándar base
                totalEscolar += cantidad;
                txtEstEscolar.Text = totalEscolar.ToString();
            }
            else if (rbUniversitario.IsChecked == true)
            {
                tarifaNombre = "Universitario";
                precioPorPagina = 0.15; // Tarifa estándar intermedia
                totalUniversitario += cantidad;
                txtEstUniversitario.Text = totalUniversitario.ToString();
            }
            else if (rbOrganizacion.IsChecked == true)
            {
                tarifaNombre = "Organización";
                precioPorPagina = 0.20; // Tarifa estándar corporativa
                totalOrganizacion += cantidad;
                txtEstOrganizacion.Text = totalOrganizacion.ToString();
            }

            // 4. Calcular el costo final del registro
            importeTotal = cantidad * precioPorPagina;

            // 5. Agregar ordenadamente los datos en cada lista independiente de la interfaz
            lstClientes.Items.Add(txtCliente.Text);
            lstCelulares.Items.Add(txtCelular.Text);
            lstCantidades.Items.Add(cantidad.ToString());
            lstTarifas.Items.Add(tarifaNombre);
            lstImportes.Items.Add(importeTotal.ToString("F2")); // Muestra el número con dos decimales

            // 6. Restablecer los controles de texto de entrada para una nueva captura
            txtCliente.Clear();
            txtCelular.Clear();
            txtCantidad.Clear();
            txtCliente.Focus();
        }
    }
}