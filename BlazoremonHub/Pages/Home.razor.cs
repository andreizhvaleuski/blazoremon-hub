using System.Collections.Immutable;
using Microsoft.AspNetCore.Components;
using Refit;

namespace BlazoremonHub.Pages;

public partial class Home : ComponentBase
{
    private readonly IPokeApi _pokeApi;
    private bool _isLoading = true;
    private IImmutableList<string> _pokemons;
    private int _pages;
    private int _selectedPage;

    private IImmutableList<int> _pageSizes = new[] { 10, 25, 50 }.ToImmutableList();
    private int _pageSize = 10;

    public int CurrentPage
    {
        get;
        set
        {
            field = value;
            LoadDataAsync();
        }
    } = 1;

    public int PageSize
    {
        get;
        set
        {
            field = value;
            LoadDataAsync();
        }
    } = 1;

    public Home(IPokeApi pokeApi)
    {
        this._pokeApi = pokeApi ?? throw new ArgumentNullException(nameof(pokeApi));
    }

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        var pokemons = await _pokeApi.GetPokemons(limit: _pageSize, offset: _pageSize * _selectedPage);

        _pages = pokemons.Count / _pageSize;

        _isLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;
        var pokemons = await _pokeApi.GetPokemons(limit: _pageSize, offset: _pageSize * _selectedPage);

        _pages = pokemons.Count / _pageSize;

        _isLoading = false;
    }

    private Task LoadDataAsync()
    {
        return Task.CompletedTask;
    }
}

public interface IPokeApi
{
    [Get("/api/v2/pokemon")]
    Task<NamedApiResourceList> GetPokemons([Query] int limit = 20, [Query] int offset = 0);
}

public record class NamedApiResourceList(int Count, string Next, string Previous, IImmutableList<NamedApiResource> Results);

public record class NamedApiResource(string Name, string Url);
