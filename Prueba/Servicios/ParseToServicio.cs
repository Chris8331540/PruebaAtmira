using Prueba.Modelos;

namespace Prueba.Servicios
{

    public interface IParseToServicio {
        Asteroide ParseToAsteroide(ApiModel modelo);
    }
    public class ParseToServicio:IParseToServicio
    {

        private static double CalcularDiametroMedio(float min, float max)
        {
            float diametroMedio = (max + min) / 2;
            return diametroMedio;
        }

        public Asteroide ParseToAsteroide(ApiModel modelo)
        {
            Asteroide asteroide = new Asteroide();
            asteroide.Nombre = modelo.Name;
            asteroide.Diametro = CalcularDiametroMedio(modelo.Estimated_Diameter.Kilometers.Estimated_diameter_min,
                modelo.Estimated_Diameter.Kilometers.Estimated_diameter_max);
            asteroide.Velocidad = modelo.Close_approach_data[0].Relative_velocity.Kilometers_per_hour;
            asteroide.Fecha = modelo.Close_approach_data[0].Close_approach_date;
            asteroide.Planeta = modelo.Close_approach_data[0].Orbiting_body;

            return asteroide;
        }
    }
}
