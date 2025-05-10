using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StudyMart.AppHost;


internal static class KeycloakResourceBuilderExtensions
{
    public static IResourceBuilder<KeycloakResource> WithExportRealmCommand(
        this IResourceBuilder<KeycloakResource> builder, string realmName, string exportPath)
    {
        builder.WithCommand(
            "export-realm",
            "Export Realm",
            context => OnRunExportRealmCommand(builder, context, realmName, exportPath),
            iconName: "export",
            iconVariant: IconVariant.Regular
        );

        return builder;
    }

    private static async Task<ExecuteCommandResult> OnRunExportRealmCommand(
        IResourceBuilder<KeycloakResource> builder,
        ExecuteCommandContext context,
        string realmName,
        string exportPath)
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var containerName = context.ResourceName;

        await ExportRealmInsideContainer(realmName, containerName);
        await CopyExportFromContainerToRealmImportFolder(containerName, exportPath);

        return CommandResults.Success();
    }

    private static async Task ExportRealmInsideContainer(string realmName, string containerName)
    {
        var result = await Cli.Wrap("docker")
            .WithArguments(args =>
                args.Add("exec").Add(containerName)
                    .Add("/opt/keycloak/bin/kc.sh")
                    .Add("export")
                    .Add($"--realm={realmName}")
                    .Add("--file=/tmp/realm-export.json"))
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync();

        if (result.ExitCode != 0 || !result.StandardOutput.Contains("Export finished successfully"))
            throw new InvalidOperationException(
                $"Export failed. Exit code: {result.ExitCode}\nError: {result.StandardError}");
    }

    private static async Task CopyExportFromContainerToRealmImportFolder(string containerName, string exportPath)
    {
        var result = await Cli.Wrap("docker")
            .WithArguments(args =>
                args.Add("cp")
                    .Add($"{containerName}:/tmp/realm-export.json")
                    .Add($"{exportPath}"))
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync();

        if (result.ExitCode != 0)
            throw new InvalidOperationException(
                $"Copy failed. Exit code: {result.ExitCode}\nError: {result.StandardError}");
    }
}