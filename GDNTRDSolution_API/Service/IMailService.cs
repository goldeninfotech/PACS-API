using GDNTRDSolution_API.Models;

namespace GDNTRDSolution_API.Service
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
        bool SendMailAsync(MailData mailData);
    }
}
