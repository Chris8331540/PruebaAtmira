using Prueba.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMock
{
    internal class UtilidadesMock
    {
        /// <summary>
        /// Método que calcula la media de los diametros minimo y maximo
        /// </summary>
        /// <param name="min">Diametro mínimo</param>
        /// <param name="max">Diametro máximo</param>
        /// <returns></returns>
        public static double CalcularDiametroMedio(float min, float max)
        {
            float diametroMedio = (max + min) / 2;
            return diametroMedio;
        }

        /// <summary>
        /// Devuelve una lista de asteroides de prueba, en concordancia con el json de prueba
        /// </summary>
        /// <returns>Lista de Asteroides</returns>
        public static List<Asteroide> GetAsteroidesDePrueba()
        {
            List<Asteroide> asteroides = new List<Asteroide>() {
                new Asteroide(){
                    Nombre = "467460 (2006 JF42)",
                    Diametro = CalcularDiametroMedio((float)0.407901194,(float)0.912094798),
                    Velocidad = 99331.0754164726,
                    Fecha = DateTime.Parse("2023-05-11"),
                    Planeta = "Earth"
                }
            };

            return asteroides;
        }
    }
}
