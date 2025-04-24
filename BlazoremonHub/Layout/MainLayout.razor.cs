using Microsoft.AspNetCore.Components;

namespace BlazoremonHub.Layout;

public partial class MainLayout : LayoutComponentBase
{
    bool _drawerOpen = true;

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}
