namespace LCHFramework.Data
{
    public class ServerApiResult
    {
        public ServerApiResult(bool isSuccess, string error, string errorResponseText) { IsSuccess = isSuccess; Error = error; ErrorResponseText = errorResponseText; }

        public bool IsSuccess { get; }
        public string Error { get; }
        public string ErrorResponseText { get; }
    }
    
    public class ServerApiResult<T> : ServerApiResult
    {
        public ServerApiResult(bool isSuccess, string error, string errorResponseText, T value) : base(isSuccess, error, errorResponseText) => Value = value;
        
        public T Value { get; }
    }
}