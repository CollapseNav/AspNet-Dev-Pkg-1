namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface ICrudController<T, GetT, CreateT> : IController, IModifyController<T, CreateT>, IReadOnlyController<T, GetT>
    where T : IBaseEntity
    where GetT : IBaseGet<T>
    where CreateT : IBaseCreate
    {
    }
}
