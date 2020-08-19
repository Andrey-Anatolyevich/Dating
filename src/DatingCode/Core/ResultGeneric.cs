using System;
using System.Diagnostics;

namespace DatingCode.Core
{
    [DebuggerDisplay("OK: {Success} Error: {ErrorMessage} Value: {Value}")]
    public struct Result<T>
    {
        public bool Success;
        public string ErrorMessage;
        public T Value;

        public static Result<T> NewFailure(string error)
        {
            return new Result<T> { ErrorMessage = error };
        }

        public static Result<T> NewSuccess(T data)
        {
            return new Result<T>
            {
                Value = data,
                Success = true
            };
        }
    }
}
