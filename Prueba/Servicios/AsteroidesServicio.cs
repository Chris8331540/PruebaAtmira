using Newtonsoft.Json.Linq;
using Prueba.Modelos;
using System.Collections.Generic;

namespace Prueba.Servicios
{
    public interface IAsteroidesServicio {
        List<Asteroide> GetAsteroides(string json);
    }
    public class AsteroidesServicio:IAsteroidesServicio
    {
        private readonly IParseToServicio _parseToServicio;
        public AsteroidesServicio(IParseToServicio parseToServicio)
        {

            _parseToServicio = parseToServicio;

        }
        public List<Asteroide> GetAsteroides(string json)
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
                            Asteroide asteroide = _parseToServicio.ParseToAsteroide(elementoObject);
                            asteroides.Add(asteroide);
                        }
                    }
                }
            }


            return asteroides;
        }
    }
}
