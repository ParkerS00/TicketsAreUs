
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Configs;

public static class DiagnosticConfig
{
    public const string SourceName = "product-service";
    public static ActivitySource Source = new ActivitySource(SourceName);
}