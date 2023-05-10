using Newtonsoft.Json.Linq;
using Prueba.Modelos;
using System.Collections;
using Prueba.Modelos;
using System.Collections.Generic;
using System;
using System.Data.SqlTypes;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Linq;

namespace Prueba.Servicios
{

    public interface IServicio
    {
        List<Asteroide> GetAsteroides(string json);
        Task<string> RealizarPeticion(int dias);
    }
    public class Servicio : IServicio
    {
        private static HttpClient httpClient = new HttpClient();
        private readonly string _rutaJson = "C:\\Users\\christopher.mendoza\\Downloads\\testJson.json";

        public List<Asteroide> GetAsteroides(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            //obtengo una asteroides de las fechas con su respectiva informacion
            var listaFechas = jsonObject["near_earth_objects"];
            List<Asteroide> asteroides = new List<Asteroide>();

            if (listaFechas != null)
            {
                var valoresListaFecha = listaFechas.Values();
                foreach (var elementoFecha in valoresListaFecha)
                {
                    //var elemento = elementoFecha.Values();
                    var elementosChildren = elementoFecha.Children();
                    foreach (var elemento in elementosChildren)
                    {
                        var elementoObject = elemento.ToObject<ApiModel>();
                        if (elementoObject.Is_potentially_hazardous_asteroid)
                        {
                            Asteroide asteroide = ParseToAsteroide(elementoObject);
                            asteroides.Add(asteroide);
                        }
                    }
                }
            }


            return asteroides;
        }

        public async Task<string> RealizarPeticion(int dias)
        {
            DateTime hoy = DateTime.Now;
            DateTime fechaFinal = hoy.AddDays(dias);
            string start_date = hoy.ToString("yyyy-MM-dd");
            string end_date = fechaFinal.ToString("yyyy-MM-dd");
            string url = $"https://api.nasa.gov/neo/rest/v1/feed?start_date={start_date}&end_date={end_date}&api_key=DEMO_KEY";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            string json = await response.Content.ReadAsStringAsync();
            return json;
        }

        private static double CalcularDiametroMedio(float min, float max)
        {
            float diametroMedio = (max + min) / 2;
            return diametroMedio;
        }

        private static Asteroide ParseToAsteroide(ApiModel modelo)
        {
            Asteroide asteroide = new Asteroide();
            asteroide.Nombre = modelo.Name;
            asteroide.Diametro = CalcularDiametroMedio(modelo.Estimated_Diameter.Kilometers.Estimated_diameter_min,
                modelo.Estimated_Diameter.Kilometers.Estimated_diameter_max);
            asteroide.Velocidad = modelo.Close_approach_data[0].Relative_velocity.Kilometers_per_hour;
            asteroide.Fecha = modelo.Close_approach_data[0].Close_approach_date;
            asteroide.Planeta = modelo.Close_approach_data[0].Orbiting_body;

            return asteroide;
        }
    }
}
