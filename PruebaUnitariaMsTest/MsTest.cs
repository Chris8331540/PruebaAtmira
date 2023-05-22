using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Prueba;
using System.Diagnostics.Eventing.Reader;
using Prueba.Servicios;
using Prueba.Controllers;
using System.Threading.Tasks;
using Prueba.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.Net.Http;
using System.ComponentModel;
using System;

namespace PruebaUnitariaMsTest
{
    [TestClass]
    //tu clase de test unitarios se llama igual que tu clase de test de mock
    public class MsTest
    {
        private readonly PeticionServicio _peticionServicio;
        private readonly ParseToServicio _parseToServicio;
        private readonly AsteroidesServicio _asteroidesServicio;
        private readonly AsteroidsController asteroids;
        private static readonly string _url = "https://api.nasa.gov/neo/rest/v1/feed?";
        private static readonly string _urlFail = "https://api.nasa.gov/neo/rest/v1/";
        private static HttpClient _httpClient;
        public MsTest()
        {
            _peticionServicio = new PeticionServicio();
            _parseToServicio = new ParseToServicio();
            _asteroidesServicio = new AsteroidesServicio(_parseToServicio);
            asteroids = new AsteroidsController(_peticionServicio, _asteroidesServicio);
            _httpClient = new HttpClient();
        }


        /// <summary>
        /// Comprueba la llamada al controlador pasando un dia valido y verifica que recibe una respuesta valida (200)
        /// </summary>
        /// <param name="dias">Cantidad de dias</param>
        /// <returns></returns>
        [TestMethod]
        [DataRow(5)]
        public async Task ComprobarControladorPeticionCorrecta(int dias)
        {
            ContentResult response = (ContentResult)await asteroids.Get(dias);

            Assert.AreEqual(200, (int)response.StatusCode);
        }

        /// <summary>
        /// Comprueba una llamada al controlador pasando un dia invalido y verifica que recibe una respuesta no valida (400)
        /// </summary>
        /// <param name="dias">Cantidad de dias</param>
        /// <returns></returns>
        [TestMethod]
        [DataRow(0)]
        public async Task ComprobarControladorPeticionIncorrecta(int dias)
        {
            ContentResult response = (ContentResult)await asteroids.Get(dias);

            Assert.AreEqual(400, (int)response.StatusCode);
        }

        /// <summary>
        /// Comprueba una llamda a la api de la Nasa y verifica que recibe una respuesta válida (200)
        /// </summary>
        /// <param name="dias">Cantidad de dias</param>
        /// <returns></returns>
        [TestMethod]
        [DataRow(2)]
        public async Task ComprobarPeticionApiNasa(int dias) {
            HttpResponseMessage response = await _peticionServicio.RealizarPeticion(dias, _url, _httpClient);
            Assert.AreEqual(200, (int)response.StatusCode);

        }

        /// <summary>
        /// Comprueba una llamada a la api de la Nasa y verifica que recibe una respuesta no válida (!=200)
        /// </summary>
        /// <param name="dias">Cantidad de dias</param>
        /// <returns></returns>
        [TestMethod]
        [DataRow(2)]
        public async Task ComprobarPeticionApiNasaFail(int dias)
        {
            HttpResponseMessage response = await _peticionServicio.RealizarPeticion(dias, _urlFail, _httpClient);
            Assert.AreNotEqual(200, (int)response.StatusCode);

        }
        /// <summary>
        /// Comprueba que la conversión de un ApiModel de ejemplo a Asteroide concuerda con el Asteroide esperado
        /// </summary>
        [TestMethod]
        public void ComprobarParseAsteriode() { 
            ApiModel model= UtilidadesMsTest.GetApiModeloEjemplo();
            Asteroide asteroide = _parseToServicio.ParseToAsteroide(model);

            Asteroide asteroideEjemplo = UtilidadesMsTest.GetAsteroideDeEjemplo();
            Assert.AreEqual(asteroide.Nombre, asteroideEjemplo.Nombre);
            Assert.AreEqual(asteroide.Velocidad, asteroideEjemplo.Velocidad);
            Assert.AreEqual(asteroide.Diametro, asteroideEjemplo.Diametro);
            Assert.AreEqual(asteroide.Planeta, asteroideEjemplo.Planeta);
            Assert.AreEqual(asteroide.Fecha, asteroideEjemplo.Fecha);
        }
        
    }

}
