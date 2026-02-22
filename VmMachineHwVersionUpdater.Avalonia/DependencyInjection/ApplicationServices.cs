namespace VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

/// <summary>
///     Provides access to the application's service provider for dependency injection.
/// </summary>
public static class ApplicationServices
{
    /// <summary>
    ///     Gets the application's service provider used for dependency injection.
    /// </summary>
    /// <remarks>
    ///     This property provides access to the IServiceProvider instance, which can be used to resolve
    ///     services and dependencies throughout the application. It is typically set during application startup and should
    ///     not be modified afterward.
    /// </remarks>
    private static IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    ///     Initializes the application's service provider for dependency resolution.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceProvider" /> is null.</exception>
    public static void Initialize(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    /// <summary>
    ///     Retrieves the required service of the specified type from the application's service provider.
    /// </summary>
    /// <remarks>
    ///     Throws an exception if the service of type T is not registered with the service provider. Use
    ///     this method when the service is essential for application operation and must be available.
    /// </remarks>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <returns>An instance of the requested service type T.</returns>
    public static T GetRequiredService<T>() => ServiceProvider.GetRequiredService<T>();
}