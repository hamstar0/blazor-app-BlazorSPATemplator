using Markdig;
using Microsoft.AspNetCore.Components;

namespace BlazorSPATemplator.Services;


public class MarkdownMarkupSupplier {
    public class Entry( string id, MarkupString title, MarkupString content ) {
        public string Id { get; } = id;

        public MarkupString Title { get; } = title;
        
        public MarkupString Content { get; } = content;
    }



    private IEnumerable<Entry> _ProcessedContentEntries = null!;



    public MarkdownMarkupSupplier( MarkdownFileSource fileSource ) {
        IEnumerable<MarkdownFileSource.Entry> fileData = fileSource.GetFileData();

        this._ProcessedContentEntries = this.ProcessContentEntries( fileData );
    }

    private IEnumerable<Entry> ProcessContentEntries( IEnumerable<MarkdownFileSource.Entry> markdownEntries ) {
        // Configure Markdig pipeline for advanced features like tables, auto-links, etc.
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        foreach( MarkdownFileSource.Entry entry in markdownEntries ) {
            // Convert raw markdown text to raw HTML text
            string titleHtml = Markdown.ToHtml( entry.Title, pipeline );
            string contentHtml = Markdown.ToHtml( entry.Content, pipeline );

            // Cast the string to MarkupString so Blazor parses the HTML elements
            yield return new Entry( entry.Id, (MarkupString)titleHtml, (MarkupString)contentHtml );
        }
    }


    public IEnumerable<Entry> GetContentEntries() {
        return this._ProcessedContentEntries.ToArray();
    }
}

