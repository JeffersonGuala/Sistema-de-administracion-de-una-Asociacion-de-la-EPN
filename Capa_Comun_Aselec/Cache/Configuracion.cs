using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Comun_Aselec.Cache
{
    public class Configuracion
    {
        private static Configuracion instancia = null;
        public string SemestreActual { get; set; }

        public Configuracion()
        {
            instancia = this;
        }
        public static Configuracion GetInstancia()
        {
            if (instancia == null)
            {
                instancia = new Configuracion();
            }
            return instancia;
        }
    }
}
