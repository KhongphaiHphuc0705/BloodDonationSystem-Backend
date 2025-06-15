using Application.DTO.SendEmailDTO;
using Domain.Entities;
using Infrastructure.Repository.Events;
using Infrastructure.Repository.Facilities;
using Infrastructure.Repository.UserRepo;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.EmailServ
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IUserRepository _repoUser;
        private readonly IEventRepository _repoEvent;
        private readonly IFacilityRepository _repoFacil;

        public EmailService(IOptions<EmailSettings> settings, IUserRepository repoUser,
            IEventRepository repoEvent, IFacilityRepository repoFacil)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings), "Email settings cannot be null.");
            _repoUser = repoUser;
            _repoEvent = repoEvent;
            _repoFacil = repoFacil;
        } 

        public async Task SendEmailBloodCollectionAsync(BloodRegistration bloodRegistration)
        {
            var (member, eventObj, facility) = await GetNecessariesForSendEmail(bloodRegistration);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Trung tâm Hiến Máu Nhân Đạo Ép Pê Tê", _settings.From));
            message.To.Add(MailboxAddress.Parse(member.Gmail));
            message.Subject = $"Cảm ơn bạn đã tham gia hiến máu tại sự kiện {eventObj.Title}";

            var builder = new BodyBuilder();

            builder.HtmlBody = $@"
    <html>
    <body style='font-family: Arial, sans-serif; color: #333;'>
        <p>Xin chào <strong>{member.FirstName}</strong>,</p>

        <p>
            Chúng tôi xin gửi lời cảm ơn chân thành vì bạn đã tham gia hiến máu tại sự kiện <strong>{eventObj.Title}</strong> được tổ chức tại địa điểm <strong>{facility.Name}</strong>.
            Những giọt máu của bạn sẽ giúp cứu sống nhiều người đang cần được truyền máu – Bạn chính là người hùng của chúng tôi!
        </p>

        <h3>🩸 Một vài lưu ý nhỏ để phục hồi sau hiến máu:</h3>
        <ul>
            <li>Uống nhiều nước trong 24 giờ tới.</li>
            <li>Tránh hoạt động thể chất nặng trong ngày hôm nay.</li>
            <li>Ăn đầy đủ, đặc biệt là thực phẩm giàu sắt như thịt đỏ, rau lá xanh,…</li>
            <li>Nếu cảm thấy chóng mặt, hãy nằm nghỉ và nâng cao chân.</li>
        </ul>

        <h3>📅 Khi nào bạn có thể hiến máu lần tiếp theo?</h3>
        <p>
            Bạn có thể hiến máu lần tiếp theo sau <strong>{ (member.Gender == true ? "12 tuần" : "16 tuần") }</strong> kể từ ngày hôm nay.
        </p>
        <p>
            Một lần nữa, cảm ơn bạn đã góp phần lan toả sự sống.
        </p>

        <p>
            Nếu bạn có bất kỳ câu hỏi nào, đừng ngần ngại liên hệ với chúng tôi qua <strong>{_settings.From}</strong>.
        </p>

        <p>
            Trân trọng,<br>
            <strong>{facility.Name}</strong>
        </p>
    </body>
    </html>";

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private async Task<(User member, Event eventObj, Facility facility)> GetNecessariesForSendEmail(BloodRegistration bloodRegistration)
        {
            var member = await _repoUser.GetByIdAsync(bloodRegistration.MemberId);
            var eventObj = await _repoEvent.GetByIdAsync(bloodRegistration.EventId);
            var facility = await _repoFacil.GetByIdAsync(eventObj.FacilityId);

            return (member, eventObj, facility);
        }
    }
}
