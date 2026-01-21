using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LCHFramework.Data;
using LCHFramework.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LCHFramework.Managers
{
    public class ServerAPIResult
    {
        public ServerAPIResult(Exception exception) => Exception = exception;
        
        public Exception Exception { get; }
        
        public bool IsSuccess => Exception == null;
    }
    
    public class ServerAPIResult<T> : ServerAPIResult
    {
        public ServerAPIResult(T value, Exception exception) : base(exception) => Value = value;
        
        public T Value { get; }
    }
    
    public static class ServerAPIManager
    {
        public static readonly Color LogColor = Color.cyan;
        
        
        
        public static async Awaitable<ServerAPIResult<Texture>> DownloadTextureAsync(Uri uri, Action<float> progress = null, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
        {
            Debug.Log($"DownloadTextureAsync: {uri}", LogColor);
            return await _DownloadFileAsync<Texture>(uri, DownloadHandlerType.Texture, progress, header, timeout, retryCount, cancellationToken);
        }
        
        public static async Awaitable<ServerAPIResult<AudioClip>> DownloadAudioClipAsync(Uri uri, Action<float> progress = null, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
        {
            Debug.Log($"DownloadAudioClipAsync: {uri}", LogColor);
            return await _DownloadFileAsync<AudioClip>(uri, DownloadHandlerType.AudioClip, progress, header, timeout, retryCount, cancellationToken);
        }
        
        public static async Awaitable<ServerAPIResult<byte[]>> DownloadFileAsync(Uri uri, Action<float> progress = null, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
        {
            Debug.Log($"DownloadFileAsync: {uri}", LogColor);
            return await _DownloadFileAsync<byte[]>(uri, DownloadHandlerType.Data, progress, header, timeout, retryCount, cancellationToken);
        }
        
        private static async Awaitable<ServerAPIResult<T>> _DownloadFileAsync<T>(Uri uri, DownloadHandlerType downloadHandlerType, Action<float> progress = null, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
        {
            using var request = UnityWebRequest.Get(uri);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            
            return await SendRequestAsync<T>(request, retryCount, downloadHandlerType: downloadHandlerType, downloadProgress: progress, cancellationToken: cancellationToken);
        }
        
        public static async Awaitable<ServerAPIResult> UploadFileAsync(Uri uri, List<IMultipartFormSection> multipartFormSections, Action<float> progress = null, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
        {
            Debug.Log($"UploadFileAsync: {uri}", LogColor);
            
            using var request = UnityWebRequest.Post(uri, multipartFormSections);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            
            return await SendRequestAsync(request, retryCount, uploadProgress: progress, cancellationToken: cancellationToken);
        }
        
        
        
        public static async Awaitable<ServerAPIResult<T>> GetAsync<T>(Uri uri, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
        {
            Debug.Log($"GetAsync: {uri}", LogColor);
            
            using var request = UnityWebRequest.Get(uri);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            
            return await SendRequestAsync<T>(request, retryCount, cancellationToken: cancellationToken);
        }
        
        public static async Awaitable<ServerAPIResult> PostAsync(Uri uri, object body, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
            => await PostAsync(uri, body, new[] { ServerAPIData.ContentType.ApplicationJson }, timeout, retryCount, cancellationToken);
        
        public static async Awaitable<ServerAPIResult> PostAsync(Uri uri, object body, IEnumerable<KeyValuePair<string, string>> header, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
        {
            Debug.Log($"PostAsync: {uri}", LogColor);
            
            using var request = UnityWebRequest.PostWwwForm(uri, JsonConvert.SerializeObject(body));
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            
            return await SendRequestAsync(request, retryCount, cancellationToken: cancellationToken);
        }
        
        public static async Awaitable<ServerAPIResult> PutAsync(Uri uri, object body, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
            => await PutAsync(uri, body, new[] { ServerAPIData.ContentType.ApplicationJson }, timeout, retryCount, cancellationToken);
        
        public static async Awaitable<ServerAPIResult> PutAsync(Uri uri, object body, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
        {
            Debug.Log($"PutAsync: {uri}", LogColor);
            
            using var request = UnityWebRequest.Put(uri, JsonConvert.SerializeObject(body));
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            
            return await SendRequestAsync(request, retryCount, cancellationToken: cancellationToken);
        }
        
        public static async Awaitable<ServerAPIResult> DeleteAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> header = null, int timeout = 0, int retryCount = 2, CancellationToken cancellationToken = default)
        {
            Debug.Log($"DeleteAsync: {uri}", LogColor);
            
            using var request = UnityWebRequest.Delete(uri);
            header?.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
            request.timeout = timeout;
            
            return await SendRequestAsync(request, retryCount, cancellationToken: cancellationToken);
        }
        
        
        
        private static async Awaitable<ServerAPIResult<T>> SendRequestAsync<T>(UnityWebRequest request, int retryCount = 2, DownloadHandlerType downloadHandlerType = DownloadHandlerType.Text, Action<float> uploadProgress = null, Action<float> downloadProgress = null, CancellationToken cancellationToken = default)
        {
            var sendRequestAsync = await SendRequestAsync(request, retryCount, downloadHandlerType, uploadProgress, downloadProgress, cancellationToken);
            var value = sendRequestAsync.IsSuccess && downloadHandlerType == DownloadHandlerType.Text ? JsonConvert.DeserializeObject<T>(request.downloadHandler.text)
                : sendRequestAsync.IsSuccess && downloadHandlerType == DownloadHandlerType.Data ? (T)(object)request.downloadHandler.data
                : sendRequestAsync.IsSuccess && downloadHandlerType == DownloadHandlerType.Texture ? (T)(object)DownloadHandlerTexture.GetContent(request)
                : sendRequestAsync.IsSuccess && downloadHandlerType == DownloadHandlerType.AudioClip ? (T)(object)DownloadHandlerAudioClip.GetContent(request)
                : default;
            return new ServerAPIResult<T>(value, sendRequestAsync.Exception);
        }
        
        private static async Awaitable<ServerAPIResult> SendRequestAsync(UnityWebRequest request, int retryCount = 2, DownloadHandlerType downloadHandlerType = DownloadHandlerType.Text, Action<float> uploadProgress = null, Action<float> downloadProgress = null, CancellationToken cancellationToken = default)
        {
            try
            {
                for (var i = 0; i < retryCount; i++)
                {
                    var sendWebRequest = request.SendWebRequest();
                    await using var registration = cancellationToken.Register(request.Abort);
                    
                    while (!sendWebRequest.isDone)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        uploadProgress?.Invoke(request.uploadProgress);
                        downloadProgress?.Invoke(request.downloadProgress);
                        await Task.Yield();
                    }
                    
                    if (request.result != UnityWebRequest.Result.Success)
                        UnityEngine.Debug.LogError($"Request ({i + 1}/{retryCount}) {request.result}: {request.error}");
                    else
                    {
                        Debug.Log($"Request Success: {(downloadHandlerType == DownloadHandlerType.Text ? $"text: {request.downloadHandler.text}" : $"data: {string.Join("", request.downloadHandler.data)}", LogColor)}");
                        return new ServerAPIResult(null);
                    }
                }
                
                return new ServerAPIResult(new Exception($"Request {request.result}: {request.error}"));
            }
            catch (OperationCanceledException e)
            {
                UnityEngine.Debug.LogError($"Request Canceled: {e}");
                return new ServerAPIResult(e);
            }
            catch (UnityException e)
            {
                UnityEngine.Debug.LogError($"Request Unity exception: {e}");
                return new ServerAPIResult(e);
            }
            catch (InvalidOperationException e)
            {
                UnityEngine.Debug.LogError($"Request Invalid operation exception: {e}");
                return new ServerAPIResult(e);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Request Unexpected exception: {e}");
                return new ServerAPIResult(e);
            }
        }
        
        
        
        private enum DownloadHandlerType
        {
            Text = 0,
            Data,
            Texture,
            AudioClip,
        }
    }
}