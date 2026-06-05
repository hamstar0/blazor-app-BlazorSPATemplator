using BlazorSPATemplator.Services;
using Microsoft.AspNetCore.Components;


namespace BlazorSPATemplator.Components.Application;



public partial class MarkdownMultiNavMenuRender : ComponentBase {
    [Inject]
    private MarkdownMarkupSupplier Loader { get; set; } = null!;
}

