﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Prueba.Modelos;
using Newtonsoft.Json;
using Prueba.Servicios;
using System.Linq;

namespace Prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Asteroids : ControllerBase
    {
        private static HttpClient httpClient = new HttpClient();
        private readonly string _rutaJson = "C:\\Users\\christopher.mendoza\\Downloads\\testJson.json";
        private readonly IServicio _servicio;

        public Asteroids(IServicio servicio)
        {
            _servicio = servicio;
        }
        [HttpGet("{dias:int}")]
        public async Task<IActionResult> Get(int dias) {

            if (dias <= 0 && dias > 7) {
                return BadRequest("Cantidad de días equivocado");
            }
            //DateTime hoy  = DateTime.Now;
            //DateTime fechaFinal = hoy.AddDays(dias);
            //string fechaHoyString = hoy.ToString("yyyy-MM-dd");
            //string fechaFinalString = fechaFinal.ToString("yyyy-MM-dd");
            //string url = $"https://api.nasa.gov/neo/rest/v1/feed?start_date={fechaHoyString}&end_date={fechaFinalString}&api_key=DEMO_KEY";

            //HttpResponseMessage response = await httpClient.GetAsync(url);

            //string json = await response.Content.ReadAsStringAsync();
            string json = System.IO.File.ReadAllText(_rutaJson);

            List<Asteroide> asteriodes = _servicio.GetAsteroides(json);
            

            string jsonAsteriodes = JsonConvert.SerializeObject(asteriodes.OrderBy(x=>x.Diametro).Take(3).ToList());


            return Ok(jsonAsteriodes);

        }
    }
}