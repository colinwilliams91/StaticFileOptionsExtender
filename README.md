# Static File Options Extender

## Purpose:
1. Enable .NET applications to serve static files which have been compressed via `gzip` and/or `brotli` \
    compression algorithms, correctly mapping their original file's content-types and response headers.
2. Allow user to extend the Options Class to add additional compression types and configure mappings.

## To Use:
```C#
using CWDev.StaticFileOptionsExtender;

var builder = WebApplication.CreateBuilder(args);

// ...

var app = builder.Build();

// ...

app.UseStaticFiles(StaticFileOptionsExtender.GetOptions()); // <--
```

## Abstract:
.NET `WebApplicationBuilder` can be leveraged to configure the application HTTP Request Pipeline. \
Within that Pipeline `StaticFileMiddleware` can be used to serve static files. \
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
            // In addition to the MIME type also set the according encoding header (e.g. "br")
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



## Contribute: