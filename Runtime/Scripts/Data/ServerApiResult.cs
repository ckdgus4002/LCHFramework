namespace LCHFramework.Data
{
    public class ServerApiResult
    {
        public ServerApiResult(bool isSuccess, string error) { IsSuccess = isSuccess; Error = error; }

        public bool IsSuccess { get; }
        public string Error { get; }
    }
    
    public class ServerApiResult<T> : ServerApiResult
    {
        public ServerApiResult(bool isSuccess, string error, T value) : base(isSuccess, error) => Value = value;
        
        public T Value { get; }
    }
}