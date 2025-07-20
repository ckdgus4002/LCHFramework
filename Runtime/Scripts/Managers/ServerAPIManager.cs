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
        public static async Task<ServerAPIResult<T>> GetAsync<T>(Uri uri, Dictionary<string, string> header = null, int timeout = 0, CancellationToken cancellationToken = default)
        {
            Debug.Log($"GetAsync: {uri}", Color.cyan);

            using var request = UnityWebRequest.Get(uri);
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync<T>(request, cancellationToken: cancellationToken);
        }

        public static async Task<ServerAPIResult> PostAsync(Uri uri, object body, Dictionary<string, string> header = null, int timeout = 0, CancellationToken cancellationToken = default)
        {
            Debug.Log($"PostAsync: {uri}", Color.cyan);
            
            using var request = UnityWebRequest.PostWwwForm(uri, JsonConvert.SerializeObject(body));
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync(request, cancellationToken: cancellationToken);
        }

        public static async Task<ServerAPIResult> PutAsync(Uri uri, object body, Dictionary<string, string> header = null, int timeout = 0, CancellationToken cancellationToken = default)
        {
            Debug.Log($"PutAsync: {uri}", Color.cyan);
            
            using var request = UnityWebRequest.Put(uri, JsonConvert.SerializeObject(body));
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync(request, cancellationToken: cancellationToken);
        }

        public static async Task<ServerAPIResult> DeleteAsync(Uri uri, Dictionary<string, string> header = null, int timeout = 0, CancellationToken cancellationToken = default)
        {
            Debug.Log($"DeleteAsync: {uri}", Color.cyan);

            using var request = UnityWebRequest.Delete(uri);
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync(request, cancellationToken: cancellationToken);
        }

        public static async Task<ServerAPIResult<byte[]>> DownloadFileAsync(Uri uri, Dictionary<string, string> header = null, int timeout = 0, CancellationToken cancellationToken = default)
        {
            Debug.Log($"DownloadFileAsync: {uri}", Color.cyan);

            using var request = UnityWebRequest.Get(uri);
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync<byte[]>(request, true, cancellationToken);
        }

        public static async Task<ServerAPIResult> UploadFileAsync(Uri uri, byte[] fileData, string fileName, string formFieldName = "file", Dictionary<string, string> header = null, int timeout = 0, CancellationToken cancellationToken = default)
        {
            Debug.Log($"UploadFileAsync: {uri}", Color.cyan);

            using var request = UnityWebRequest.Post(uri, new List<IMultipartFormSection> { new MultipartFormFileSection(formFieldName, fileData, fileName, "application/octet-stream") });
            request.SetRequestHeader(header);
            request.timeout = timeout;

            return await SendRequestAsync(request, cancellationToken: cancellationToken);
        }

        private static void SetRequestHeader(this UnityWebRequest request, Dictionary<string, string> header)
            => header.ForEach(t => request.SetRequestHeader(t.Key, t.Value));
        
        private static async Task<ServerAPIResult<T>> SendRequestAsync<T>(UnityWebRequest request, bool isData = false, CancellationToken cancellationToken = default)
        {
            var sendRequestAsync = await SendRequestAsync(request, isData, cancellationToken);
            
            var value = sendRequestAsync.IsSuccess && !isData ? JsonConvert.DeserializeObject<T>(request.downloadHandler.text)
                : sendRequestAsync.IsSuccess && isData ? (T)(object)request.downloadHandler.data
                : default;
            return new ServerAPIResult<T>(value, sendRequestAsync.Exception);
        }
        
        private static async Task<ServerAPIResult> SendRequestAsync(UnityWebRequest request, bool isData = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var operation = request.SendWebRequest();
                await using var registration = cancellationToken.Register(request.Abort);
                while (!operation.isDone)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Response: {(!isData ? request.downloadHandler.text : string.Join("", request.downloadHandler.data))}", Color.green);
                    return new ServerAPIResult(null);
                }

                Debug.LogError($"Request failed: {request.error}");
                return new ServerAPIResult(new Exception(request.error));
            }
            catch (OperationCanceledException e)
            {
                Debug.LogError("Request canceled");
                return new ServerAPIResult(e);
            }
            catch (UnityException e)
            {
                Debug.LogError($"Unity exception: {e}");
                return new ServerAPIResult(e);
            }
            catch (InvalidOperationException e)
            {
                Debug.LogError($"Invalid operation: {e}");
                return new ServerAPIResult(e);
            }
            catch (Exception e)
            {
                Debug.LogError($"Unexpected error: {e}");
                return new ServerAPIResult(e);
            }
        }
    }
}