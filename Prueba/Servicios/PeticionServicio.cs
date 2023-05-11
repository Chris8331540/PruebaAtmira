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

    public interface IPeticionServicio
    {
        
        Task<string> RealizarPeticion(int dias);
    }
    public class PeticionServicio : IPeticionServicio
    {
        private static HttpClient httpClient = new HttpClient();
        private readonly string _rutaJson = "C:\\Users\\christopher.mendoza\\Downloads\\testJson.json";

        

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

        
    }
}
