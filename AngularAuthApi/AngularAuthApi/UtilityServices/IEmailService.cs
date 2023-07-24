using AngularAuthApi.Models;

namespace AngularAuthApi.UtilityServices
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
