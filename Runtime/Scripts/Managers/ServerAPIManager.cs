using System;
using System.Collections.Generic;
using System.Threading;
using LCHFramework.Data;
using LCHFramework.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LCHFramework.Managers
{
    public class ServerAPIResult
    {
        public ServerAPIResult(bool isSuccess, string error) { IsSuccess = isSuccess; Error = error; }

        public bool IsSuccess { get; }
        public string Error { get; }
    }
    
    public class ServerAPIResult<T> : ServerAPIResult
    {
        public ServerAPIResult(bool isSuccess, string error) : base(isSuccess, error) => Value = default;
        
        public ServerAPIResult(bool isSuccess, string error, T value) : base(isSuccess, error) => Value = value;
        
        public T Value { get; }
    }
    
    public static class ServerAPIManager
    {
        public const int RetryCount = 3;
        public static readonly Color LogColor = Color.cyan;
        
        
        
        public static Awaitable<ServerAPIResult<byte[]>> DownloadFileAsync(Uri uri, string contentType, IEnumerable<KeyValuePair<string, string>> header = null, Action<float> progress = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) => DownloadDataAsync<ServerAPIResult<byte[]>>(uri, contentType, header, DownloadHandlerType.File, progress, timeout, retryCount, cancellationToken);
        
        public static Awaitable<ServerAPIResult<Texture2D>> DownloadTexture2DAsync(Uri uri, string contentType, IEnumerable<KeyValuePair<string, string>> header = null, Action<float> progress = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) => DownloadDataAsync<ServerAPIResult<Texture2D>>(uri, contentType, header, DownloadHandlerType.Texture, progress, timeout, retryCount, cancellationToken);
        
        public static Awaitable<ServerAPIResult<AudioClip>> DownloadAudioClipAsync(Uri uri, string contentType, IEnumerable<KeyValuePair<string, string>> header = null, Action<float> progress = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) => DownloadDataAsync<ServerAPIResult<AudioClip>>(uri, contentType, header, DownloadHandlerType.AudioClip, progress, timeout, retryCount, cancellationToken);
        
        private static Awaitable<T> DownloadDataAsync<T>(Uri uri, string contentType, IEnumerable<KeyValuePair<string, string>> header, DownloadHandlerType downloadHandlerType, Action<float> progress, int timeout, int retryCount, CancellationToken cancellationToken) where T : ServerAPIResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(DownloadDataAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Get(uri);
            request.SetRequestHeader(ServerAPIData.ContentTypeName, contentType);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, null, (downloadHandlerType, progress), cancellationToken);
        
        public static Awaitable<T> UploadFileAsync<T>(Uri uri, List<IMultipartFormSection> multipartFormSections, IEnumerable<KeyValuePair<string, string>> header = null, Action<float> progress = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerAPIResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(UploadFileAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Post(uri, multipartFormSections);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, progress, cancellationToken);
        
        public static  Awaitable<T> UploadFileAsync<T>(Uri uri, byte[] data, IEnumerable<KeyValuePair<string, string>> header = null, Action<float> progress = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerAPIResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(UploadFileAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Put(uri, data);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, progress, cancellationToken);
        
        public static Awaitable<T> GetAsync<T>(Uri uri, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerAPIResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(GetAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Get(uri);
            request.SetRequestHeader(ServerAPIData.ContentTypeName, ServerAPIData.ContentTypeValue.ApplicationJson);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, cancellationToken);
        
        public static Awaitable<T> PostAsync<T>(Uri uri, string data, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerAPIResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"{nameof(PostAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Post(uri, data, ServerAPIData.ContentTypeValue.ApplicationJson);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, cancellationToken);
        
        public static Awaitable<T> PutAsync<T>(Uri uri, string data, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerAPIResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(PutAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Put(uri, data);
            request.uploadHandler.contentType = ServerAPIData.ContentTypeValue.ApplicationJson;
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, cancellationToken);
        
        public static Awaitable<T> DeleteAsync<T>(Uri uri, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerAPIResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(DeleteAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Delete(uri);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, cancellationToken);

        private static Awaitable<T> SendRequestAsync<T>(Func<UnityWebRequest> getRequest, int retryCount, CancellationToken cancellationToken) where T : ServerAPIResult => SendRequestAsync<T>(getRequest, retryCount, null, cancellationToken);

        private static Awaitable<T> SendRequestAsync<T>(Func<UnityWebRequest> getRequest, int retryCount, Action<float> upload, CancellationToken cancellationToken) where T : ServerAPIResult => SendRequestAsync<T>(getRequest, retryCount, upload, (DownloadHandlerType.Json, null), cancellationToken);
        
        private static async Awaitable<T> SendRequestAsync<T>(Func<UnityWebRequest> getRequest, int retryCount, Action<float> upload, (DownloadHandlerType, Action<float>) download, CancellationToken cancellationToken) where T : ServerAPIResult
        {
            var isSuccess = false;
            var error = string.Empty;
            var valueIsRequired = typeof(T) != typeof(ServerAPIResult);
            object value = null;
            try
            {
                for (var i = 0; i < retryCount && !isSuccess; i++)
                {
                    using var request = getRequest.Invoke();
                    var sendWebRequest = request.SendWebRequest();
                    await using var registration = cancellationToken.Register(request.Abort);

                    while (cancellationToken is { IsCancellationRequested: false } && !sendWebRequest.isDone)
                    {
                        upload?.Invoke(request.uploadProgress);
                        download.Item2?.Invoke(request.downloadProgress);
                        await Awaitable.NextFrameAsync(cancellationToken).SuppressCancellationThrow();
                    }

                    isSuccess = request.result == UnityWebRequest.Result.Success;
                    error = request.error;
                    if (!isSuccess)
                        UnityEngine.Debug.LogError($"Response {i + 1} of {retryCount}: {request.result}, {request.error}");
                    else
                    {
                        Debug.Log($"Response Success.{(string.IsNullOrEmpty(request.downloadHandler.text) && request.downloadHandler.data.IsEmpty() ? "" : $"\nText: {request.downloadHandler.text}\nData: {string.Join("", request.downloadHandler.data)}")}", LogColor);
                        value = !valueIsRequired ? null
                            : download.Item1 == DownloadHandlerType.Json ? JsonConvert.DeserializeObject<T>(request.downloadHandler.text)
                            : download.Item1 == DownloadHandlerType.File ? request.downloadHandler.data
                            : download.Item1 == DownloadHandlerType.Texture ? DownloadHandlerTexture.GetContent(request)
                            : download.Item1 == DownloadHandlerType.AudioClip ? DownloadHandlerAudioClip.GetContent(request)
                            : throw new ArgumentOutOfRangeException(nameof(download.Item1), download.Item1, null);
                    }
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
                isSuccess = false;
                error = $"{e}";
            }
            
            return (T)(!valueIsRequired ? Activator.CreateInstance(typeof(T), isSuccess, error) : Activator.CreateInstance(typeof(T), isSuccess, error, value));
        }
        
        
        
        private enum DownloadHandlerType
        {
            Json = 0,
            File,
            Texture,
            AudioClip,
        }
    }
}