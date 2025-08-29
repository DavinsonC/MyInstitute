using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyInstitute.SchoolManagement.Shared.Models;
using MyInstitute.SchoolManagement.AccessService.Repositories;
using MyInstitute.SchoolManagement.Shared.EntitiesSoftSec;
using MyInstitute.SchoolManagement.Frontend.Helpers;

namespace MyInstitute.SchoolManagement.FrontEnd.Pages.EntitiesViews.Usuarios;

public partial class IndexUsuarios
{
    private int CurrentPage = 1;
    private int TotalPages;
    private int PageSize = 15;
    private const string baseUrl = "/api/Usuarios";

    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IDialogService _dialogService { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    public List<Usuario>? Usuarios { get; set; }

    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await Cargar();
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await Cargar();
    }

    private async Task SelectedPage(int page)
    {
        CurrentPage = page;
        await Cargar(page);
    }

    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true };
        IDialogReference? dialog;
        if (isEdit)
        {
            var parameters = new DialogParameters
            {
                { "Id", id }
            };
            dialog = await _dialogService.ShowAsync<EditUsuario>($"Editar Usuarios", parameters, options);
        }
        else
        {
            dialog = await _dialogService.ShowAsync<CreateUsuario>($"Nuevo Usuario", options);
        }

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await Cargar();
        }
    }

    private async Task ShowDetailsAsync(int id)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true };
        IDialogReference? dialog;

        var parameters = new DialogParameters
            {
                { "Id", id }
            };
        dialog = await _dialogService.ShowAsync<DetailsUsuario>($"Detalle Usuario", parameters, options);

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await Cargar();
        }
    }

    private async Task Cargar(int page = 1)
    {
        var url = $"{baseUrl}?page={page}&recordsnumber={PageSize}";
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }
        var responseHttp = await _repository.GetAsync<List<Usuario>>(url);
        // Centralizamos el manejo de errores
        bool errorHandled = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandled)
        {
            _navigationManager.NavigateTo("/");
            return;
        }

        Usuarios = responseHttp.Response;
        if (responseHttp.HttpResponseMessage.Headers.TryGetValues("Totalpages", out var valores))
        {
            var valor = valores.FirstOrDefault();
            if (int.TryParse(valor, out var total))
            {
                TotalPages = total;
            }
            else
            {
                TotalPages = 1; // valor por defecto
            }
        }
        else
        {
            TotalPages = 1; // valor por defecto si no existe el header
        }
    }

    private async Task DeleteAsync(int id)
    {
        var result = await _sweetAlert.FireAsync(new SweetAlertOptions
        {
            Title = "Confirmaction",
            Text = "Estas Seguro de Borrar el Registro",
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true
        });

        var confirm = string.IsNullOrEmpty(result.Value);

        if (confirm)
        {
            return;
        }

        var responseHTTP = await _repository.DeleteAsync($"{baseUrl}/{id}");
        // Centralizamos el manejo de errores
        bool errorHandled = await _responseHandler.HandleErrorAsync(responseHTTP);
        if (errorHandled)
        {
            await Cargar();
            return;
        }
        await Cargar();
    }
}