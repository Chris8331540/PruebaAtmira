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
            HttpResponseMessage response = await _peticionServicio.RealizarPeticion(dias);
            if (response.IsSuccessStatusCode)//si recibo una respuesta válida continuo la operacion normal
            {
                List<Asteroide> asteriodes = _asteroidesServicio.GetAsteroides(await response.Content.ReadAsStringAsync());
                string jsonAsteriodes = JsonConvert.SerializeObject(asteriodes.OrderByDescending(x => x.Diametro).Take(3).ToList());

                return new ContentResult
                {
                    Content = jsonAsteriodes,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
            }
            else//en caso contrario, devuelvo la respuesta de la api de la Nasa
            {
                return new ContentResult
                {
                    Content = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode,
                };
            }


        }
    }
}
