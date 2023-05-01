using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTest.Client.Utils
{
    public static class LocalStorageExtension
    {

        public async static Task<Guid> GetUserId(this ILocalStorageService LocalStorage)
        {
            var userGuid = await LocalStorage.GetItemAsStringAsync("UserId");
            if (!string.IsNullOrEmpty(userGuid))
            {
                return new Guid(userGuid);
            }
            return Guid.Empty;
        }

        public static Guid GetUserIdSync(this ISyncLocalStorageService LocalStorage)
        {
            String userGuid = LocalStorage.GetItemAsString("UserId");

            return Guid.TryParse(userGuid, out Guid UserId) ? UserId : Guid.Empty;
        }

        public async static Task<String> GetUserEMail(this ILocalStorageService LocalStorage)
        {
            return await LocalStorage.GetItemAsStringAsync("email");
        }

        public async static Task<String> GetUserFullName(this ILocalStorageService LocalStorage)
        {
            return await LocalStorage.GetItemAsStringAsync("UserFullName");
        }
    }
}
