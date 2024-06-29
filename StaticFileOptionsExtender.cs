using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;

namespace CWDev
{
    /// <summary>
    /// Handles WebGL specific <see langword="MIME"/> and <see langword="ContentEncoding"/>
    /// types and file path handling for the <see langword="Gzip"/> and <see langword="Brotli"/> compressions.<br/>
    /// Inteded for <see cref="WebApplication.UseStaticFiles"/> request middleware pipeline
    /// </summary>
    /// <remarks>
    /// Also exposes <see cref="CustomContentTypeProvider.AddCompressionEncoding"/><br/>
    /// method for extending additional <see cref="StaticFileOptionsExtender.CompressionEncodings"/> types
    /// </remarks>
    public static class StaticFileOptionsExtender
    {
        private const string GZIP_EXTENSION = ".gz";
        private const string BROTLI_EXTENSION = ".br";

        // This Dictionary will be used to look up if a file's extension type is a compression type
        private static readonly Dictionary<string, string> _compressionEncodings = new()
        {
            { GZIP_EXTENSION, "gzip" },
            { BROTLI_EXTENSION, "br" }
        };

        public static IReadOnlyDictionary<string, string> CompressionEncodings => _compressionEncodings;

        /// <summary>
        /// Exposes custom <see cref="StaticFileOptions"/> for request middleware pipeline
        /// </summary>
        /// <remarks>
        /// Handles identifying correct file types from compressed files for <see langword="MIME"/>
        /// types and <see langword="ContentEncoding"/> response Headers
        /// </remarks>
        /// <returns>Returns custom <see cref="StaticFileOptions"/></returns>
        public static StaticFileOptions GetOptions()
        {
            var customFileTypeProvider = new CustomContentTypeProvider();
            return new StaticFileOptions
            {
                ContentTypeProvider = customFileTypeProvider,
                OnPrepareResponse = (StaticFileResponseContext context) =>
                {
                    // In addition to the MIME type also set the according encoding header (e.g. "br")
                    if (CompressionEncodings.TryGetValue(Path.GetExtension(context.File.Name), out string? encoding))
                    {
                        context.Context.Response.Headers["Content-Encoding"] = encoding;
                    }
                }
            };
        }

        /// <summary>
        /// Handles the MIME type mapping for <see cref="StaticFileOptions"/>
        /// to pass to <see cref="WebApplication.UseStaticFiles"/> request middleware pipeline config
        /// </summary>
        /// <remarks>
        /// <see cref="FileExtensionContentTypeProvider"/> IDictionary property already handles<br/>
        /// the MIME type mappings for the 380 most commonly used file types 
        /// this extends it for Unity WebGL build .data file extension type and according MIME type
        /// </remarks>
        private class CustomContentTypeProvider : IContentTypeProvider
        {
            private readonly FileExtensionContentTypeProvider fileTypeProvider = new();

            public CustomContentTypeProvider()
            {
                fileTypeProvider.Mappings[".data"] = "application/octet-stream";
            }

            #region Private Class Methods
            /// <summary>
            /// Interface method used when the app tries to find a MIME mapping for a served static file<br/>
            /// Handles truncating file compression extension so actual File <see langword="ContentType"/> can be properly mapped by its file type extension
            /// </summary>
            /// <example>
            /// Truncating the .gz or .br compression extension and get the actual extension
            /// "Build\something\image.png.gz" -> "Build\something\image.png" -> ".png"
            /// </example>
            /// <remarks>
            /// Leveraging <see cref="System.IO.Path"/> methods normalizes path separators.<br/>
            /// Edge Case: <see cref="Path.GetDirectoryName"/> "root directory" arg returns null<br/>
            /// However <see cref="Path.Combine"/> leverages <see langword="Path.CombineInternal"/> handling null or empty
            /// But I pass string.Empty so the interpreter stops complaining
            /// </remarks>
            /// <param name="filePath">Static file path string containing compression extension</param>
            /// <param name="contentType"> Code smell out param .NET
            /// <see cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
            /// </param>
            /// <returns>The according MIME type</returns>
            public bool TryGetContentType(string filePath, out string contentType)
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

                // Gets the last "." exentsion (compression type ".gz" from "file.png.gz" if multiple extensions exist)
                var extension = Path.GetExtension(filePath);

                // If extension is a compression type extension (e.g. ".gz") truncates it to expose base file extension (e.g. ".png")
                if (_compressionEncodings.ContainsKey(extension))
                {
                    var filePathDirectory = Path.GetDirectoryName(filePath);
                    var fileNameTruncated = Path.GetFileNameWithoutExtension(filePath);
                    filePath = Path.Combine(filePathDirectory ?? string.Empty, fileNameTruncated);
                }

#pragma warning disable CS8601 // Possible null reference assignment.
                return fileTypeProvider.TryGetContentType(filePath, out contentType);
#pragma warning restore CS8601 // Possible null reference assignment.
            }

            /// <summary>
            /// Method to add new compression mappings. Follow this pattern: 
            /// <see langword="extension"/> -> ".gz" (compresision type extension) <see langword="encoding"/> -> "gzip" (compression name)
            /// </summary>
            /// <param name="extension">A File extension like ".gz"</param>
            /// <param name="encoding">The correct encoding type for the file extension type like "gzip"</param>
            /// <returns>True if compression mapping added, False if already exists</returns>
            public static bool AddCompressionEncoding(string extension, string encoding)
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(extension);
                ArgumentNullException.ThrowIfNullOrWhiteSpace(encoding);

                return _compressionEncodings.TryAdd(extension, encoding);
            }
            #endregion
        }
    }
}
