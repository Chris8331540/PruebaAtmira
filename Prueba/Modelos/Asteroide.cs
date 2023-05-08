using System;
using System.Reflection.Metadata.Ecma335;

namespace Prueba.Modelos
{
    public class Asteroide
    {
        public string Nombre { get; set; }
        public double Diametro { get; set; }
        public double Velocidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Planeta { get; set; }
    }
}
