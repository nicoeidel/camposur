using Camposur.Core;
using Camposur.Core.Helpers;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camposur.BusinessLogic.Logic.Interfaces
{
    public class SendGridLogic : ISendGridLogic
    {
        public async Task<Response> SendEmailValidation(string recipName, string recipEmail, string linkUrl)
        {
            var templateId = ConfigurationHelper.SendGrid_TemplateEmailValidation;

            return await SendRegistrationEmail(recipName, recipEmail, templateId, linkUrl);
        }

        public async Task<Response> SendPasswordReset(string recipName, string recipEmail, string linkUrl)
        {
            var templateId = ConfigurationHelper.SendGrid_TemplatePasswordReset;

            return await SendRegistrationEmail(recipName, recipEmail, templateId, linkUrl);
        }

        public async Task<Response> SendUserRegistrationByAdminAlert(string recipName, string recipEmail, string linkUrl)
        {
            var templateId = ConfigurationHelper.SendGrid_TemplateUserRegistrationByAdminAlert;

            return await BuildEmail(recipName, recipEmail, templateId, linkUrl);
        }

        public async Task<Response> SendUserRegistrationAlert(string recipName, string recipEmail, string linkUrl)
        {
            var templateId = ConfigurationHelper.SendGrid_TemplateUserRegistrationAlert;

            return await BuildEmail(recipName, recipEmail, templateId, linkUrl);
        }

        private async Task<Response> BuildEmail(string recipName, string recipEmail, string templateId, string linkUrl)
        {
            var recipient = new EmailAddress(recipEmail, recipName);
            var substitutions = new Dictionary<string, string>
            {
                {
                    Constants.SendGridRecipientName, recipName
                },
                {
                    Constants.SendGridEmailLinkUrl, linkUrl
                }
            };

            return await SendTemplateEmail(recipient, templateId, substitutions);
        }

        private async Task<Response> SendRegistrationEmail(string recipName, string recipEmail, string templateId, string linkUrl)
        {
            var recipient = new EmailAddress(recipEmail, recipName);
            var substitutions = new Dictionary<string, string>
            {
                {
                    Constants.SendGridRecipientName, recipName
                },
                {
                    Constants.SendGridEmailLinkUrl, linkUrl
                }
            };

            return await SendTemplateEmail(recipient, templateId, substitutions);
        }

        private async Task<Response> SendTemplateEmail(EmailAddress recipient, string templateId, Dictionary<string, string> substitutions)
        {
            var client = new SendGridClient(ConfigurationHelper.SendGrid_ApiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress(Constants.SendGrid_RegisterInternalEmail, Constants.SendGrid_RegisterEmailSubject));

            var recipients = new List<EmailAddress> { recipient };
            msg.AddTos(recipients);

            msg.AddSubstitutions(substitutions);
            msg.TemplateId = templateId;

            return await client.SendEmailAsync(msg);
        }

        public async Task<Response> SendEmailValidation(string recipName, string recipEmail, string subject, string body, string linkUrl)
        {
            return await SendRegistrationEmail(recipName, recipEmail, subject, body, linkUrl);
        }

        private async Task<Response> SendRegistrationEmail(string recipName, string recipEmail, string subject, string body, string linkUrl)
        {
            var recipient = new EmailAddress(recipEmail, recipName);
            var substitutions = new Dictionary<string, string>
            {
                {
                    Constants.SendGridRecipientName, recipName
                },
                {
                    Constants.SendGridEmailLinkUrl, linkUrl
                }
            };
            return await SendHtmlEmail(recipient, body, subject, substitutions);
        }

        public async Task<Response> SendHtmlEmail(EmailAddress recipient, string body, string subject, Dictionary<string, string> substitutions)
        {
            var client = new SendGridClient(ConfigurationHelper.SendGrid_ApiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress(Constants.SendGrid_RegisterInternalEmail, Constants.SendGrid_RegisterEmailSubject));

            var recipients = new List<EmailAddress> { recipient };
            msg.AddTos(recipients);

            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, body);

            msg.AddSubstitutions(substitutions);

            return await client.SendEmailAsync(msg);
        }
    }
}
