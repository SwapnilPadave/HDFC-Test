namespace MachinTestForHDFC.ResponseModels
{
    public class ApiResponse<T>
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public T? data { get; set; }
        public List<Errors> errors { get; set; } = new List<Errors>();
    }

    public class Errors
    {
        public string PropertyName { get; set; } = string.Empty;
        public required string[] ErrorMessages { get; set; }
    }
}
