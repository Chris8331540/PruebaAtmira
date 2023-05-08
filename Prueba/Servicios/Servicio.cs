using Newtonsoft.Json.Linq;
using Prueba.Modelos;
using System.Collections;
using Prueba.Modelos;
using System.Collections.Generic;
using System;
using System.Data.SqlTypes;
using Newtonsoft.Json;

namespace Prueba.Servicios
{

    public interface IServicio
    {
        List<Asteroide> GetAsteroides(string json);
    }
    public class Servicio : IServicio
    {


        public List<Asteroide> GetAsteroides(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            //obtengo una asteroides de las fechas con su respectiva informacion
            var listaFechas = jsonObject["near_earth_objects"];
            List<Asteroide> asteroides = new List<Asteroide>();
            var valoresListaFecha = listaFechas.Values();

            foreach (var elementoFecha in valoresListaFecha)
            {
                //var elemento = elementoFecha.Values();
                var elementosChildren = elementoFecha.Children();
                foreach(var elemento in elementosChildren) {
                    var elementoObject = elemento.ToObject<ApiModel>();
                    if (elementoObject.Is_potentially_hazardous_asteroid)
                    {
                        Asteroide asteroide = ParseToAsteroide(elementoObject);
                        asteroides.Add(asteroide);
                    }
                }
            }

            return asteroides;
        }

        private static double CalcularDiametro(float min, float max)
        {
            float diametroMedio = max - min;
            return diametroMedio;
        }

        private static Asteroide ParseToAsteroide(ApiModel modelo) {
            Asteroide asteroide = new Asteroide();
            asteroide.Nombre = modelo.Name;
            asteroide.Diametro = CalcularDiametro(modelo.Estimated_Diameter.Kilometers.Estimated_diameter_min,
                modelo.Estimated_Diameter.Kilometers.Estimated_diameter_max);
            asteroide.Velocidad = modelo.Close_approach_data[0].Relative_velocity.Kilometers_per_hour;
            asteroide.Fecha = modelo.Close_approach_data[0].Close_approach_date;
            asteroide.Planeta = modelo.Close_approach_data[0].Orbiting_body;

            return asteroide;
        }
    }
}
