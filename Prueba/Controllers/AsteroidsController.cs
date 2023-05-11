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
        [HttpGet]
        public async Task<IActionResult> Get(int dias)
        {

            if (dias <= 0 || dias > 7)
            {
                return BadRequest("Cantidad de días equivocado");
            }

            string json = await _peticionServicio.RealizarPeticion(dias);
            List<Asteroide> asteriodes = _asteroidesServicio.GetAsteroides(json);
            string jsonAsteriodes = JsonConvert.SerializeObject(asteriodes.OrderBy(x => x.Diametro).Take(3).ToList());

            return Ok(jsonAsteriodes);

        }
    }
}
