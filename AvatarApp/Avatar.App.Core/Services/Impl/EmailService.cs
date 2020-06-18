using System.Threading.Tasks;
using Avatar.App.SharedKernel.Settings;
using Avatar.App.Core.Constants;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using Avatar.App.SharedKernel.Interfaces;
using Avatar.App.Core.Entities;
using System.Linq;

namespace Avatar.App.Core.Services.Impl
{
    public class EmailService : BaseServiceWithAuthorization, IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly AvatarAppSettings _avatarAppSettings;
        private readonly EmailSettings _emailSettings;
        private readonly GeneralEmailSettings _generalEmailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings, 
            IOptions<GeneralEmailSettings> generalEmailSettings, IOptions<AvatarAppSettings> avatarAppOptions, 
            SmtpClient smtpClient, IRepository<User> userRepository) : base(userRepository)
        {
            _smtpClient = smtpClient;
            _avatarAppSettings = avatarAppOptions.Value;
            _emailSettings = emailSettings.Value;
            _generalEmailSettings = generalEmailSettings.Value;
        }

        public async Task SendConfirmCodeAsync(string email, string confirmCode, string guid)
        {
            var emailBody = CreateConfirmEmailBody(guid, confirmCode);

            var emailMessage = CreateEmailMessage(email, EmailMessages.ConfirmSubject, emailBody);

            await SendMessageAsync(emailMessage);
        }

        public async Task SendPasswordResetAsync(string email, string confirmCode, string guid)
        {
            var emailBody = CreatePasswordResetBody(guid, confirmCode);

            var emailMessage = CreateEmailMessage(email, EmailMessages.ResetPasswordSubject, emailBody);

            await SendMessageAsync(emailMessage);
        }

        public async Task SendGeneralEmailMessage(string subject, string text)
        {
            var allUsers = UserRepository.List();

            var emails = allUsers.Where(user => user.ConsentToGeneralEmail == true).Select(user => user.Email);

            var message = CreateGeneralEmailMessage(emails, subject, CreateGeneralMessageBody(text));

            await SendGeneralMessageAsync(message);
        }

        #region Private Methods

        private MimeMessage CreateEmailMessage(string email, string subject, MimeEntity body)
        {
            var emailMessage = new MimeMessage { Subject = subject, Body = body };
            emailMessage.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.Email));
            emailMessage.To.Add(new MailboxAddress("", email));

            return emailMessage;
        }

        private MimeMessage CreateGeneralEmailMessage(IEnumerable<string> emails, string subject, MimeEntity body)
        {
            var emailMessage = new MimeMessage { Subject = subject, Body = body };
            emailMessage.From.Add(new MailboxAddress(_generalEmailSettings.Name, _generalEmailSettings.Email));
            emailMessage.To.AddRange(emails.Select(email => new MailboxAddress("", email)));

            return emailMessage;
        }

        private MimeEntity CreateConfirmEmailBody(string guid, string confirmCode)
        {
            return new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<td class='esd-stripe' align='center'>                                                                                                                                                                                                                    " +
"    <table class='es-content-body'  style='max-width:390px; width:100%; display:block' cellspacing='0' cellpadding='0' bgcolor='#ffffff' align='center'>                                                                                                                         " +
"        <tbody>                                                                                                                                                                                                                                                                  " +
"            <tr>                                                                                                                                                                                                                                                                 " +
"                <td class='esd-structure es-p20t es-p20b es-p20r es-p20l' align='left'>                                                                                                                                                                                          " +
"                    <table width='100%' cellspacing='0' cellpadding='0'>                                                                                                                                                                                                         " +
"                        <tbody>                                                                                                                                                                                                                                                  " +
"                            <tr>                                                                                                                                                                                                                                                 " +
"                                <td class='esd-container-frame' width='350' valign='top' align='center'>                                                                                                                                                                         " +
"                                    <table width='100%' cellspacing='0' cellpadding='0'>                                                                                                                                                                                         " +
"                                        <tbody>                                                                                                                                                                                                                                  " +
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td align='left' class='esd-block-image' style='font-size: 0px;'>                                                                                                                                                                " +
"                                                    <a target='_blank'>                                                                                                                                                                                                          " +
"                                                        <div style='overflow:hidden; width:80px'>                                                                                                                                                                                " +
"                                                        <img class='adapt-img' src='https://hkl.stripocdnplugin.email/content/d7b6d2ba18fb4bc0abf8a55b4bdfb791/lib/pluginId_d7b6d2ba18fb4bc0abf8a55b4bdfb791_71172/55761587575661955.jpg' height='90' alt style='display: block'>" +
"                                                    </div></a>                                                                                                                                                                                                                   " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td class='esd-block-text ' align='left'>                                                                                                                                                                                        " +
"                                                    <h2 style='color: #353539;'><strong>Привет!</strong></h2>                                                                                                                                                                    " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td align='left' class='esd-block-text'>                                                                                                                                                                                         " +
"                                                    <div>Благодарим за выбор XCE FACTOR!<br>                                                                                                                                                                                     " +
"														XCE FACTOR — это кастинг в новый формат шоу талантов. Без жюри, накрутки голосов и цензуры. <br><br>                                                                                                                      " +
"														Покажи свой талант всему миру, решай, кто самый талантливый&nbsp;и кто должен участвовать в финальном&nbsp;шоу, чтобы&nbsp;получить главный приз!<br><br>                                                                 " +
"														Чтобы завершить регистрацию аккаунта, подтвердите адрес эл. почты. Это поможет защитить аккаунт и позволит при необходимости восстановить пароль.<br><br>                                                                 " +
"													</div>                                                                                                                                                                                                                        " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td align='center' class='esd-block-button es-p10'>                                                                                                                                                                              " +
"                                                    <span class='es-button-border' style='background: #9435d0; display: block; height: 50px; width:95%;'>                                                                                                                        " +
$"                                                        <a href='{_avatarAppSettings.WebUrl}/auth/confirm?guid={guid}&confirmCode={confirmCode}' class='es-button' target='_blank' style='color:#FFFFFF;text-decoration:none;                                                                                                                                     " +
"                                                        background: #9435d0; border-color: #9435d0; font-size: 16px;                                                                                                                                                             " +
"                                                        border-left-width: 0px; border-right-width: 0px;                                                                                                                                                                         " +
"                                                        display: inline-block; vertical-align: middle;'>Подтвердить эл. почту</a>                                                                                                                                                " +
"                                                        <span style='display: inline-block; height:100%; vertical-align: middle;'></span>                                                                                                                                        " +
"                                                    </span>                                                                                                                                                                                                                      " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                            <tr>" +
"                                                <td> "+
"                                                 <div>Срок действия ссылки истекает через 10 минут.<br>" +
"                                                      После истечения времени вы можете повторно запросить подтверждение почты по <a href='http://web.xce-factor.ru/#/auth/confirmation/send'>ссылке</a></div>" +
"                                                </td>     "+
"                                            </tr>"+
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td align='center' class='esd-block-text es-m-txt-l'>                                                                                                                                                                            " +
"                                                    <div style='width:70%; font-size:12px; padding-top:5px;color: #cccccc;'>Если вы не регистрировались, пожалуйста, игнорируйте это письмо</div>                                                                                " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                        </tbody>                                                                                                                                                                                                                                 " +
"                                    </table>                                                                                                                                                                                                                                     " +
"                                </td>                                                                                                                                                                                                                                            " +
"                            </tr>                                                                                                                                                                                                                                                " +
"                        </tbody>                                                                                                                                                                                                                                                 " +
"                    </table>                                                                                                                                                                                                                                                     " +
"                </td>                                                                                                                                                                                                                                                            " +
"            </tr>                                                                                                                                                                                                                                                                " +
"        </tbody>                                                                                                                                                                                                                                                                 " +
"    </table>                                                                                                                                                                                                                                                                     " +
"</td>                                                                                                                                                                                                                                                                            " 
            };
        }


        private MimeEntity CreatePasswordResetBody(string guid, string confirmCode)
        {
            return new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<td class='esd-stripe' align='center'>                                                                                                                                                                                                                    " +
"    <table class='es-content-body'  style='max-width:390px; width:100%; display:block' cellspacing='0' cellpadding='0' bgcolor='#ffffff' align='center'>                                                                                                                         " +
"        <tbody>                                                                                                                                                                                                                                                                  " +
"            <tr>                                                                                                                                                                                                                                                                 " +
"                <td class='esd-structure es-p20t es-p20b es-p20r es-p20l' align='left'>                                                                                                                                                                                          " +
"                    <table width='100%' cellspacing='0' cellpadding='0'>                                                                                                                                                                                                         " +
"                        <tbody>                                                                                                                                                                                                                                                  " +
"                            <tr>                                                                                                                                                                                                                                                 " +
"                                <td class='esd-container-frame' width='350' valign='top' align='center'>                                                                                                                                                                         " +
"                                    <table width='100%' cellspacing='0' cellpadding='0'>                                                                                                                                                                                         " +
"                                        <tbody>                                                                                                                                                                                                                                  " +
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td align='left' class='esd-block-image' style='font-size: 0px;'>                                                                                                                                                                " +
"                                                    <a target='_blank'>                                                                                                                                                                                                          " +
"                                                        <div style='overflow:hidden; width:80px'>                                                                                                                                                                                " +
"                                                        <img class='adapt-img' src='https://hkl.stripocdnplugin.email/content/d7b6d2ba18fb4bc0abf8a55b4bdfb791/lib/pluginId_d7b6d2ba18fb4bc0abf8a55b4bdfb791_71172/55761587575661955.jpg' height='90' alt style='display: block'>" +
"                                                    </div></a>                                                                                                                                                                                                                   " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td class='esd-block-text ' align='left'>                                                                                                                                                                                        " +
"                                                    <h2 style='color: #353539;'><strong>Привет!</strong></h2>                                                                                                                                                                    " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td align='left' class='esd-block-text'>                                                                                                                                                                                         " +
"                                                    <div>Благодарим за выбор XCE FACTOR!<br>                                                                                                                                                                                     " +
"														XCE FACTOR — это кастинг в новый формат шоу талантов. Без жюри, накрутки голосов и цензуры. <br><br>                                                                                                                      " +
"														Покажи свой талант всему миру, решай, кто самый талантливый&nbsp;и кто должен участвовать в финальном&nbsp;шоу, чтобы&nbsp;получить главный приз!<br><br>                                                                 " +
"														Чтобы завершить регистрацию аккаунта, подтвердите адрес эл. почты. Это поможет защитить аккаунт и позволит при необходимости восстановить пароль.<br><br>                                                                 " +
"													</div>                                                                                                                                                                                                                        " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td align='center' class='esd-block-button es-p10'>                                                                                                                                                                              " +
"                                                    <span class='es-button-border' style='background: #9435d0; display: block; height: 50px; width:95%;'>                                                                                                                        " +
$"                                                        <a href='{_avatarAppSettings.WebUrl}/auth/password/change?guid={guid}&confirmCode={confirmCode}' class='es-button' target='_blank' style='color:#FFFFFF;text-decoration:none;                                                                                                                                     " +
"                                                        background: #9435d0; border-color: #9435d0; font-size: 16px;                                                                                                                                                             " +
"                                                        border-left-width: 0px; border-right-width: 0px;                                                                                                                                                                         " +
"                                                        display: inline-block; vertical-align: middle;'>Сменить пароль</a>                                                                                                                                                " +
"                                                        <span style='display: inline-block; height:100%; vertical-align: middle;'></span>                                                                                                                                        " +
"                                                    </span>                                                                                                                                                                                                                      " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                            <tr>                                                                                                                                                                                                                                 " +
"                                                <td align='center' class='esd-block-text es-m-txt-l'>                                                                                                                                                                            " +
"                                                    <div style='width:70%; font-size:12px; padding-top:5px;color: #cccccc;'>Если вы не запрашивали это изменение, игнорируйте это письмо.</div>                                                                                " +
"                                                </td>                                                                                                                                                                                                                            " +
"                                            </tr>                                                                                                                                                                                                                                " +
"                                        </tbody>                                                                                                                                                                                                                                 " +
"                                    </table>                                                                                                                                                                                                                                     " +
"                                </td>                                                                                                                                                                                                                                            " +
"                            </tr>                                                                                                                                                                                                                                                " +
"                        </tbody>                                                                                                                                                                                                                                                 " +
"                    </table>                                                                                                                                                                                                                                                     " +
"                </td>                                                                                                                                                                                                                                                            " +
"            </tr>                                                                                                                                                                                                                                                                " +
"        </tbody>                                                                                                                                                                                                                                                                 " +
"    </table>                                                                                                                                                                                                                                                                     " +
"</td>"                                   

            };
        }

        private MimeEntity CreateGeneralMessageBody(string text)
        {
            return new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = text
            };
        }


        private async Task SendMessageAsync(MimeMessage emailMessage)
        {
            await _smtpClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port,
                _emailSettings.UseSsl);
            await _smtpClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
            await _smtpClient.SendAsync(emailMessage);

            await _smtpClient.DisconnectAsync(true);
        }

        private async Task SendGeneralMessageAsync(MimeMessage emailMessage)
        {
            await _smtpClient.ConnectAsync(_generalEmailSettings.Host, _generalEmailSettings.Port,
                _generalEmailSettings.UseSsl);
            await _smtpClient.AuthenticateAsync(_generalEmailSettings.UserName, _generalEmailSettings.Password);
            await _smtpClient.SendAsync(emailMessage);

            await _smtpClient.DisconnectAsync(true);
        }



        #endregion
    }
}
