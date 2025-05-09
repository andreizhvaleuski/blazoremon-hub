﻿using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Refit;

namespace BlazoremonHub.Pages;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public partial class Home : ComponentBase
{
    private readonly IImmutableList<int> _pageSizes = new[] { 25, 50, 75, 100 }.ToImmutableList();

    [Inject]
    public IPokeApi PokeApi {  get; set; }

    private int _totalPages;
    private int _currentPage = 1;
    private int _pageSize;
    private RemoteOperationState _operationState;
    private IImmutableList<PokemonModel>? _pokemons;
    private int _totalPokemons;

    private int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage == value)
            {
                return;
            }

            _currentPage = value;

            LoadPokemonsAsync();
        }
    }

    private int PageSize
    {
        get => _pageSize;
        set
        {
            if (_pageSize == value)
            {
                return;
            }

            _pageSize = value;
            _currentPage = 1;

            LoadPokemonsAsync();
        }
    }

    public Home()
    {
        _pageSize = _pageSizes[0];
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadPokemonsAsync();
    }

    private async Task LoadPokemonsAsync()
    {
        _operationState = RemoteOperationState.InProgress;
        StateHasChanged();

        try
        {
            // Intentional delay.
            await Task.Delay(1000);

            var limit = _pageSize;
            var offset = _pageSize * (_currentPage - 1);

            var response = await PokeApi.GetPokemons(limit: limit, offset: offset);

            _totalPages = (int)Math.Ceiling((double)response.Count / _pageSize);

            _pokemons = response.Results
                .Select(result => new PokemonModel(result.Name, result.Url))
                .ToImmutableList();

            _totalPokemons = response.Count;

            _operationState = RemoteOperationState.Succeeded;
        }
        catch (Exception exception)
        {
            _operationState = RemoteOperationState.Failed;
        }
        finally
        {
            StateHasChanged();
        }
    }
}

public record class PokemonModel(string Name, string Url);

public enum RemoteOperationState
{
    InProgress = 0,
    Succeeded = 1,
    Failed = 2
}

public interface IPokeApi
{
    [Get("/api/v2/pokemon")]
    Task<NamedApiResourceList> GetPokemons([Query] int limit = 20, [Query] int offset = 0);
}

public record class NamedApiResourceList(int Count, string Next, string Previous, IEnumerable<NamedApiResource> Results);

public record class NamedApiResource(string Name, string Url);
