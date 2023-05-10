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

namespace PruebaUnitariaMsTest
{
    [TestClass]
    public class UnitTest1
    {
        private IEnumerable<int> valoresDePrueba;
        private readonly Servicio _servicio;
        private readonly AsteroidsController asteroids;
        public UnitTest1()
        {
            _servicio = new Servicio();
            valoresDePrueba = Enumerable.Range(1, 7);
            asteroids = new AsteroidsController(_servicio);
        }


        [TestMethod]
        //[DynamicData(nameof(valoresDePrueba))]
        [DataRow(1)]
        public async Task ComprobarRealizarPeticion(int dias)
        {
            string json = await _servicio.RealizarPeticion(dias);

            Assert.IsNotNull(json);
        }

        [TestMethod]
        //[DynamicData(nameof(valoresDePrueba))]
        [DataRow(0)]
        public void ComprobarControladorPeticionIncorrecta(int dias) {
            var response = asteroids.Get(dias).Result;
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));
        }
    }

}
