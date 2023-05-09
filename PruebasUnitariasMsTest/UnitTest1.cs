using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
namespace PruebasUnitariasMsTest
{
    [TestClass]
    public class UnitTest1
    {
        private IEnumerable<int> valoresDePrueba;
        public UnitTest1()
        {
            valoresDePrueba = Enumerable.Range(1, 7);
        }


        [DataTestMethod]
        [DynamicData(nameof(valoresDePrueba))]

        public void ComprobarDias(int dias)
        {

        }
    }
}
