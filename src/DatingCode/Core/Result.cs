using System.Diagnostics;

namespace DatingCode.Core
{
    [DebuggerDisplay("OK: {Success} Error: {ErrorMessage}")]
    public struct Result
    {
        public bool Success;
        public string ErrorMessage;

        public static Result NewFailure(string error)
        {
            return new Result { ErrorMessage = error };
        }

        public static Result NewSuccess()
        {
            return new Result { Success = true };
        }
    }
}
