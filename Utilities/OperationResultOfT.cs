namespace SmartBookmarkApi.Utilities
{
    public class OperationResultOfT<T> : OperationResult
    {
        public T? Data { get; set; }
    }
}
