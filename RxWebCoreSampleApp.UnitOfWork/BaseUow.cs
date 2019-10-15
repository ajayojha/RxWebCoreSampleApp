using RxWeb.Core.Data;

namespace RxWebCoreSampleApp.UnitOfWork
{
    public class BaseUow : CoreUnitOfWork
    {
        public BaseUow(IDbContext context, IRepositoryProvider repositoryProvider) : base(context, repositoryProvider) { }
    }
}


