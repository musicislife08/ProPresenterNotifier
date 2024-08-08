using MudBlazor;
using ProPresenterNotifier.Components;
using MudBlazor.Services;
using ProPresenter.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProPresenterServices(builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddMudBlazorSnackbar(options =>
{
    options.PositionClass = Defaults.Classes.Position.BottomCenter;
    options.SnackbarVariant = Variant.Filled;
    options.PreventDuplicates = false;
    options.MaxDisplayedSnackbars = 10;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();