namespace WebApiOptions.Settings;

public sealed class AppSettings
{
    public string ApiKey { get; set; } = string.Empty;

    public string Theme { get; set; } = "light";
    public int MaxItemsPerPage { get; set; } = 10;
}
