using BlazorTest.Shared.CustomException;
using BlazorTest.Shared.Dto;
using System.Net.Http.Json;
using System.Net.Mail;

namespace BlazorTest.Client.Utils
{
    public static class HttpClientExtension
    {
        public async static Task<TResult> PostGetServiceResponseAsync<TResult, TValue>(this HttpClient Client, String Url, TValue Value, bool ThrowSuccessException = false)
        {
            var httpRes = await Client.PostAsJsonAsync(Url, Value);

            if (httpRes.IsSuccessStatusCode)
            {
                var res = await httpRes.Content.ReadFromJsonAsync<ServiceResponse<TResult>>();

                return !res.Success && ThrowSuccessException ? throw new ApiException(res.Message) : res.Data;
            }

            throw new HttpException(httpRes.StatusCode.ToString());
        }

        public async static Task<BaseResponse> PostGetBaseResponseAsync<TValue>(this HttpClient Client, String Url, TValue Value, bool ThrowSuccessException = false)
        {
            var httpRes = await Client.PostAsJsonAsync(Url, Value);

            if (httpRes.IsSuccessStatusCode)
            {
                var res = await httpRes.Content.ReadFromJsonAsync<BaseResponse>();

                return !res.Success && ThrowSuccessException ? throw new ApiException(res.Message) : res;
            }

            throw new HttpException(httpRes.StatusCode.ToString());
        }


        public async static Task<T> GetServiceResponseAsync<T>(this HttpClient Client, String Url, bool ThrowSuccessException = false)
        {
            Console.WriteLine(Url);
            var httpRes = await Client.GetFromJsonAsync<ServiceResponse<T>>(Url);
            Console.WriteLine($"response {httpRes.Data}");
            return !httpRes.Success && ThrowSuccessException ? throw new ApiException(httpRes.Message) : httpRes.Data;
        }
    }
}
