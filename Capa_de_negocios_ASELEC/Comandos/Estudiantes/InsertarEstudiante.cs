using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_de_negocios_ASELEC.Comandos
{
    public class InsertarEstudiante: Comando
    {
        private CN_Estudiante objetoCN = new CN_Estudiante();
        private int idTipoAportacion;
        private string nombreCliente;
        private string cedula;
        private int idCarrera;
        private string correo;
        private string telefono;
        private string valordeaportacion;

        public InsertarEstudiante(CN_Estudiante objetoCN, int idTipoAportacion, string nombreCliente, string cedula, int idCarrera, string correo, string telefono, string valordeaportacion) 
        {
            this.idTipoAportacion = idTipoAportacion;
            this.nombreCliente = nombreCliente;
            this.cedula = cedula;
            this.idCarrera = idCarrera;
            this.correo = correo;
            this.telefono = telefono;
            this.valordeaportacion = valordeaportacion;
        }

        public void ejecutar()
        {
            objetoCN.Insertar(idTipoAportacion, nombreCliente, cedula, idCarrera, correo, telefono, valordeaportacion);
        }
    }
}
