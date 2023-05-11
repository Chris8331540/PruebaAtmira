using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prueba.Servicios;
using Prueba.Controllers;
using Prueba.Modelos;
using Moq;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using Castle.Components.DictionaryAdapter;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;

namespace TestMock
{
    [TestClass]
    public class UnitTest1
    {
        private readonly string _rutaJson = "C:\\Users\\christopher.mendoza\\Downloads\\testJson.json";
        private readonly string _rutaResultado = "C:\\Users\\christopher.mendoza\\Downloads\\resultado.json";//3 dias

        [TestMethod]
        public async Task PruebaAsteroidController()
        {

            int dias = 7;
            string archivo = File.ReadAllText(_rutaJson);
            Mock<IPeticionServicio> peticionServicio = new Mock<IPeticionServicio>();
            Mock<IAsteroidesServicio> asteroidesServicio = new Mock<IAsteroidesServicio>();
            Mock<IParseToServicio> parseToServicio = new Mock<IParseToServicio>();

            peticionServicio.Setup(s => s.RealizarPeticion(It.IsAny<int>())).ReturnsAsync(archivo);

            parseToServicio
            .Setup(p => p.ParseToAsteroide(It.IsAny<ApiModel>()))
            .Returns((ApiModel modelo) => new Asteroide
            {
                Nombre = modelo.Name,
                Diametro = CalcularDiametroMedio(modelo.Estimated_Diameter.Kilometers.Estimated_diameter_min,
                    modelo.Estimated_Diameter.Kilometers.Estimated_diameter_max),
                Velocidad = modelo.Close_approach_data[0].Relative_velocity.Kilometers_per_hour,
                Fecha = modelo.Close_approach_data[0].Close_approach_date,
                Planeta = modelo.Close_approach_data[0].Orbiting_body
            });
            
            asteroidesServicio.Setup(a => a.GetAsteroides(It.IsAny<string>())).Returns(GetAsteroidesDePrueba(archivo));

            AsteroidsController asteriods = new AsteroidsController(peticionServicio.Object, asteroidesServicio.Object);
            var response = await asteriods.Get(dias);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            peticionServicio.Verify();
            parseToServicio.Verify();
            asteroidesServicio.Verify();

        }

        private static double CalcularDiametroMedio(float min, float max)
        {
            float diametroMedio = (max + min) / 2;
            return diametroMedio;
        }

        List<Asteroide> GetAsteroidesDePrueba(string json)
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
                            var asteroidePrueba = new Asteroide
                            {
                                Nombre = elementoObject.Name,
                                Diametro = CalcularDiametroMedio(elementoObject.Estimated_Diameter.Kilometers.Estimated_diameter_min,
                    elementoObject.Estimated_Diameter.Kilometers.Estimated_diameter_max),
                                Velocidad = elementoObject.Close_approach_data[0].Relative_velocity.Kilometers_per_hour,
                                Fecha = elementoObject.Close_approach_data[0].Close_approach_date,
                                Planeta = elementoObject.Close_approach_data[0].Orbiting_body
                            };

                            asteroides.Add(asteroidePrueba);
                        }
                    }
                }
            }


            return asteroides;
        }
    }
}
