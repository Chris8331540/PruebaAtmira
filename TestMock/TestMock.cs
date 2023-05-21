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
using System.Net.Http;
using System.Net;

namespace TestMock
{
    [TestClass]
    public class TestMock
    {
        private readonly string _rutaJson = "../../../../Prueba/ArchivoJson/testJson2.json";//1 dia
        Mock<IPeticionServicio> peticionServicio;
        Mock<IAsteroidesServicio> asteroidesServicio;
        Mock<IParseToServicio> parseToServicio;
        Mock<HttpClient> httpClient;
        private static readonly string _url = "https://api.nasa.gov/neo/rest/v1/feed?";

        //TODO: Está bien, pero falta mockear el httpClient del servicio para ver como funciona ese servicio con json falso de la nasa
        public TestMock()
        {
            peticionServicio = new Mock<IPeticionServicio>();
            asteroidesServicio = new Mock<IAsteroidesServicio>();
            parseToServicio = new Mock<IParseToServicio>();
            httpClient = new Mock<HttpClient>();
        }

        /// <summary>
        /// Simula una llamada a mi controlador, con una cantidad de días no válida
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PruebaAsteroideControllerFailDias() {
            int dias = 10;

            AsteroidsController asteriods = new AsteroidsController(peticionServicio.Object, asteroidesServicio.Object);
            var response = await asteriods.Get(dias);

            Assert.IsInstanceOfType(response, typeof(ContentResult));


            var result = (ContentResult)response;
            var statusCode = result.StatusCode;
            Assert.AreNotEqual(200, statusCode);
        }

        /// <summary>
        /// Simula la llamada del metodo Realizar petición, que devuelve una respuesta Ok con el json falso de la nasa
        /// </summary>
        /// <returns></returns>
        //[TestMethod]
        //public async Task PruebaPeticionServicioHttpClient() {
        //    string archivo = File.ReadAllText(_rutaJson);
        //    int dias = 2;
        //    httpClient.Setup(s=>s.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage()
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Content = new StringContent(archivo)
        //    });

        //    var servicio = new PeticionServicio();
        //    servicio.GetType().GetField("httpClient", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(servicio, httpClient.Object);

        //    var response = await servicio.RealizarPeticion(dias, _url);
        //    httpClient.Verify();
        //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        //}

        /// <summary>
        /// Simula una llamada a la Api de la Nasa y esta retorna un StatusCode != 200
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PruebaAsteroideControllerFailApiNasa() {
            int dias = 7;
            string archivo = File.ReadAllText(_rutaJson);
            peticionServicio.Setup(s => s.RealizarPeticion(It.IsAny<int>(), _url)).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    code = 400,
                    http_error = "BAD_REQUEST",
                    error_message = "Error X"
                })),
            });

            AsteroidsController asteriods = new AsteroidsController(peticionServicio.Object, asteroidesServicio.Object);
            var response = await asteriods.Get(dias);

            Assert.IsInstanceOfType(response, typeof(ContentResult));


            var result = (ContentResult)response;
            var statusCode = result.StatusCode;
            Assert.AreNotEqual(200, statusCode);
            peticionServicio.Verify();

        }

        /// <summary>
        /// Simula la llamada al controlador, pasando un valor válido
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PruebaAsteroidController()
        {

            int dias = 7;
            string archivo = File.ReadAllText(_rutaJson);

            peticionServicio.Setup(s => s.RealizarPeticion(It.IsAny<int>(), _url)).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(archivo)
            });

            parseToServicio.Setup(p => p.ParseToAsteroide(It.IsAny<ApiModel>())).Returns((ApiModel modelo) => new Asteroide
            {
                Nombre = modelo.Name,
                Diametro = UtilidadesMock.CalcularDiametroMedio(modelo.Estimated_Diameter.Kilometers.Estimated_diameter_min,
                    modelo.Estimated_Diameter.Kilometers.Estimated_diameter_max),
                Velocidad = modelo.Close_approach_data[0].Relative_velocity.Kilometers_per_hour,
                Fecha = modelo.Close_approach_data[0].Close_approach_date,
                Planeta = modelo.Close_approach_data[0].Orbiting_body
            });
            
            asteroidesServicio.Setup(a => a.GetAsteroides(It.IsAny<string>())).Returns(UtilidadesMock.GetAsteroidesDePrueba());

            AsteroidsController asteriods = new AsteroidsController(peticionServicio.Object, asteroidesServicio.Object);
            var response = await asteriods.Get(dias);

            Assert.IsInstanceOfType(response, typeof(ContentResult));


            var result = (ContentResult)response;
            var statusCode = result.StatusCode;
            Assert.AreEqual(200, statusCode);

            peticionServicio.Verify();
            parseToServicio.Verify();
            asteroidesServicio.Verify();

        }

        
    }
}
