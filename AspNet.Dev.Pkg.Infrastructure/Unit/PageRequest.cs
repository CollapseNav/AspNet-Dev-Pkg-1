namespace AspNet.Dev.Pkg.Infrastructure.Unit
{
    public class PageRequest
    {
        public int Index { get; set; } = 1;
        public int Max { get; set; } = 10;
        /// <summary>
        /// 不用传，会自动计算
        /// </summary>
        public int Skip
        {
            get => skip ?? (Index - 1) * Max; set => skip = value;
        }
        private int? skip;
    }
}
