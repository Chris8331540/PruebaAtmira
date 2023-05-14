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

        /// <summary>
        /// Recibe como parámetro la respuesta de la api de la Nasa, recoge los objetos de la key "near_earth_objects"
        /// gracias a apiModel y los transforma a objetos Asteroides los cuales son almacenados en una lista y son devueltos.
        /// </summary>
        /// <param name="json">Es el json de respuesta de la api de la Nasa</param>
        /// <returns>Devuelve una lista de Asteroides</returns>
        public List<Asteroide> GetAsteroides(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            
            var listaFechas = jsonObject["near_earth_objects"];
            List<Asteroide> asteroides = new List<Asteroide>();

            if (listaFechas != null)
            {
                var valoresListaFecha = listaFechas.Values();
                foreach (var elementoFecha in valoresListaFecha)
                {
                    var elementosChildren = elementoFecha.Children();
                    foreach (var elemento in elementosChildren)
                    {
                        //transformo cada elemento (información de un asteroide) a apiModel que representa lo que se recibe de la nasa.
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
