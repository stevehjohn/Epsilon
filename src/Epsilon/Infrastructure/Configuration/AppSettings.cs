using System;
using System.IO;
using System.Text.Json;

namespace Epsilon.Infrastructure.Configuration;

public class AppSettings
{
    private static readonly Lazy<AppSettings> Lazy = new(GetAppSettings);

    public static AppSettings Instance => Lazy.Value;

    public Rendering Rendering { get; init; }

    private AppSettings()
    {
    }

    private static AppSettings GetAppSettings()
    {
        var json = File.ReadAllText("app-settings.json");

        return JsonSerializer.Deserialize<AppSettings>(json);
    }
}