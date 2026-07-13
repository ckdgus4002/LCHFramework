namespace LCHFramework.Data
{
    public class ServerAPIResult
    {
        public ServerAPIResult(bool isSuccess, string error) { IsSuccess = isSuccess; Error = error; }

        public bool IsSuccess { get; }
        public string Error { get; }
    }
    
    public class ServerAPIResult<T> : ServerAPIResult
    {
        public ServerAPIResult(bool isSuccess, string error, T value) : base(isSuccess, error) => Value = value;
        
        public T Value { get; }
    }
}