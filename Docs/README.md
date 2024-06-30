# Static File Options Extender

## Purpose:
1. Enable .NET applications to serve static files which have been compressed via `gzip` and/or `brotli` \
    compression algorithms, correctly mapping their original file's content-types and response headers.
2. Allow user to extend the Options Class to add additional compression types and configure mappings.

## To Use:
1. Leverage the package's `static using` directive*
2. Call the package's `GetOptions` method as the argument to `UseStaticFiles(...)`* \
    this will pass the extended `StaticFileOptions` while building the request pipeline.
```C#
using static CWDev.StaticFileOptionsExtender;      // <-- *

var builder = WebApplication.CreateBuilder(args);

// ...

var app = builder.Build();

// ...

app.UseStaticFiles(GetOptions());                  // <-- *
```

## Abstract:
.NET `WebApplicationBuilder` can be leveraged to configure the application HTTP Request Pipeline. \
Within that Pipeline `StaticFileMiddleware` can be used to serve static files. \
\
Typically, the `app.UseStaticFiles(StaticFileOptions options)` middleware configures a few key \
steps. This library is mostly concerned with TWO -- Providers and Handlers -- regarding:
- Mapping files `content-types`:
  - `IContentTypeProvider StaticFileOptions.ContentTypeProvider`*
- Adding or changing the response headers:
  - `Action<StaticFileResponseContext> StaticFileOptions.OnPrepareResponse`*

`StaticFileOptions StaticFileOptionsExtender.GetOptions`
```C#
public static StaticFileOptions GetOptions()
{
    var customFileTypeProvider = new CustomContentTypeProvider();
    return new StaticFileOptions
    {
        ContentTypeProvider = customFileTypeProvider,                // <-- *
        OnPrepareResponse = (StaticFileResponseContext context) =>   // <-- *
        {
            if (CompressionEncodings.TryGetValue(Path.GetExtension(context.File.Name), out string? encoding))
            {
                context.Context.Response.Headers.ContentEncoding = encoding;
            }
        }
    };
}
```

Specifically, `StaticFileOptionsExtender` leverages a class inheriting from `IContentTypeProvider` \
to extend the MIME type mappings for the 380 most commonly used file types (default). \
This library originally was built to enable .NET to serve static files which have been compressed for WebGL \
by building from Unity.

## Overview:

## Release-Notes:
- [CHANGELOG](https://github.com/colinwilliams91/StaticFileOptionsExtender/blob/main/CHANGELOG.md)
  - The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html)
  - Commits follow [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) patterns

## Contribute:
- Please open a [GitHub Issue](https://github.com/colinwilliams91/StaticFileOptionsExtender/issues) with concerns.
- Open a [PR](https://github.com/colinwilliams91/StaticFileOptionsExtender/pulls) if you have a code proposal.
- I will respond in `<=24` hours with thoughts and/or solution.