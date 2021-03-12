using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Capa_de_negocios_ASELEC;

namespace Menu
{
    /// <summary>
    /// Lógica de interacción para FRM_auxiliar_de_ingreso_de_estudiantes.xaml
    /// </summary>
    public partial class FRM_auxiliar_de_ingreso_de_estudiantes : Window
    {
        //private Control_de_usuario_gestion_prestamos padre;
        CN_Estudiante NEST = new CN_Estudiante();
        Control_de_usuario_gestion_prestamos padre = null;

        public FRM_auxiliar_de_ingreso_de_estudiantes(Control_de_usuario_gestion_prestamos parametro)
        {
            InitializeComponent();
            padre = parametro;

        }
        public FRM_auxiliar_de_ingreso_de_estudiantes()
        {
            InitializeComponent();
        }
        private void cmb_Estado_de_aportacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_Estado_de_aportacion_aux.Text != "Aportante")
            {
                txt_Valor_de_aportacion_aux.Text = "20,00";
            }

            if (cmb_Estado_de_aportacion_aux.Text != "No Aportante")
            {
                txt_Valor_de_aportacion_aux.Text = "0,00";
            }
        }

        private void btn_regresar_admin_estudiantes_aux_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar el formulario?", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void btn_agregar_admin_estudiantes_aux_Click(object sender, RoutedEventArgs e)
        {


            string identificacion = txt_cedula_admin_estudiantes_aux.Text;

            if (Validacion_Identificacion.VerificaIdentificacion(identificacion) == false || identificacion.Length < 10)
            {
                MessageBox.Show("Verifique que la cédula ingresada sea válida", "Error en validación de datos", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
            else if (string.IsNullOrEmpty(txt_nombre_admin_estudiantes_aux.Text))
            {
                MessageBox.Show("Verifique que se ha llenado correctamente el Nombre del estudiante", "Error en ingreso de datos", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
            else if (string.IsNullOrEmpty(txt_cedula_admin_estudiantes_aux.Text))
            {
                MessageBox.Show("Verifique que la cédula ingresada sea válida", "Error en validación de datos", MessageBoxButton.OK, MessageBoxImage.Information);

                return;

            }
            else if (cmb_Estado_de_aportacion_aux.SelectedIndex == -1)
            {
                MessageBox.Show("Verifique que se escogío correctamente el Estado de Aportación", "Error en ingreso de datos", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
            else if (cmb_carrera_admin_estudiantes_aux.SelectedIndex == -1)
            {
                MessageBox.Show("Verifique que se escogío correctamente la Carrera", "Error en ingreso de datos", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
            else if (string.IsNullOrEmpty(txt_telefono_admin_estudiantes_aux.Text))
            {
                MessageBox.Show("Verifique que ha llenado correctamente el Teléfono del estudiante", "Error en ingreso de datos", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
            else if (Regex.IsMatch(txt_correo_admin_estudiantes_aux.Text,
                 @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                 @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"))
            {

            }
            else
            {

                MessageBox.Show("Verifique que el correo ingresado sea correcto", "Error en validación de datos", MessageBoxButton.OK, MessageBoxImage.Information);
                return;


            }


            if (Validacion_Identificacion.VerificaIdentificacion(identificacion) == true)
            {
                try
                {
                    char separator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    int id = NEST.Insertar(cmb_Estado_de_aportacion_aux.SelectedIndex + 1, txt_nombre_admin_estudiantes_aux.Text, txt_cedula_admin_estudiantes_aux.Text, cmb_carrera_admin_estudiantes_aux.SelectedIndex + 1, txt_correo_admin_estudiantes_aux.Text, txt_telefono_admin_estudiantes_aux.Text, txt_Valor_de_aportacion_aux.Text.Replace(',', separator));


                    MessageBox.Show("     Estudiante " + txt_nombre_admin_estudiantes_aux.Text + "\n Ingresado correctamente", "Ingreso de datos exitoso", MessageBoxButton.OK, MessageBoxImage.Information);


                    if (padre != null)
                    {
                        padre.txt_cedula_gpre.Text = txt_cedula_admin_estudiantes_aux.Text;
                        padre.txt_nombre_estudiante_gpre.Text = txt_nombre_admin_estudiantes_aux.Text;
                  
                        padre.txt_estado_de_aportacion_gpre.Text = cmb_Estado_de_aportacion_aux.Text;
                        //padre.txtTelefono.Text = TXT_TELEFONO.Text;
                        padre.idEst = id.ToString();
                       // padre.Show();
                        this.Close();
                    }
                   // limpiarForm();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("El estudiante con CI: " + txt_cedula_admin_estudiantes_aux.Text + " ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
    }
}
