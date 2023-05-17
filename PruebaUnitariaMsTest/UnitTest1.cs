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
    //TODO: parece tontería pero el nombre de las clases debe de ser identificativo
    //tu clase de test unitarios se llama igual que tu clase de test de mock
    public class UnitTest1
    {
        private readonly PeticionServicio _peticionServicio;
        private readonly ParseToServicio _parseToServicio;
        private readonly AsteroidesServicio _asteroidesServicio;
        private readonly AsteroidsController asteroids;
        private static readonly string _url = "https://api.nasa.gov/neo/rest/v1/feed?";
        private static readonly string _urlFail = "https://api.nasa.gov/neo/rest/v1/";
        public UnitTest1()
        {
            _peticionServicio = new PeticionServicio();
            _parseToServicio = new ParseToServicio();
            _asteroidesServicio = new AsteroidesServicio(_parseToServicio);
            asteroids = new AsteroidsController(_peticionServicio, _asteroidesServicio);
        }


        /// <summary>
        /// Comprueba la llamada al controlador pasando un dia valido y verifica que recibe una respuesta valida (200)
        /// </summary>
        /// <param name="dias">Cantidad de dias</param>
        /// <returns></returns>
        [TestMethod]
        [DataRow(1)]
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
            HttpResponseMessage response = await _peticionServicio.RealizarPeticion(dias, _url);
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
            HttpResponseMessage response = await _peticionServicio.RealizarPeticion(dias, _urlFail);
            Assert.AreNotEqual(200, (int)response.StatusCode);

        }
        /// <summary>
        /// Comprueba que la conversión de un ApiModel de ejemplo a Asteroide concuerda con el Asteroide esperado
        /// </summary>
        [TestMethod]
        public void ComprobarParseAsteriode() { 
            ApiModel model= GetApiModeloEjemplo();
            Asteroide asteroide = _parseToServicio.ParseToAsteroide(model);

            Asteroide asteroideEjemplo = GetAsteroideDeEjemplo();
            Assert.AreEqual(asteroide.Nombre, asteroideEjemplo.Nombre);
            Assert.AreEqual(asteroide.Velocidad, asteroideEjemplo.Velocidad);
            Assert.AreEqual(asteroide.Diametro, asteroideEjemplo.Diametro);
            Assert.AreEqual(asteroide.Planeta, asteroideEjemplo.Planeta);
            Assert.AreEqual(asteroide.Fecha, asteroideEjemplo.Fecha);
        }
        //TODO: Todo lo que no sea un TEST, no debe ir en una clase/archivo de tests, esta lógica está bien pero extráela
        /// <summary>
        /// Metodo que crea un objeto Asteroide
        /// </summary>
        /// <returns>Retorna un objeto Asteroide</returns>
        private Asteroide GetAsteroideDeEjemplo() {
            Asteroide asteroide = new Asteroide()
            {
                Nombre = "467460 (2006 JF42)",
                Diametro = CalcularDiametroMedio((float)0.407901194, (float)0.912094798),
                Velocidad = (float)99331.0754164726,
                Fecha = DateTime.Parse("2023-05-11"),
                Planeta = "Earth"
            };
            return asteroide;
        }
        /// <summary>
        /// Calcula la media del Diametro
        /// </summary>
        /// <param name="min">Diametro mínimo</param>
        /// <param name="max">Diametro máximo</param>
        /// <returns>Retorna un double que representa la media del diametro minimo y maximo</returns>
        private static double CalcularDiametroMedio(float min, float max)
        {
            float diametroMedio = (max + min) / 2;
            return diametroMedio;
        }

        /// <summary>
        /// Método que crea un objeto ApiModelo de ejemplo
        /// </summary>
        /// <returns>Retorna un objeto ApiModelo</returns>
        private ApiModel GetApiModeloEjemplo()
        {
            ApiModel model = new ApiModel()
            {
                Name = "467460 (2006 JF42)",
                Estimated_Diameter = new Estimated_Diameter()
                {
                    Kilometers = new Kilometers()
                    {
                        Estimated_diameter_min = (float)0.407901194,
                        Estimated_diameter_max = (float)0.912094798
                    },
                },
                Is_potentially_hazardous_asteroid = true,
                Close_approach_data = new List<Element>()
                {
                    new Element()
                    {
                        Close_approach_date = DateTime.Parse("2023-05-11"),
                        Relative_velocity = new Relative_velocity()
                        {
                            Kilometers_per_second = (float)10,
                            Kilometers_per_hour = (float)99331.0754164726,
                            Miles_per_hour =(float) 22369.5
                        },
                        Orbiting_body = "Earth"
                    }
                }

            };
            return model;
        }
    }

}
