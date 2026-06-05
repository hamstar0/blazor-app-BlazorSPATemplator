using BlazorSPATemplator.Data;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace BlazorSPATemplator.Services;


public class MarkdownMarkupSupplier {
    public class Entry( string id, MarkupString title, MarkupString content ) {
        public string Id { get; } = id;

        public MarkupString Title { get; } = title;
        
        public MarkupString Content { get; } = content;
    }



    private IEnumerable<Entry> ProcessedContentEntries = null!;



    public MarkdownMarkupSupplier() {
        // Define the path to your server-side markdown file
        // string filePath = Path.Combine( this.Env.WebRootPath, this.MarkdownFile );   //ex. "content.md"
        //
        // if( File.Exists(filePath) ) {
        //    string markdownContent = await File.ReadAllTextAsync( filePath );
        //    ...
        // }
        // else {
        //     this.RenderedHtml = (MarkupString)"<p style='color:red;'>Error: Markdown file not found.</p>";
        // }

        this.ProcessedContentEntries = this.ProcessContentEntries();
    }

    private IEnumerable<Entry> ProcessContentEntries() {
        // Configure Markdig pipeline for advanced features like tables, auto-links, etc.
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        foreach( MarkdownContentSource.Entry entry in MarkdownContentSource.Entries ) {
            // Convert raw markdown text to raw HTML text
            string htmlContent = Markdown.ToHtml( entry.Content, pipeline );

            // Cast the string to MarkupString so Blazor parses the HTML elements
            yield return new Entry( entry.Id, (MarkupString)entry.Title, (MarkupString)htmlContent );
        }
    }


    public IEnumerable<Entry> GetContentEntries() {
        return this.ProcessedContentEntries.ToArray();
    }
}

