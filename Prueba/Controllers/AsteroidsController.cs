using Microsoft.AspNetCore.Http;
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
    public class AsteroidsController : ControllerBase
    {

        private readonly IPeticionServicio _peticionServicio;
        private readonly IAsteroidesServicio _asteroidesServicio;
        private HttpClient httpClient;
        private static readonly string _url = "https://api.nasa.gov/neo/rest/v1/feed?";
        public AsteroidsController(IPeticionServicio peticionServicio, IAsteroidesServicio asteroidesServicio)
        {
            _peticionServicio = peticionServicio;
            _asteroidesServicio = asteroidesServicio;
        }
        /// <summary>
        /// Metodo Get de la API
        /// </summary>
        /// <param name="dias">Recibe el número de dias comprendido entre 1 y 7</param>
        /// <returns>Respuesta de la petición y su contenido</returns>
        [HttpGet]
        public async Task<IActionResult> Get(int dias)
        {

            if (dias <= 0 || dias > 7)
            {
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new
                    {
                        code = 400,
                        http_error = "BAD_REQUEST",
                        error_message = "Cantidad de días equivocado"
                    }),
                    ContentType = "application/json",
                    StatusCode = 400
                };
            }

            //recibo el response de la petición a la api de la Nasa
            httpClient = new HttpClient();
            HttpResponseMessage response = await _peticionServicio.RealizarPeticion(dias, _url, httpClient);
            if (response.IsSuccessStatusCode)//si recibo una respuesta válida continuo la operacion normal
            {
                List<Asteroide> asteroides = _asteroidesServicio.GetAsteroides(await response.Content.ReadAsStringAsync());
                string jsonAsteriodes = JsonConvert.SerializeObject(asteroides);
                if (asteroides.Count == 0) {
                    return new ContentResult
                    {
                        Content = jsonAsteriodes,
                        ContentType = "application/json",
                        StatusCode = 204
                    };
                }
                return new ContentResult
                {
                    Content = jsonAsteriodes,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
            }
            else//en caso contrario, devuelvo la respuesta de la api de la Nasa
            {
                //simple y sencillo, me gusta, quizás podrías haber modificado el mensaje devuelto de la API de la nasa y ponerle
                //algún tipo de identificador para que se sepa que lo que ha fallado es la API de la nasa pero bien
                
                return new ContentResult
                {
                    Content = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode,
                    
                };
            }


        }
    }
}
