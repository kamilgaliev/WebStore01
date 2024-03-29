﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected string Address { get; }

        protected HttpClient Http { get; }

        protected BaseClient(IConfiguration Configuration, string ServiceAddress)
        {
            Address = ServiceAddress;

            Http = new HttpClient 
            { 
                BaseAddress = new Uri(Configuration["WebApiURL"]),
                DefaultRequestHeaders =
                {
                    Accept = {new MediaTypeWithQualityHeaderValue("application/json")}
                }
            };
        }

        protected T Get<T> (string url) => GetAsync<T> (url).Result;

        protected async Task<T> GetAsync<T>( string url)
        {
            var response = await Http.GetAsync(url);

            return await response.EnsureSuccessStatusCode().Content.ReadAsAsync<T>();
        }

        protected HttpResponseMessage Post<T> (string url, T item) => PostAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PostAsync<T> (string url, T item) 
        {
            var response = await Http.PostAsJsonAsync(url, item);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            var response = await Http.PutAsJsonAsync(url, item);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

        protected async Task<HttpResponseMessage> DeleteAsync (string url)
        {
            var response = await Http.DeleteAsync(url);

            return response;
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this); // при наличии ~BaseClient() => Dispose(false);
        }

        //~BaseClient() => Dispose(false);

        private bool _Disposed;
        protected virtual void Dispose(bool disposing)
        {
            if(_Disposed) return;

            if(disposing)
            {
                //очистка управляемых ресурсов
                Http.Dispose();
            }
            //очистка неуправляемых ресурсов
            _Disposed = true;
        }
    }
}
