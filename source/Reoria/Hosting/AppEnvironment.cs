namespace Reoria.Hosting
{
    public static class AppEnvironment
    {
        public static readonly string[] Environments = { "Development", "Staging", "Production" };
        public static readonly string ActiveEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
    }
}
