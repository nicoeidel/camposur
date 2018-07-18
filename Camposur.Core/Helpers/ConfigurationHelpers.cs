using System.Configuration;

namespace Camposur.Core.Helpers
{
    public static class ConfigurationHelper
    {

        #region configuration_keys

        private const string Const_SendGridApiKey = "SendGridApiKey";
        private const string Const_SendGrid_TemplateEmailValidation = "SendGridTemplateEmailValidation";
        private const string Const_SendGrid_TemplatePasswordReset = "SendGridTemplatePasswordReset";
        private const string Const_SendGrid_TemplateUserRegistrationAlert = "SendGridTemplateUserRegistrationAlert";
        private const string Const_SendGrid_TemplateUserRegistrationByAdminAlert = "SendGridTemplateUserRegistrationByAdminAlert";

        #endregion

        private static string GetParam(string paramKey)
        {
            return ConfigurationManager.AppSettings[paramKey];
        }

        #region SendGrid

        public static string SendGrid_ApiKey
        {
            get
            {
                return GetParam(Const_SendGridApiKey);
            }
        }

        public static string SendGrid_TemplateEmailValidation
        {
            get
            {
                return GetParam(Const_SendGrid_TemplateEmailValidation);
            }
        }

        public static string SendGrid_TemplatePasswordReset
        {
            get
            {
                return GetParam(Const_SendGrid_TemplatePasswordReset);
            }
        }

        public static string SendGrid_TemplateUserRegistrationAlert => GetParam(Const_SendGrid_TemplateUserRegistrationAlert);

        public static string SendGrid_TemplateUserRegistrationByAdminAlert => GetParam(Const_SendGrid_TemplateUserRegistrationByAdminAlert);

        #endregion
    }
}
