using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LCHFramework.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Managers
{
    public class ServerAPIResult
    {
        public ServerAPIResult(Exception exception) => Exception = exception;
        
        public Exception Exception { get; private set; }
        
        public bool IsSuccess => Exception == null;
    }
    
    public class ServerAPIResult<T> : ServerAPIResult
    {
        public ServerAPIResult(T value, Exception exception) : base(exception) => Value = value;
            
        public T Value { get; private set; }
    }

    public static class ServerAPIManager
    {
        public static async Task<ServerAPIResult<T>> GetAsync<T>(Uri uri, int retryCount = 2, int timeout = 0, Dictionary<string, string> header = null, CancellationToken cancellationToken = default)
        {
            Debug.Log($"GetAsync: {uri}", Color.cyan);

            using var request = UnityWebRequest.Get(uri);
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync<T>(request, false, retryCount, cancellationToken: cancellationToken);
        }

        public static async Task<ServerAPIResult> PostAsync(Uri uri, object body, int retryCount = 2, int timeout = 0, Dictionary<string, string> header = null, CancellationToken cancellationToken = default)
        {
            Debug.Log($"PostAsync: {uri}", Color.cyan);
            
            using var request = UnityWebRequest.PostWwwForm(uri, JsonConvert.SerializeObject(body));
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync(request, false, retryCount, cancellationToken: cancellationToken);
        }

        public static async Task<ServerAPIResult> PutAsync(Uri uri, object body, int retryCount = 2, int timeout = 0, Dictionary<string, string> header = null, CancellationToken cancellationToken = default)
        {
            Debug.Log($"PutAsync: {uri}", Color.cyan);
            
            using var request = UnityWebRequest.Put(uri, JsonConvert.SerializeObject(body));
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync(request, false, retryCount, cancellationToken: cancellationToken);
        }

        public static async Task<ServerAPIResult> DeleteAsync(Uri uri, int retryCount = 2, int timeout = 0, Dictionary<string, string> header = null, CancellationToken cancellationToken = default)
        {
            Debug.Log($"DeleteAsync: {uri}", Color.cyan);

            using var request = UnityWebRequest.Delete(uri);
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync(request, false, retryCount, cancellationToken: cancellationToken);
        }

        public static async Task<ServerAPIResult> UploadFileAsync(Uri uri, byte[] fileData, Action<float> progress, int retryCount = 2, int timeout = 0, Dictionary<string, string> header = null, CancellationToken cancellationToken = default)
        {
            Debug.Log($"UploadFileAsync: {uri}", Color.cyan);

            using var request = UnityWebRequest.Post(uri, new List<IMultipartFormSection> { new MultipartFormFileSection(fileData) });
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync(request, false, retryCount, progress, null, cancellationToken);
        }
        
        public static async Task<ServerAPIResult<byte[]>> DownloadFileAsync(Uri uri, Action<float> progress, int retryCount = 2, int timeout = 0, Dictionary<string, string> header = null, CancellationToken cancellationToken = default)
        {
            Debug.Log($"DownloadFileAsync: {uri}", Color.cyan);

            using var request = UnityWebRequest.Get(uri);
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync<byte[]>(request, true, retryCount, null, progress, cancellationToken);
        }

        public static void SetRequestHeader(this UnityWebRequest request, Dictionary<string, string> header)
            => header.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
        
        public static async Task<ServerAPIResult<T>> SendRequestAsync<T>(UnityWebRequest request, bool isData = false, int retryCount = 2, Action<float> uploadProgress = null, Action<float> downloadProgress = null, CancellationToken cancellationToken = default)
        {
            var sendRequestAsync = await SendRequestAsync(request, isData, retryCount, uploadProgress, downloadProgress, cancellationToken);
            var value = sendRequestAsync.IsSuccess && !isData ? JsonConvert.DeserializeObject<T>(request.downloadHandler.text)
                : sendRequestAsync.IsSuccess && isData ? (T)(object)request.downloadHandler.data
                : default;
            return new ServerAPIResult<T>(value, sendRequestAsync.Exception);
        }
        
        public static async Task<ServerAPIResult> SendRequestAsync(UnityWebRequest request, bool isData = false, int retryCount = 2, Action<float> uploadProgress = null, Action<float> downloadProgress = null, CancellationToken cancellationToken = default)
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
                        Debug.LogError($"Request ({i + 1}/{retryCount}) {request.result}: {request.error}");
                    else
                    {
                        Debug.Log($"Request Success: {(!isData ? $"text: {request.downloadHandler.text}" : $"data: {string.Join("", request.downloadHandler.data)}", Color.green)}");
                        return new ServerAPIResult(null);
                    }
                }
                
                return new ServerAPIResult(new Exception($"Request {request.result}: {request.error}"));
            }
            catch (OperationCanceledException e)
            {
                Debug.LogError($"Request Canceled: {e}");
                return new ServerAPIResult(e);
            }
            catch (UnityException e)
            {
                Debug.LogError($"Request Unity exception: {e}");
                return new ServerAPIResult(e);
            }
            catch (InvalidOperationException e)
            {
                Debug.LogError($"Request Invalid operation exception: {e}");
                return new ServerAPIResult(e);
            }
            catch (Exception e)
            {
                Debug.LogError($"Request Unexpected exception: {e}");
                return new ServerAPIResult(e);
            }
        }
    }
}