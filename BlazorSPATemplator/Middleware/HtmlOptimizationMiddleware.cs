using System.Text;
using System.Text.RegularExpressions;

namespace BlazorSPATemplator.Middleware;



public class HtmlOptimizationMiddleware {
    private readonly RequestDelegate _next;
    private readonly string _minimalCss;



    public HtmlOptimizationMiddleware( RequestDelegate next, IWebHostEnvironment env ) {
        _next = next;
        
        // Load minimal CSS from file
        string cssPath = Path.Combine( env.WebRootPath, "css", "minimal-bootstrap.css" );
        _minimalCss = File.Exists(cssPath)
            ? File.ReadAllText(cssPath)
            : string.Empty;
    }

    public async Task InvokeAsync( HttpContext context ) {
        // Only process HTML responses in production
        if( context.Request.Path.StartsWithSegments("/api") || !context.Request.Path.HasValue ) {
            await _next(context);
            return;
        }

        Stream originalBodyStream = context.Response.Body;

        using var memoryStream = new MemoryStream();

        context.Response.Body = memoryStream;

        // Apply other rendering work into our fresh stream, first
        await _next( context );

        // Rewind
        memoryStream.Position = 0;

        if( context.Response.ContentType?.Contains("text/html") == true ) {

            using var reader = new StreamReader( memoryStream );

            string html = await reader.ReadToEndAsync();

            // Process HTML
            // html = this.RemoveScriptTags( html );
            // html = this.RemoveExternalResourceLinks( html );
            html = this.InlineMinimalCss( html );

            // Write processed HTML
            var processedBytes = Encoding.UTF8.GetBytes( html );
            context.Response.ContentLength = processedBytes.Length;

            await originalBodyStream.WriteAsync( processedBytes );
        } else {
            // Not HTML, just copy as-is
            await memoryStream.CopyToAsync( originalBodyStream );
        }

        context.Response.Body = originalBodyStream;
    }


    private string RemoveScriptTags( string html ) {
        // Remove all <script> tags and their content
        return Regex.Replace(html, @"<script[^>]*>.*?</script>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }

    private string RemoveExternalResourceLinks( string html ) {
        // Remove external stylesheet links (bootstrap, etc.)
        html = Regex.Replace(html, @"<link[^>]*href=[""']https?://[^""']*[""'][^>]*>", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"<link[^>]*rel=[""']stylesheet[""'][^>]*href=[""']https?://[^""']*[""'][^>]*>", "", RegexOptions.IgnoreCase);
        
        return html;
    }

    private string InlineMinimalCss( string html ) {
        if( string.IsNullOrEmpty(_minimalCss) ) {
            return html;
        }

        // Inject minimal CSS as inline <style> in <head>
        var styleTag = $"<style>{_minimalCss}</style>";
        return Regex.Replace(
            input: html,
            pattern: @"</head>",
            replacement: styleTag + "</head>",
            options: RegexOptions.IgnoreCase
        );
    }
}

