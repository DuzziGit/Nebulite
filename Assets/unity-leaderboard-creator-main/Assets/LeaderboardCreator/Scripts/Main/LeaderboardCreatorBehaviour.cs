using System;
using System.Collections;
using System.Collections.Generic;
using Dan.Enums;
using Dan.Models;
using UnityEngine;
using UnityEngine.Networking;

using static Dan.ConstantVariables;

namespace Dan.Main
{
    public sealed class LeaderboardCreatorBehaviour : MonoBehaviour
    {
        [Serializable]
        private struct EntryResponse
        {
            public Entry[] entries;
        }

        internal static LeaderboardCreatorConfig Config =>
            Resources.Load<LeaderboardCreatorConfig>("LeaderboardCreatorConfig");

        private static string GetError(UnityWebRequest request) =>
            $"{request.responseCode}: {request.downloadHandler.text}";

        // Removed: Authorize() and ResetAndAuthorize()

        internal void SendGetRequest(string url, Action<bool> callback, Action<string> errorCallback)
        {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(false);
                    errorCallback?.Invoke(GetError(request));
                    return;
                }
                callback?.Invoke(true);
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }

        internal void SendGetRequest(string url, Action<int> callback, Action<string> errorCallback)
        {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(0);
                    errorCallback?.Invoke(GetError(request));
                    return;
                }
                callback?.Invoke(int.Parse(request.downloadHandler.text));
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }

        internal void SendGetRequest(string url, Action<Entry> callback, Action<string> errorCallback)
        {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(new Entry());
                    errorCallback?.Invoke(GetError(request));
                    return;
                }
                var response = JsonUtility.FromJson<Entry>(request.downloadHandler.text);
                callback?.Invoke(response);
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }

        internal void SendGetRequest(string url, Action<Entry[]> callback, Action<string> errorCallback)
        {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(Array.Empty<Entry>());
                    errorCallback?.Invoke(GetError(request));
                    return;
                }
                var response = JsonUtility.FromJson<EntryResponse>($"{{\"entries\":{request.downloadHandler.text}}}");
                callback?.Invoke(response.entries);
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }

        internal void SendPostRequest(string url, List<IMultipartFormSection> form, Action<bool> callback = null, Action<string> errorCallback = null)
        {
            var request = UnityWebRequest.Post(url, form);
            StartCoroutine(HandleRequest(request, callback, errorCallback));
        }

#if UNITY_ANDROID
        private class ForceAcceptAll : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData) => true;
        }
#endif

        private static IEnumerator HandleRequest(UnityWebRequest request, Action<bool> onComplete, Action<string> errorCallback = null)
        {
#if UNITY_ANDROID
    request.certificateHandler = new ForceAcceptAll();
#endif
            yield return request.SendWebRequest();

            // 🔍 DEBUG OUTPUT
            Debug.Log($"[HTTP] URL Called: {request.url}");
            Debug.Log($"[HTTP] Response Code: {request.responseCode}");
            Debug.Log($"[HTTP] Raw Response: {request.downloadHandler.text}");
            
            if (request.responseCode != 200)
            {
                onComplete.Invoke(false);
                errorCallback?.Invoke(GetError(request));
                request.downloadHandler.Dispose();
                request.Dispose();
                yield break;
            }

            onComplete.Invoke(true);
            request.downloadHandler.Dispose();
            request.Dispose();
        }


        private static void HandleError(UnityWebRequest request)
        {
            var message = Enum.GetName(typeof(StatusCode), (StatusCode)request.responseCode);
            message = string.IsNullOrEmpty(message) ? "Unknown" : message.SplitByUppercase();

            var text = request.downloadHandler.text;
            if (!string.IsNullOrEmpty(text))
                message = $"{message}: {text}";

            LeaderboardCreator.LogError(message);
        }
    }
}
