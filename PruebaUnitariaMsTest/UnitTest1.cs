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
    public class UnitTest1
    {
        private IEnumerable<int> valoresDePrueba;
        private readonly PeticionServicio _peticionServicio;
        private readonly ParseToServicio _parseToServicio;
        private readonly AsteroidesServicio _asteroidesServicio;
        private readonly AsteroidsController asteroids;
        public UnitTest1()
        {
            _peticionServicio = new PeticionServicio();
            _parseToServicio = new ParseToServicio();
            _asteroidesServicio = new AsteroidesServicio(_parseToServicio);
            valoresDePrueba = Enumerable.Range(1, 7);
            asteroids = new AsteroidsController(_peticionServicio, _asteroidesServicio);
        }


        [TestMethod]
        //[DynamicData(nameof(valoresDePrueba))]
        [DataRow(1)]
        public async Task ComprobarControladorPeticionCorrecta(int dias)
        {
            ContentResult response = (ContentResult)await asteroids.Get(dias);

            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        //[DynamicData(nameof(valoresDePrueba))]
        [DataRow(0)]
        public async Task ComprobarControladorPeticionIncorrecta(int dias)
        {
            ContentResult response = (ContentResult)await asteroids.Get(dias);

            Assert.AreEqual(400, (int)response.StatusCode);
        }

        [TestMethod]
        [DataRow(2)]
        public async Task ComprobarPeticionApiNasa(int dias) {
            HttpResponseMessage response = await _peticionServicio.RealizarPeticion(dias);
            Assert.AreEqual(200, (int)response.StatusCode);

        }

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

        private static double CalcularDiametroMedio(float min, float max)
        {
            float diametroMedio = (max + min) / 2;
            return diametroMedio;
        }

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
