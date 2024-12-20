using data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Zurnal_Vizual.ViewModels;
using remoteData.RemoteDataBase;
using domain.UseCase;

namespace Zurnal_Vizual.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonService(this IServiceCollection collection)
        {
            collection
                .AddDbContext<RemoteDatabaseContext>()
                .AddSingleton<IGroupRepository, SQLGroupRepositoryImpl>()
                .AddSingleton<IUserRepository,SQLUserRepositoryImpl>()
                .AddSingleton<IPresenceRepository,SQLPresenceRepositoryImpl>()
                .AddSingleton<UseCaseGeneratePresence>()
                .AddSingleton<UserUseCase>()
                .AddTransient<GroupUseCase>()
                .AddTransient<MainWindowViewModel>();
        }
    }
}
