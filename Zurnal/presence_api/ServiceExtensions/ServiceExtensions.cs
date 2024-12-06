using data.Repository;
using domain.UseCase;

public static class ServiceExtensions
{
    public static void ConfigurateGroup(this IServiceCollection services)
    {
        services
        .AddScoped<IGroupRepository, SQLGroupRepositoryImpl>()
        .AddScoped<GroupUseCase>();
    }

    public static void ConfigurateUser(this IServiceCollection services)
    {
        services
        .AddScoped<IUserRepository, SQLUserRepositoryImpl>()
        .AddScoped<UserUseCase>();
    }

    public static void ConfiguratePresence(this IServiceCollection services)
    {
        services
        .AddScoped<IPresenceRepository, SQLPresenceRepositoryImpl>()
        .AddScoped<UseCaseGeneratePresence>();

    }

    public static void ConfigurateAdminPanel(this IServiceCollection services)
    {
        services
            .AddScoped<IGroupRepository, SQLGroupRepositoryImpl>()
            .AddScoped<IUserRepository, SQLUserRepositoryImpl>()
            .AddScoped<IPresenceRepository, SQLPresenceRepositoryImpl>()
            .AddScoped<GroupUseCase>()
            .AddScoped<UserUseCase>()
            .AddScoped<UseCaseGeneratePresence>();
    }

    public static void ConfigurateRepositories(this IServiceCollection services)
    {
        services.AddScoped<IGroupRepository, SQLGroupRepositoryImpl>();
        services.AddScoped<IUserRepository, SQLUserRepositoryImpl>();
        services.AddScoped<IPresenceRepository, SQLPresenceRepositoryImpl>();
    }
}