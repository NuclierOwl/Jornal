using data.Repository;
using domain.UseCase;
using remoteData.RemoteDataBase;
using ui;
var services = new ServiceCollection();

services
    .AddDbContext<RemoteDatabaseContext>()
    .AddSingleton<IGroupRepository, SQLGroupRepositoryImpl>()
    .AddSingleton<IUserRepository, SQLUserRepositoryImpl>()
    .AddSingleton<IPresenceRepository, SQLPresenceRepositoryImpl>()
    .AddSingleton<UserUseCase>()
    .AddSingleton<GroupUseCase>()
    .AddSingleton<UseCaseGeneratePresence>()
    .AddSingleton<GroupConsoleUI>()
    .AddSingleton<PresenceConsole>()
    .AddSingleton<MainMenuUI>();

var serviceProvider = services.BuildServiceProvider();
MainMenuUI mainMenuUI = serviceProvider.GetService<MainMenuUI>();
mainMenuUI.DisplayMenu();