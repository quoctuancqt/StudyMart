using System;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Npgsql.Replication;

namespace StudyMart.ApiService.Features;

public static class ApiVersioningConfig
{
    public static ApiVersionSet CreateApiVersionSet(this IEndpointRouteBuilder app)
    {
        var apiVersionSet = app.NewApiVersionSet()
                                .HasApiVersion(new ApiVersion(1))
                                .ReportApiVersions()
                                .Build();

        return apiVersionSet;
    }
}
