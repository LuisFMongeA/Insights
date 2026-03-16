using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Insights.SharedKernel.Extensions;

public static class ConfigurationExtensions
{
    public static WebApplicationBuilder AddSharedConfiguration(
        this WebApplicationBuilder builder)
    {
        // Busca el fichero compartido subiendo niveles hasta encontrarlo
        var sharedConfigPath = FindSharedConfig(
            builder.Environment.ContentRootPath);

        if (sharedConfigPath is not null)
        {
            builder.Configuration.AddJsonFile(
                sharedConfigPath,
                optional: false,
                reloadOnChange: true);
        }
        else
        {
            // No lanza excepción, solo avisa
            // Puede que en producción no exista y se use variables de entorno
            Console.WriteLine("appsettings.shared.json not found, skipping...");
        }

        return builder;
    }

    private static string? FindSharedConfig(string startPath)
    {
        var directory = new DirectoryInfo(startPath);

        // Sube hasta 5 niveles buscando el fichero
        for (int i = 0; i < 5; i++)
        {
            var filePath = Path.Combine(directory.FullName, "appsettings.shared.json");
            if (File.Exists(filePath))
                return filePath;

            if (directory.Parent is null) break;
            directory = directory.Parent;
        }

        return null;
    }
}