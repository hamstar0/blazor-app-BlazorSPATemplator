namespace BlazorSPATemplator.Data;


public static class MarkdownContentSource {
    public class Entry( string id, string titleMarkdown, string contentMarkdown ) {
        public string Id { get; } = id;

        public string Title { get; } = titleMarkdown;

        public string Content { get; } = contentMarkdown;
    }



    public static IReadOnlyList<MarkdownContentSource.Entry> Entries { get; } = new[] {
        new MarkdownContentSource.Entry(
            "home",
            "Home",
@"Welcome to the wiki-style SPA. Use the sidebar links or embedded links to navigate between pages.

Try visiting [section-2](#section-2) or [section-3](#section-3)."
        ),
        new MarkdownContentSource.Entry(
            "section-2",
            "Section Two",
@"This page is generated from text stored in a shared data structure.

Navigate to [Home](#home) or [Section Three](#section-3)."
        ),
        new MarkdownContentSource.Entry(
            "section-3",
            "Section Three",
@"All navigation in this app uses hash-based fragment links to show and hide sections.
Return to [Home](#home) or go to [Section Two](#section-2)."
        ),
    };
}

