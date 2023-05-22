using System.Threading.Tasks;
using System.Net.Http;

namespace Prueba.Servicios
{
    public interface IPeticionServicio
    {
        
        Task<HttpResponseMessage> RealizarPeticion(int dias, string url, HttpClient httpClient);
    }
}
