namespace Packing.Mvc.Servicios.Interfaces
{
    public interface IEnviadorCorreos
    {
        void EnviarEmail(string email, string subject, string htmlMessage);
    }
}
