using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Avion
    {
        public int id { get; set; }
        public string modelo { get; set; }
        public int cantidadPasajeros { get; set; }
        public override string ToString()
        {
            return $"{modelo}, {cantidadPasajeros} pasajeros";
        }
    }
}
