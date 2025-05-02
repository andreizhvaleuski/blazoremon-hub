using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazoremonHub.Layout;

public partial class MainLayout : LayoutComponentBase
{
    private bool _isDarkMode;
    private MudThemeProvider _mudThemeProvider = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _mudThemeProvider.WatchSystemPreference(OnSystemPreferenceChanged);
            StateHasChanged();
        }
    }

    private Task OnSystemPreferenceChanged(bool isDarkMode)
    {
        _isDarkMode = isDarkMode;
        StateHasChanged();
        return Task.CompletedTask;
    }
}