using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorTest.Client.CustomComponents.ModalComponents;

namespace BlazorTest.Client.Utils
{
    public class ModalManager
    {
        private readonly IModalService _modalService;
        public ModalManager(IModalService modalService)
        {
            _modalService = modalService;
        }
        
        public async Task ShowMessageAsync(string Title,string Message,int duration = 0)
        {
            ModalParameters mParams = new ModalParameters();
            mParams.Add("Message", Message);
            var modalRef = _modalService.Show<ShowMessagePopupComponent>(Title, mParams);

            if (duration > 0)
            {
                await Task.Delay(duration);
                modalRef.Close();
            }

            await modalRef.Result;
        }

        public async Task<bool> ConfirmationAsync(string Title, string Message)
        {
            ModalParameters mParams = new ModalParameters();
            mParams.Add("Message", Message);

            var modalRef = _modalService.Show<ShowMessagePopupComponent>(Title,mParams);
            var modalResult = await modalRef.Result;
            return !modalResult.Cancelled;
        }

    }
}
