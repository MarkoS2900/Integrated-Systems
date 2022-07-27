using Cinema.Domain.DomainModels;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Services.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly IRepository<EmailMessage> _emailRepository;
        private readonly IEmailService _emailService;

        public BackgroundEmailSender(IRepository<EmailMessage> emailRepository, IEmailService emailService)
        {
            _emailRepository = emailRepository;
            _emailService = emailService;
        }

        public async Task DoWork()
        {
            await _emailService.SendEmailAsync(_emailRepository.GetAll().Where(z => !z.status).ToList());
        }
    }
}
