namespace BlazorSPATemplator.Services;


public class MarkdownFileSource {
//     public static IReadOnlyList<MarkdownFileSource.Entry> Entries { get; } = new[] {
//         new MarkdownFileSource.Entry(
//             "home",
//             "Home",
// @"Welcome to the wiki-style SPA. Use the sidebar links or embedded links to navigate between pages.

// Try visiting [section-2](#section-2) or [section-3](#section-3)."
//         ),
//         new MarkdownFileSource.Entry(
//             "section-2",
//             "Section Two",
// @"This page is generated from text stored in a shared data structure.

// Navigate to [Home](#home) or [Section Three](#section-3)."
//         ),
//         new MarkdownFileSource.Entry(
//             "section-3",
//             "Section Three",
// @"All navigation in this app uses hash-based fragment links to show and hide sections.
// Return to [Home](#home) or go to [Section Two](#section-2)."
//         ),
//     };


    
    private readonly string _BaseDir;



    public MarkdownFileSource( IHostEnvironment env ) {
        this._BaseDir = Path.Combine( env.ContentRootPath, "Content", "Markdown" );
    }

    public IEnumerable<string> GetFileData() {
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

        return Directory
            .EnumerateFiles( this._BaseDir, "*.content.md" )
            .Select( p => File.ReadAllText(p) );
    }
}

