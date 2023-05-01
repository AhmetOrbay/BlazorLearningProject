using Blazored.LocalStorage;
using BlazorTest.Client.Utils;
using BlazorTest.Shared.CustomException;
using BlazorTest.Shared.Dto;
using BlazorTest.Shared.FilterModels;
using Microsoft.AspNetCore.Components;

namespace BlazorTest.Client.Pages.PageProcess
{
    public class OrderBusiness : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }

        [Inject]
        protected ISyncLocalStorageService LocalStorageSync { get; set; }


        [Inject]
        ModalManager ModalManager { get; set; }

        public OrderListFilterModel filterModel = new OrderListFilterModel() 
        { CreateDateFirst = DateTime.UtcNow.Date.AddDays(-1), 
            CreateDateLast = DateTime.UtcNow.Date.AddDays(-1)};

        protected List<OrderDto> OrderList = new();

        internal bool loading;

        protected override async Task OnInitializedAsync()
        {
            await ReLoadList();
        }

        protected String GetRemaningDateStr(DateTime ExpireDate)
        {
            TimeSpan ts = ExpireDate.Subtract(DateTime.Now);

            return ts.TotalSeconds >= 0 ? $"{ts.Hours}:{ts.Minutes}:{ts.Seconds}" : "00:00:00";
        }

        public void GoDetails(Guid SelectedOrderId)
        {
            NavigationManager.NavigateTo("/orders-items/" + SelectedOrderId.ToString());
        }


        public void GoCreateOrder()
        {
            NavigationManager.NavigateTo("/orders/add");
        }

        public void GoEditOrder(Guid OrderId)
        {
            NavigationManager.NavigateTo("/orders/edit/" + OrderId.ToString());
        }

        public async Task ReLoadList()
        {
            loading = true;

            try
            {
                OrderList = await Http.PostGetServiceResponseAsync<List<OrderDto>, OrderListFilterModel>("api/Order/OrdersByFilter", filterModel, true);
            }
            catch (ApiException ex)
            {
                await ModalManager.ShowMessageAsync("List Error", ex.Message);
            }
            finally
            {
                loading = false;
            }
        }

        public bool IsExpired(DateTime ExpireDate)
        {
            TimeSpan ts = ExpireDate.Subtract(DateTime.Now);
            return ts.TotalSeconds < 0;
        }

        public async Task DeleteOrder(Guid OrderId)
        {
            try
            {
                var modalRes = await ModalManager.ConfirmationAsync("Confirm", "Order will be deleted. Are you sure?");
                if (!modalRes)
                    return;

                var res = await Http.GetServiceResponseAsync<BaseResponse>("api/Order/DeleteOrder/" + OrderId, true);

                OrderList.RemoveAll(i => i.Id == OrderId);
            }
            catch (ApiException ex)
            {
                await ModalManager.ShowMessageAsync("Deletion Error", ex.Message);
            }
        }

        //sadece olusturan kullanicinin islem yapmasini kontrol icin yapildi.
        public bool IsMyOrder(Guid CreatedUserId)
        {
            return LocalStorageSync.GetUserIdSync() == CreatedUserId;
        }
    }
}
