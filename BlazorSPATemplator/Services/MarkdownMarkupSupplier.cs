using Markdig;
using Microsoft.AspNetCore.Components;

namespace BlazorSPATemplator.Services;


public class MarkdownMarkupSupplier {
    public class Entry( string id, long sortWeight, MarkupString title, MarkupString content ) {
        public string Id { get; } = id;

        public long SortWeight { get; } = sortWeight;

        public MarkupString Title { get; } = title;
        
        public MarkupString Content { get; } = content;
    }



    private IEnumerable<Entry> _ProcessedContentEntries = null!;



    public MarkdownMarkupSupplier( MarkdownFileSource fileSource ) {
        IEnumerable<string> fileData = fileSource.GetFileData();

        this._ProcessedContentEntries = this.ProcessContentEntries( fileData )
            .OrderBy( e => e.SortWeight )
            .OrderBy( e => e.Title.ToString() );
    }

    private IEnumerable<Entry> ProcessContentEntries( IEnumerable<string> markdownEntries ) {
        // Configure Markdig pipeline for advanced features like tables, auto-links, etc.
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        foreach( string content in markdownEntries ) {
            string[] lines = content.Split( '\n' );

            // Line 1: Hash id (sans hash)
            string id = lines[0].Trim();
            // Line 2: Sort weight
            long weight = int.Parse( lines[1].Trim() );
            // Line 3: Title
            string titleRaw = lines[2];
            // Lines: Content
            string contentRaw = string.Join( '\n', lines.TakeLast(lines.Length - 3) );

            // Convert raw markdown text to raw HTML text
            string titleHtml = Markdown.ToHtml( titleRaw, pipeline );
            string contentHtml = Markdown.ToHtml( contentRaw, pipeline );

            // Cast the string to MarkupString so Blazor parses the HTML elements
            yield return new Entry( id, weight, (MarkupString)titleHtml, (MarkupString)contentHtml );
        }
    }


    public IEnumerable<Entry> GetContentEntries() {
        return this._ProcessedContentEntries.ToArray();
    }
}

