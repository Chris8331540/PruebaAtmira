using Prueba.Modelos;
using System.Collections.Generic;

namespace Prueba.Servicios
{
    public interface IAsteroidesServicio {
        List<Asteroide> GetAsteroides(string json);
    }
}
