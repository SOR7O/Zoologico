using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//Agregando los namespaces necesarios para SQL Server
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace Zoologico
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Varible miembro
        SqlConnection sqlConnection;
   
        public MainWindow()
        {
            InitializeComponent();
            //ZoologicoConnectionString
            string connectionString = ConfigurationManager.ConnectionStrings["Zoologico.Properties.Settings.ZoologicoConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            MostrarZoologico();
            MostrarAnimales();
            
        }
       
       
        private void MostrarZoologico()
        {
            try
            {
                 MessageBox.Show("Entrando al try");
                //El query a realizar en la base de datos
                string query = "SELECT * FROM ZOO.Zoologico";

                //sqlDataAdapater es una intrefaz entre las tablas y los objetos utilizables de C#
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query,sqlConnection);
                using (sqlDataAdapter)
                {
                    //Objeto en C# que refleja una tabla de BD
                    DataTable tablaZoologico = new DataTable();
                    //Llenar el objeto de tipo DataTable
                    sqlDataAdapter.Fill(tablaZoologico);
                    //Desplegar el campo que se desea mandar a llamar|| para el usuario
                    lbZoologicos.DisplayMemberPath = "ciudad";
                    //valor entregado|| Para el prorgramadoe
                    lbZoologicos.SelectedValuePath = "id";
                    //Referencia de los datos para el listbox(popular)
                    lbZoologicos.ItemsSource = tablaZoologico.DefaultView;
                    MessageBox.Show("Exitosa");

                }
            }
            catch(Exception e)
            {

                MessageBox.Show(e.ToString(),e.Message);
            }
        }
        private void MostrarAnimalesZoologico()
        {
            try
            {
                string query = @"SELECT * FROM ZOO.Animal a INNER JOIN ZOO.AnimalZoologico b
                               ON a.id=b.idAnimal WHERE b.idZoologico=@zooId";
                //Comando SQL
                SqlCommand sqlcommand= new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlcommand);
                using (sqlDataAdapter)
                {
                    //Reemplazar el valor del parametro del query 
                    sqlcommand.Parameters.AddWithValue("@zooId", lbZoologicos.SelectedValue);
                    DataTable tablaAnimalZoologico = new DataTable();

                    sqlDataAdapter.Fill(tablaAnimalZoologico);
                    lbAnimalesZoologicos.DisplayMemberPath = "nombre";
                    lbAnimalesZoologicos.SelectedValuePath = "id";
                    lbAnimalesZoologicos.ItemsSource = tablaAnimalZoologico.DefaultView;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString(),e.Message);
            }
        }
        private void MostrarAnimales()
        {
            try
            {
                string query = "SELECT * FROM ZOO.Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                using (sqlDataAdapter)
                {
                    DataTable tablaAnimal = new DataTable();
                    sqlDataAdapter.Fill(tablaAnimal);
                    lbAnimales.DisplayMemberPath = "nombre";
                    lbAnimales.SelectedValue = "id";
                    lbAnimales.ItemsSource = tablaAnimal.DefaultView;

                }


            }
            catch
            {

            }
        }


        private void LbZoologicos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MostrarAnimalesZoologico();

        }
    }
}
