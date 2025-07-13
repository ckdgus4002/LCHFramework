using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Managers
{
    public static class ServerAPIManager
    {
        public static async Task<RequestResult<T>> GetAsync<T>(
            string url,
            Dictionary<string, string> headers = null,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            Debug.Log($"GetAsync: {url}", Color.cyan);

            using var request = UnityWebRequest.Get(url);
            AddHeaders(request, headers);
            request.timeout = timeout;

            return await SendRequestAsync<T>(request, cancellationToken);
        }

        public static async Task<RequestResult<T>> PostAsync<T>(
            string url,
            object body,
            Dictionary<string, string> headers = null,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            Debug.Log($"PostAsync: {url}", Color.cyan);

            var jsonBody = JsonConvert.SerializeObject(body);
            var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

            using var request = UnityWebRequest.PostWwwForm(url, "");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            AddHeaders(request, headers);
            request.timeout = timeout;

            return await SendRequestAsync<T>(request, cancellationToken);
        }

        public static async Task<RequestResult<T>> PutAsync<T>(
            string url,
            object body,
            Dictionary<string, string> headers = null,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            Debug.Log($"PutAsync: {url}", Color.cyan);

            var jsonBody = JsonConvert.SerializeObject(body);
            var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

            using var request = UnityWebRequest.Put(url, bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            AddHeaders(request, headers);
            request.timeout = timeout;

            return await SendRequestAsync<T>(request, cancellationToken);
        }

        public static async Task<RequestResult<T>> DeleteAsync<T>(
            string url,
            Dictionary<string, string> headers = null,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            Debug.Log($"DeleteAsync: {url}", Color.cyan);

            using var request = UnityWebRequest.Delete(url);
            AddHeaders(request, headers);
            request.timeout = timeout;

            return await SendRequestAsync<T>(request, cancellationToken);
        }

        public static async Task<RequestResult<byte[]>> DownloadFileAsync(
            string url,
            Dictionary<string, string> headers = null,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            Debug.Log($"DownloadFileAsync: {url}", Color.cyan);

            using var request = UnityWebRequest.Get(url);
            AddHeaders(request, headers);
            request.timeout = timeout;
            
            try
            {
                await WithCancellationAndAbort(request.SendWebRequest(), request, cancellationToken);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return RequestResult<byte[]>.Success(request.downloadHandler.data);
                }
                else
                {
                    Debug.LogError($"Download failed: {request.error}");
                    return RequestResult<byte[]>.Fail(new Exception(request.error));
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.LogError("Download canceled");
                return RequestResult<byte[]>.Fail(ex);
            }
        }

        public static async Task<RequestResult<T>> UploadFileAsync<T>(
            string url,
            byte[] fileData,
            string fileName,
            string formFieldName = "file",
            Dictionary<string, string> headers = null,
            int timeout = 0,
            CancellationToken cancellationToken = default)
        {
            Debug.Log($"UploadFileAsync: {url}", Color.cyan);

            var formData = new List<IMultipartFormSection>
            {
                new MultipartFormFileSection(formFieldName, fileData, fileName, "application/octet-stream")
            };

            using var request = UnityWebRequest.Post(url, formData);
            AddHeaders(request, headers);
            request.timeout = timeout;

            return await SendRequestAsync<T>(request, cancellationToken);
        }

        private static async Task<RequestResult<T>> SendRequestAsync<T>(UnityWebRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await WithCancellationAndAbort(request.SendWebRequest(), request, cancellationToken);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var responseText = request.downloadHandler.text;
                    Debug.Log($"Response: {responseText}", Color.green);

                    if (!string.IsNullOrEmpty(responseText))
                        return RequestResult<T>.Success(JsonConvert.DeserializeObject<T>(responseText));
                    else
                        return RequestResult<T>.Success(default);
                }
                else
                {
                    Debug.LogError($"Request failed: {request.error}");
                    return RequestResult<T>.Fail(new Exception(request.error));
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.LogError("Request canceled");
                return RequestResult<T>.Fail(ex);
            }
            catch (UnityException ex)
            {
                Debug.LogError($"Unity exception: {ex}");
                return RequestResult<T>.Fail(ex);
            }
            catch (InvalidOperationException ex)
            {
                Debug.LogError($"Invalid operation: {ex}");
                return RequestResult<T>.Fail(ex);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error: {ex}");
                return RequestResult<T>.Fail(ex);
            }
        }

        private static async Task WithCancellationAndAbort(UnityWebRequestAsyncOperation operation, UnityWebRequest request, CancellationToken cancellationToken)
        {
            using var registration = cancellationToken.Register(() => request.Abort());

            while (!operation.isDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }
        }

        private static void AddHeaders(UnityWebRequest request, Dictionary<string, string> headers)
        {
            if (headers == null) return;

            foreach (var pair in headers)
                request.SetRequestHeader(pair.Key, pair.Value);
        }
        
        
        
        public struct RequestResult<T>
        {
            private RequestResult(T value, bool isSuccess, Exception exception)
            {
                Value = value;
                IsSuccess = isSuccess;
                Exception = exception;
            }
            
            public static RequestResult<T> Success(T value) => new(value, true, null);
            public static RequestResult<T> Fail(Exception exception) => new(default, false, exception);
            
            public T Value { get; private set; }
            public bool IsSuccess { get; private set; }
            public Exception Exception { get; private set; }
        }
    }
}