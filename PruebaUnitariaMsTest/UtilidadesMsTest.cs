using Prueba.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaUnitariaMsTest
{
    internal class UtilidadesMsTest
    {
        /// <summary>
        /// Metodo que crea un objeto Asteroide
        /// </summary>
        /// <returns>Retorna un objeto Asteroide</returns>
        public static Asteroide GetAsteroideDeEjemplo()
        {
            Asteroide asteroide = new Asteroide()
            {
                Nombre = "467460 (2006 JF42)",
                Diametro = CalcularDiametroMedio((float)0.407901194, (float)0.912094798),
                Velocidad = (float)99331.0754164726,
                Fecha = DateTime.Parse("2023-05-11"),
                Planeta = "Earth"
            };
            return asteroide;
        }
        /// <summary>
        /// Calcula la media del Diametro
        /// </summary>
        /// <param name="min">Diametro mínimo</param>
        /// <param name="max">Diametro máximo</param>
        /// <returns>Retorna un double que representa la media del diametro minimo y maximo</returns>
        private static double CalcularDiametroMedio(float min, float max)
        {
            float diametroMedio = (max + min) / 2;
            return diametroMedio;
        }

        /// <summary>
        /// Método que crea un objeto ApiModelo de ejemplo
        /// </summary>
        /// <returns>Retorna un objeto ApiModelo</returns>
        public static ApiModel GetApiModeloEjemplo()
        {
            ApiModel model = new ApiModel()
            {
                Name = "467460 (2006 JF42)",
                Estimated_Diameter = new Estimated_Diameter()
                {
                    Kilometers = new Kilometers()
                    {
                        Estimated_diameter_min = (float)0.407901194,
                        Estimated_diameter_max = (float)0.912094798
                    },
                },
                Is_potentially_hazardous_asteroid = true,
                Close_approach_data = new List<Element>()
                {
                    new Element()
                    {
                        Close_approach_date = DateTime.Parse("2023-05-11"),
                        Relative_velocity = new Relative_velocity()
                        {
                            Kilometers_per_second = (float)10,
                            Kilometers_per_hour = (float)99331.0754164726,
                            Miles_per_hour =(float) 22369.5
                        },
                        Orbiting_body = "Earth"
                    }
                }

            };
            return model;
        }
    }
}
