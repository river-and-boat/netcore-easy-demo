namespace UserService.Controller.Response
{
    public class CommonResponse<T>
    {
        public T Data { get; set; }
        public int Code { get; set; }

        public CommonResponse(T Data, int Code)
        {
            this.Data = Data;
            this.Code = Code;
        }
    }
}
