﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
using Capa_Comun_Aselec;
using Capa_de_negocios_ASELEC;

namespace Menu
{
    /// <summary>
    /// Lógica de interacción para Control_de_usuario_gestion_prestamos.xaml
    /// </summary>
    public partial class Control_de_usuario_gestion_prestamos : UserControl
    {
        CN_Estudiante estudiantes = new CN_Estudiante();
        CN_Articulo articuloCN = new CN_Articulo();
        CN_Prestamo prestamoCN = new CN_Prestamo();
        DataRowView articuloSeleccionadoRow;
        DataRow estudianteActualRow;
        public string idEst = null;
        double totalAPagar = 0;
        FRM_auxiliar_de_ingreso_de_estudiantes nuevoEst;

        FRM_auxiliar_de_ingreso_de_estudiantes nuevoest = new FRM_auxiliar_de_ingreso_de_estudiantes();
        public Control_de_usuario_gestion_prestamos()
        {
            InitializeComponent();
            txt_responsable_gpre.Text = Usuario_cache.Nombre;
            txtFecha_de_registro_gpre.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_cedula_gpre.Text)|| txt_cedula_gpre.Text.Length<10)
            {

                MessageBox.Show("Ingrese correctamente un dato en el número de cédula", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else {
                List<Prestamo> detalles = dtg_detalle_alq.Items.Cast<Prestamo>().ToList();
                prestamoCN.insertarPrestamo(
                    Convert.ToInt32(estudianteActualRow[0]),
                    detalles
                );
                MessageBox.Show("Préstamo registrado con éxito, el estudiante " + estudianteActualRow[4] + " deberá devolver " + articuloSeleccionadoRow[1] + " en " + detalles[0].Tiempo + " horas.");
                limpiar();
            }
        }
        private void txtBuscar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscar_articulo.Text))

            {
                txtBuscar_articulo.Visibility = System.Windows.Visibility.Collapsed;

                txtBuscar_marca.Visibility = System.Windows.Visibility.Visible;

            }
        }

        private void txtBuscar_marca_GotFocus(object sender, RoutedEventArgs e)
        {
            txtBuscar_marca.Visibility = System.Windows.Visibility.Collapsed;

            txtBuscar_articulo.Visibility = System.Windows.Visibility.Visible;

            txtBuscar_articulo.Focus();
        }

        private void btn_buscar_cedula_gpre_Click(object sender, RoutedEventArgs e)
        {
            string identificacion = txt_cedula_gpre.Text;
            char separator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            if (Validacion_Identificacion.VerificaIdentificacion(identificacion) == false || identificacion.Length < 10)
            {
                MessageBox.Show("Verifique identificación del cliente");
                btn_registrar_alquiler_gpre.IsEnabled = false;
                return;

               
            }
            else
            {
                DataTable respuesta = estudiantes.BuscarPorCedula(txt_cedula_gpre.Text);
                if (respuesta.Rows.Count > 0)
                {
                    DataRow fila = respuesta.Rows[0];
                    estudianteActualRow = fila;
                    txt_nombre_estudiante_gpre.Text = fila[4].ToString();
                    txt_estado_de_aportacion_gpre.Text = fila[2].ToString();
                    double descuento = 0;
                    if (fila[1].ToString() == "2")
                    {
                        txt_estado_de_aportacion_gpre.Text = "Aportante";
                        // descuento = totalApagar * 0.3;
                        descuento = totalAPagar * 0.3;

                        btn_registrar_alquiler_gpre.IsEnabled = true;
                    }
                    else {
                        txt_estado_de_aportacion_gpre.Text = "No Aportante";

                    }
                    totalAPagar = totalAPagar - descuento;
                    // txt_descuento_apor_gpre.Text = descuento.ToString();
                    txt_descuento_apor_gpre.Text= Math.Round(descuento, 3).ToString("0.00").Replace(',', separator);
                    //txt_valor_estimado_gpre.Text = totalAPagar.ToString();
                    txt_valor_estimado_gpre.Text= Math.Round(totalAPagar, 3).ToString("0.00").Replace(',', separator);

                    btn_registrar_alquiler_gpre.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Cliente no registrado, proceda a ingresar sus datos");
                    nuevoEst = new FRM_auxiliar_de_ingreso_de_estudiantes(this);
                    nuevoEst.txt_cedula_admin_estudiantes_aux.Text = txt_cedula_gpre.Text;
                    nuevoEst.ShowDialog();
                    btn_registrar_alquiler_gpre.IsEnabled = true;
                }
               // btnCF.IsEnabled = true;



            }
        }

        private void btn_buscar_nombre_articulo_gpre_Click(object sender, RoutedEventArgs e)
        {
            dtg_lista_de_articulos.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = articuloCN.buscarArticulo(txtBuscar_articulo.Text) });
        }

        private void dtg_lista_de_articulos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView rowView = dtg_lista_de_articulos.SelectedItem as DataRowView;
            if (rowView != null)
            {
                txt_nombre_de_articulo.Text = rowView[1].ToString();
                articuloSeleccionadoRow = rowView;

                btn_agregar_detalle_alquiler.IsEnabled = true;
                btn_registrar_alquiler_gpre.IsEnabled = false;
                btn_buscar_cedula_gpre.IsEnabled = false;
            }

            btn_agregar_detalle_alquiler.IsEnabled = true;
           // btn_registrar_alquiler_gpre.IsEnabled = true;
            btn_buscar_cedula_gpre.IsEnabled = true;
        }

        private void btn_agregar_detalle_alquiler_Click(object sender, RoutedEventArgs e)
        {
            // int articulosDisponibles = Convert.ToInt32(articuloSeleccionadoRow[4].ToString());
            // int horasAlquiladas = Convert.ToInt32(txt_tiempo_de_alquiler.Text);

            if (string.IsNullOrEmpty(txt_tiempo_de_alquiler.Text))
            {
                MessageBox.Show("No se puede agregar al detalle con una cantidad nula", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;


            }

            else if (Convert.ToInt32(articuloSeleccionadoRow[4].ToString())<=0)
            {
                MessageBox.Show("Artículo no disponible para prestamos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (Convert.ToInt32(txt_tiempo_de_alquiler.Text) == 0)
            {

                MessageBox.Show("No se puede agregar al detalle con una cantidad de 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            else if(Convert.ToInt32(txt_tiempo_de_alquiler.Text) > 0)
            {
                int articulosDisponibles = Convert.ToInt32(articuloSeleccionadoRow[4].ToString());
                int horasAlquiladas = Convert.ToInt32(txt_tiempo_de_alquiler.Text);
                char separator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                float costoEnHoras = float.Parse(articuloSeleccionadoRow[5].ToString().Replace(separator, ','));
                float total = horasAlquiladas * costoEnHoras;
                articuloSeleccionadoRow[4] = articulosDisponibles - 1;
                dtg_detalle_alq.Items.Add(new Prestamo
                {
                    IdArticulo = Convert.ToInt32(articuloSeleccionadoRow[0]),
                    Articulo = articuloSeleccionadoRow[1].ToString(),
                    CostoPorHora = costoEnHoras,
                    Tiempo = horasAlquiladas,
                    Total = total
                });
                totalAPagar = totalAPagar+ total;
                txt_valor_estimado_gpre.Text = Math.Round(totalAPagar, 3).ToString("0.00").Replace(',', separator);
                dtg_lista_de_articulos.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = new DataTable() });
                txt_tiempo_de_alquiler.Clear();
                btn_registrar_alquiler_gpre.IsEnabled = true;

            }
        }
        private void limpiar()
        {
            dtg_detalle_alq.Items.Clear();
           
            txt_tiempo_de_alquiler.Clear();
  
            txt_cedula_gpre.Clear();

            txt_valor_estimado_gpre.Clear();
            txt_tiempo_de_alquiler.Clear();
            txt_nombre_de_articulo.Clear();
            txt_estado_de_aportacion_gpre.Clear();
            txt_descuento_apor_gpre.Clear();
            txt_nombre_estudiante_gpre.Clear();
 
       
            articuloSeleccionadoRow = null;
            estudianteActualRow = null;
            idEst = null;
            totalAPagar = 0;
    }

        private void txt_tiempo_de_alquiler_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txt_cedula_gpre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            btn_agregar_detalle_alquiler.IsEnabled = false;
            btn_registrar_alquiler_gpre.IsEnabled = false;
        }
    }
}
