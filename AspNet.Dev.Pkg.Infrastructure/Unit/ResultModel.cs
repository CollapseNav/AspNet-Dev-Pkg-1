namespace AspNet.Dev.Pkg.Infrastructure.Unit
{
    public class BaseResultModel
    {
        public int Code { get; set; }
        public object Result { get; set; }
        public string Msg { get; set; }
    }
}
