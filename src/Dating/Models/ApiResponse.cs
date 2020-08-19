namespace Dating.Models
{
    public class ApiResponse
    {
        public bool Success;
        public string ErrorCode;

        public static ApiResponse NewSuccess()
        {
            var result = new ApiResponse()
            {
                Success = true
            };
            return result;
        }

        public static ApiResponse NewFail(string errorCode)
        {
            var result = new ApiResponse()
            {
                Success = false,
                ErrorCode = errorCode
            };
            return result;
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Value;

        public static ApiResponse NewSuccess(T value)
        {
            var result = new ApiResponse<T>()
            {
                Success = true,
                Value = value
            };
            return result;
        }

        public static new ApiResponse<T> NewFail(string errorCode)
        {
            var result = new ApiResponse<T>()
            {
                Success = false,
                ErrorCode = errorCode
            };
            return result;
        }
    }
}
