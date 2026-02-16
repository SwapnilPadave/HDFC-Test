namespace MachinTestForHDFC.ResponseModels
{
    public class ServiceResult
    {
        public bool IsSuccess => !Errors.Any();
        public List<(string Field, string Message)> Errors { get; set; } = new List<(string Field, string Message)>();
    }
}
