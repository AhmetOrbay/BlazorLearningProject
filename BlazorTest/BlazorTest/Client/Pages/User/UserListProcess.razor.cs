using BlazorTest.Client.Utils;
using BlazorTest.Shared.CustomException;
using BlazorTest.Shared.Dto;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Json;

namespace BlazorTest.Client.Pages.User
{
    public class UserListProcess:ComponentBase
    {
        [Inject]
        public HttpClient _client { get; set; }

        protected List<UserDto> userList = new List<UserDto>();


        [Inject]
        ModalManager ModalManager { get; set; }


        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadList();
        }

        ///
        protected void GotoCreateUser()
        {
            NavigationManager.NavigateTo("/users/add");
        }

        protected void GotoEditUser(Guid UserId)
        {
            NavigationManager.NavigateTo($"/users/edit/{UserId}");
        }
        protected async Task DeleteUser(Guid UserId)
        {
            try
            {
                var Url = "api/user/deleteUser";
                var resConfirmation = await ModalManager.ConfirmationAsync("Confirmation", "User will be deleted. Are you sure?");
                if (resConfirmation)
                {
                    var userDeleteResponse = await _client.PostGetServiceResponseAsync<bool, Guid>($"{Url}", UserId, true);
                    await LoadList();
                }
                else return;
            }
            catch (ApiException exApi)
            {
                await ModalManager.ShowMessageAsync("ApiException", exApi.Message);
            }
            catch (Exception ex)
            {
                await ModalManager.ShowMessageAsync("Exception", ex.Message);
            }
        }
        protected async Task LoadList()
        {
            try
            {
                userList = await _client.GetServiceResponseAsync<List<UserDto>>("api/user/users", true);
            }
            catch (ApiException exception)
            {
                await ModalManager.ShowMessageAsync("Api Exception", exception.Message);

            }
            catch (Exception exception)
            {
                await ModalManager.ShowMessageAsync("Exception", exception.Message);

            }

        }

    }
}
