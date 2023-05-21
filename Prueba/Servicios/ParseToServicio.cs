using Prueba.Modelos;

namespace Prueba.Servicios
{
    public class ParseToServicio:IParseToServicio
    {
        /// <summary>
        /// Calcula el diametro medio que hay entre un diametro minimo y máximo
        /// </summary>
        /// <param name="min">Diámetro mínimo</param>
        /// <param name="max">Diámetro máximo</param>
        /// <returns>El resultado de calcular el diametro medio.</returns>
        private static double CalcularDiametroMedio(float min, float max)
        {
            float diametroMedio = (max + min) / 2;
            return diametroMedio;
        }

        /// <summary>
        /// Tranforma un objeto apiModel a Asteroide
        /// </summary>
        /// <param name="modelo">objeto apiModel</param>
        /// <returns>Devuelve un objeto Asteroide</returns>
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
