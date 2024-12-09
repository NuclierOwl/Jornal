using data.Repository;
using domain.Service;
using domain.UseCase;
using Microsoft.Extensions.DependencyInjection;
using Presence.Desktop.ViewModels;
using remoteData.RemoteDataBase;

namespace Presence.Desktop.DI
{
    public static class ServiceColletionExtensions
    {
        public static void AddCommonService(this IServiceCollection collection) {
            collection
             .AddDbContext<RemoteDatabaseContext>()
             .AddSingleton<IGroupRepository, SQLGroupRepositoryImpl>()
             .AddTransient<GroupUseCase, GroupService>()
             .AddTransient<GroupViewModel>();
        }
    }
}
