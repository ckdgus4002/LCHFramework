using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using LCHFramework.Data;
using LCHFramework.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LCHFramework.Managers
{
    public static class ServerApiManager
    {
        public const int RetryCount = 3;
        public static readonly Color LogColor = Color.cyan;
        
        
        
        public static Awaitable<T> UploadFileAsync<T>(Uri uri, List<IMultipartFormSection> multipartFormSections, IEnumerable<KeyValuePair<string, string>> header = null, Action<float> progress = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerApiResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(UploadFileAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Post(uri, multipartFormSections);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, progress, cancellationToken);
        
        public static Awaitable<T> UploadFileAsync<T>(Uri uri, byte[] data, IEnumerable<KeyValuePair<string, string>> header = null, Action<float> progress = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerApiResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(UploadFileAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Put(uri, data);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, progress, cancellationToken);
        
        public static Awaitable<T> GetAsync<T>(Uri uri, IEnumerable<KeyValuePair<string, string>> header = null, DownloadHandlerType downloadHandlerType = DownloadHandlerType.Json, Action<float> downloadProgress = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerApiResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(GetAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Get(uri);
            request.SetRequestHeader(ServerApiData.ContentType, ServerApiData.ContentTypeValue.ApplicationJson);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, (downloadHandlerType, downloadProgress), cancellationToken);
        
        public static Awaitable<T> PostAsync<T>(Uri uri, string data, IEnumerable<KeyValuePair<string, string>> header = null, DownloadHandlerType downloadHandlerType = DownloadHandlerType.Json, Action<float> downloadProgress = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerApiResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(PostAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Post(uri, data, ServerApiData.ContentTypeValue.ApplicationJson);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, (downloadHandlerType, downloadProgress), cancellationToken);
        
        public static Awaitable<T> PutAsync<T>(Uri uri, string data, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) where T : ServerApiResult => SendRequestAsync<T>(() =>
        {
            Debug.Log($"Request {nameof(PutAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Put(uri, data);
            request.uploadHandler.contentType = ServerApiData.ContentTypeValue.ApplicationJson;
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, cancellationToken);
        
        public static Awaitable<ServerApiResult> DeleteAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = RetryCount, CancellationToken cancellationToken = default) => SendRequestAsync<ServerApiResult>(() =>
        {
            Debug.Log($"Request {nameof(DeleteAsync)}: {uri}", LogColor);
            var request = UnityWebRequest.Delete(uri);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            return request;
            
        }, retryCount, cancellationToken);
        
        private static Awaitable<T> SendRequestAsync<T>(Func<UnityWebRequest> getRequest, int retryCount, CancellationToken cancellationToken) where T : ServerApiResult => SendRequestAsync<T>(getRequest, retryCount, null, cancellationToken);
        
        private static Awaitable<T> SendRequestAsync<T>(Func<UnityWebRequest> getRequest, int retryCount, (DownloadHandlerType, Action<float>) download, CancellationToken cancellationToken) where T : ServerApiResult => SendRequestAsync<T>(getRequest, retryCount, null, download, cancellationToken);
        
        private static Awaitable<T> SendRequestAsync<T>(Func<UnityWebRequest> getRequest, int retryCount, Action<float> upload, CancellationToken cancellationToken) where T : ServerApiResult => SendRequestAsync<T>(getRequest, retryCount, upload, (DownloadHandlerType.Json, null), cancellationToken);
        
        private static async Awaitable<T> SendRequestAsync<T>(Func<UnityWebRequest> getRequest, int retryCount, Action<float> upload, (DownloadHandlerType, Action<float>) download, CancellationToken cancellationToken) where T : ServerApiResult
        {
            var isSuccess = false;
            var error = string.Empty;
            var errorResponseText = string.Empty;
            var valueIsRequired = typeof(T) != typeof(ServerApiResult);
            object value = null;
            try
            {
                for (var i = 0; !cancellationToken.IsCancellationRequested && !isSuccess && i < retryCount; i++)
                {
                    using var request = getRequest.Invoke();
                    if (download.Item1 == DownloadHandlerType.Texture) request.downloadHandler = new DownloadHandlerTexture();
                    else if (download.Item1 == DownloadHandlerType.AudioClipWav) request.downloadHandler = new DownloadHandlerAudioClip(request.uri, AudioType.WAV);
                    else if (download.Item1 == DownloadHandlerType.AudioClipMp3) request.downloadHandler = new DownloadHandlerAudioClip(request.uri, AudioType.MPEG);
                    
                    var sendWebRequest = request.SendWebRequest();
                    await using var registration = cancellationToken.Register(request.Abort);
                    
                    while (!cancellationToken.IsCancellationRequested && !sendWebRequest.isDone)
                    {
                        upload?.Invoke(request.uploadProgress);
                        download.Item2?.Invoke(request.downloadProgress);
                        await Awaitable.NextFrameAsync(cancellationToken).SuppressCancellationThrow();
                    }
                    
                    isSuccess = request.result == UnityWebRequest.Result.Success;
                    error = request.error;
                    var isProtocolError = request.result == UnityWebRequest.Result.ProtocolError;
                    var isBreakUnsuccessfully = cancellationToken.IsCancellationRequested || isProtocolError || retryCount - 1 <= i;
                    var requestDownloadHandlerData = request.downloadHandler == null ? Array.Empty<byte>() : request.downloadHandler.data;
                    var requestDownloadHandlerText = request.downloadHandler == null || (download.Item1 != DownloadHandlerType.Json && download.Item1 != DownloadHandlerType.Buffer) ? string.Empty : request.downloadHandler.text;
                    if (!isSuccess && !isBreakUnsuccessfully)
                    {
                        UnityEngine.Debug.Log($"Response {i + 1} of {retryCount}: {request.result}, {request.error}, {requestDownloadHandlerText}");
                        await Awaitable.WaitForSecondsAsync(Mathf.Pow(2f, i), cancellationToken).SuppressCancellationThrow();
                    }
                    else if (!isSuccess && isBreakUnsuccessfully)
                    {
                        errorResponseText = Encoding.UTF8.GetString(requestDownloadHandlerData);
                        break;
                    }
                    else if (!valueIsRequired)
                        Debug.Log("Response Success.", LogColor);
                    else
                    {
                        Debug.Log("Response Success."
                                  + $"\nText: {requestDownloadHandlerText}"
                                  + $"\nData: {requestDownloadHandlerData.Length}", LogColor);
                        value = download.Item1 switch
                        {
                            DownloadHandlerType.Json => JsonConvert.DeserializeObject(requestDownloadHandlerText, typeof(T).GetGenericArguments()[0]),
                            DownloadHandlerType.Buffer => requestDownloadHandlerData,
                            DownloadHandlerType.Texture => DownloadHandlerTexture.GetContent(request),
                            DownloadHandlerType.AudioClipWav => DownloadHandlerAudioClip.GetContent(request),
                            DownloadHandlerType.AudioClipMp3 => DownloadHandlerAudioClip.GetContent(request),
                            _ => throw new ArgumentOutOfRangeException(nameof(download.Item1), download.Item1, null)
                        };
                    }
                }
            }
            catch (Exception e)
            {
                isSuccess = false;
                error = $"{e}";
            }
            
            return (T)(!valueIsRequired ? Activator.CreateInstance(typeof(T), isSuccess, error, errorResponseText) : Activator.CreateInstance(typeof(T), isSuccess, error, errorResponseText, value));
        }
        
        
        
        public enum DownloadHandlerType
        {
            Json = 0,
            Buffer,
            Texture,
            AudioClipWav,
            AudioClipMp3,
        }
    }
}