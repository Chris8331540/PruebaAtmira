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
    public class UnitTest1
    {
        //private readonly string _rutaJson = "C:\\Users\\christopher.mendoza\\Downloads\\testJson.json";
        private readonly string _rutaJson = "../../../../Prueba/ArchivoJson/testJson2.json";//1 dia
        Mock<IPeticionServicio> peticionServicio;
        Mock<IAsteroidesServicio> asteroidesServicio;
        Mock<IParseToServicio> parseToServicio = new Mock<IParseToServicio>();
        private static readonly string _url = "https://api.nasa.gov/neo/rest/v1/feed?";
        public UnitTest1()
        {
            peticionServicio = new Mock<IPeticionServicio>();
            asteroidesServicio = new Mock<IAsteroidesServicio>();
            parseToServicio = new Mock<IParseToServicio>();
        }

        /// <summary>
        /// Simula una llamada a mi controlador, con una cantidad de d�as no v�lida
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
        /// Simula la llamada al controlador, pasando un valor v�lido
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
            
            asteroidesServicio.Setup(a => a.GetAsteroides(It.IsAny<string>())).Returns(GetAsteroidesDePrueba());

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

        /// <summary>
        /// M�todo que calcula la media de los diametros minimo y maximo
        /// </summary>
        /// <param name="min">Diametro m�nimo</param>
        /// <param name="max">Diametro m�ximo</param>
        /// <returns></returns>
        private static double CalcularDiametroMedio(float min, float max)
        {
            float diametroMedio = (max + min) / 2;
            return diametroMedio;
        }

        /// <summary>
        /// Devuelve una lista de asteroides de prueba, en concordancia con el json de prueba
        /// </summary>
        /// <returns>Lista de Asteroides</returns>
        private List<Asteroide> GetAsteroidesDePrueba()
        {
            List<Asteroide> asteroides = new List<Asteroide>() { 
                new Asteroide(){ 
                    Nombre = "467460 (2006 JF42)",
                    Diametro = CalcularDiametroMedio((float)0.407901194,(float)0.912094798),
                    Velocidad = 99331.0754164726,
                    Fecha = DateTime.Parse("2023-05-11"),
                    Planeta = "Earth"
                }
            };

            return asteroides;
        }
    }
}
