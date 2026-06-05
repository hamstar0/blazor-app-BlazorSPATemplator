using BlazorSPATemplator.Services;
using Microsoft.AspNetCore.Components;


namespace BlazorSPATemplator.Components.Application;



public partial class MarkupMultiRender : ComponentBase {
    [Inject]
    private MarkdownMarkupSupplier Loader { get; set; } = null!;
}

