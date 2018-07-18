using System;

namespace Camposur.Model.ViewModel
{
    public enum ResetPasswordError { InvalidEmail, Exception }

    public class ResetPasswordResult
    {
        public bool Success { get; set; }

        public ResetPasswordError? ErrorType { get; set; }
        public Exception ErrorException { get; set; }

        public ResetPasswordResult(bool success, ResetPasswordError? eType = null, Exception e = null)
        {
            Success = success;
            ErrorType = eType;
            ErrorException = e;
        }
    }
}