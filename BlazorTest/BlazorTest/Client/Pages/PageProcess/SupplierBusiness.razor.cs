using Blazored.LocalStorage;
using BlazorTest.Client.Utils;
using BlazorTest.Shared.CustomException;
using BlazorTest.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BlazorTest.Client.Pages.PageProcess
{
    public class SupplierBusiness : ComponentBase
    {

        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }

        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }

        [Inject]
        protected ISyncLocalStorageService LocalStorageSync { get; set; }

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        IJSRuntime jsRuntime { get; set; }



        protected List<SupplierDto> SupplierList;

        protected override async Task OnInitializedAsync()
        {
            await ReLoadList();
        }

        public void GoCreateSupplier()
        {
            UrlNavigationManager.NavigateTo("/suppliers/add");
        }

        public void GoEditOrder(Guid SupplierId)
        {
            UrlNavigationManager.NavigateTo("/suppliers/edit/" + SupplierId.ToString());
        }

        public async Task ReLoadList()
        {
            var res = await Http.GetFromJsonAsync<ServiceResponse<List<SupplierDto>>>($"api/Supplier/Suppliers");

            SupplierList = res.Success && res.Data != null ? res.Data : new List<SupplierDto>();
        }

        public async Task DeleteSupplier(Guid SupplierId)
        {
            var modalRes = await ModalManager.ConfirmationAsync("Confirm", "Supplier will be deleted. Are you sure?");
            if (!modalRes)
                return;

            try
            {
                var res = await Http.PostGetBaseResponseAsync("api/Supplier/DeleteSupplier", SupplierId);

                if (res.Success)
                {
                    SupplierList.RemoveAll(i => i.Id == SupplierId);
                    //await loadList();
                }
            }
            catch (ApiException ex)
            {
                await ModalManager.ShowMessageAsync("Error", ex.Message);
            }
        }

        public async void GoWebUrl(Uri Url)
        {
            await jsRuntime.InvokeAsync<object>("open", Url.ToString(), "_blank");
        }
    }
}
