using BlazorSPATemplator.Components;
using BlazorSPATemplator.Services;
using BlazorSPATemplator.Middleware;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();
builder.Services.AddSingleton<MarkdownFileSource>();
builder.Services.AddSingleton<MarkdownMarkupSupplier>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler( "/Error", createScopeForErrors: true );
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Remove scripts and inline minimal CSS
app.UseMiddleware<HtmlOptimizationMiddleware>();

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>();

app.MapFallback(context =>
{
    context.Response.Redirect("/");
    return Task.CompletedTask;
});

app.Run();

//dotnet publish -c Release -r win-x64 --self-contained -o ./publish
