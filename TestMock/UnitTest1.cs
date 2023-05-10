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

namespace TestMock
{
    [TestClass]
    public class UnitTest1
    {
        private readonly string _rutaJson = "C:\\Users\\christopher.mendoza\\Downloads\\testJson.json";
        private readonly string _rutaResultado = "C:\\Users\\christopher.mendoza\\Downloads\\resultado.json";//3 dias

        [TestMethod]
        public async Task TestMethod1()
        {

            int dias = 3;
            string archivo = File.ReadAllText(_rutaJson);
            Mock<IServicio> servicios = new Mock<IServicio>();
            servicios.Setup(s => s.RealizarPeticion(dias)).Returns(Task.FromResult(archivo));
            servicios.Setup(s => s.GetAsteroides(archivo)).Returns(GetAsteroidesDePrueba());

            AsteroidsController asteriods = new AsteroidsController(servicios.Object);
            var response = await asteriods.Get(dias);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        }

        private List<Asteroide> GetAsteroidesDePrueba() {
            List<Asteroide> asteroides  = new List<Asteroide>();
            string archivo = File.ReadAllText(_rutaResultado);
            JArray jsonArray = JArray.Parse(archivo);
            foreach(var asteroide in jsonArray) {
                var elementoObject = asteroide.ToObject<Asteroide>();
                asteroides.Add(elementoObject);
            }
            return asteroides;

        }
    }
}
