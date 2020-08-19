using System;
using System.Diagnostics;

namespace DatingCode.Core
{
    [DebuggerDisplay("OK: {Success} Error: {ErrorMessage} Value: {Value}")]
    public class Maybe<T>
    {
        public bool Success;
        public string ErrorMessage;
        public T Value;

        public static Maybe<T> NewFailure(string error)
        {
            return new Maybe<T> { ErrorMessage = error };
        }

        public static Maybe<T> NewSuccess(T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return new Maybe<T>
            {
                Value = data,
                Success = true
            };
        }

        /// <exception cref="ArgumentException"> If value is ValueType. For value types use explicit methods. </exception>
        public static Maybe<T> NewFromValue(T value, string errorMessageIfNull)
        {
            if (typeof(T).IsValueType)
                throw new ArgumentException($"{nameof(value)} can't be ValueType.");

            if (value != null)
                return NewSuccess(value);

            return NewFailure(errorMessageIfNull);
        }

        internal Result<T> ToResult()
        {
            if (Success)
                return Result<T>.NewSuccess(Value);

            return Result<T>.NewFailure(ErrorMessage);
        }
    }
}
