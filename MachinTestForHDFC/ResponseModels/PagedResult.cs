namespace MachinTestForHDFC.ResponseModels
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; }=new List<T>();
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
}
