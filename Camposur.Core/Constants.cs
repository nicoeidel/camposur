namespace Camposur.Core
{
    public class Constants
    {
        public const string RoleNameAdmin = "Role_Admin";

        public const string DefaultAdmin_Email = "nicoeidel@hotmail.com";
        public const string DefaultAdmin_Password = "Password01";

        public const string SendGridRecipientName = "-userName-";
        public const string SendGridEmailLinkUrl = "-linkUrl-";

        public const string SendGrid_RegisterInternalEmail = "no-reply@test.com";
        public const string SendGrid_RegisterEmailSubject = "Servicio de Registro";
        public const string SendGrid_RegisterEmailBody = "<html><p>Hola, -userName-! <br />Su registro está casi completo! Por favor, siga este enlace: <br /><a href='-linkUrl-'>Validar Email</a></p></html>";

        public const string SendGridAuthLink_ConfirmEmail = "?userID={0}&code={1}";
        public const string SendGridAuthLink_ResetPassword = "?userID={0}&code={1}";
    }
}