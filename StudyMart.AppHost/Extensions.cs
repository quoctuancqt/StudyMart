namespace StudyMart.AppHost;

internal static class Extensions
{
    public static IResourceBuilder<ExecutableResource>? AddTodoDbMigration(this IDistributedApplicationBuilder builder)
    {
        IResourceBuilder<ExecutableResource>? migrateOperation = default;

        if (builder.ExecutionContext.IsRunMode)
        {
            migrateOperation = builder.AddEfMigration<Projects.StudyMart_ApiService>("todo-db-migration");
        }

        return migrateOperation;
    }

    public static IResourceBuilder<ExecutableResource> AddEfMigration<TProject>(this IDistributedApplicationBuilder builder, string name)
        where TProject : IProjectMetadata, new()
    {
        var projectDirectory = Path.GetDirectoryName(new TProject().ProjectPath)!;

        // TODO: Support passing a connection string
        return builder.AddExecutable(name, "dotnet", projectDirectory, "ef", "database", "update", "--no-build");
    }

    public static string GetProjectDirectory(this IResourceBuilder<ProjectResource> project) =>
        Path.GetDirectoryName(project.Resource.GetProjectMetadata().ProjectPath)!;
}