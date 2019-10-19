using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient {

    public IEnumerator Post(string url, string bodyJsonString)
    {
        Debug.Log("Sending post request with body:" + bodyJsonString);
        UnityWebRequest request = null;
        using (request = CreatePostRequest(url, bodyJsonString))
        {
            yield return request.SendWebRequest();
            yield return new HttpResponse(request.isNetworkError || request.isHttpError, request.downloadHandler.text);
        }
    }

    private UnityWebRequest CreatePostRequest(string url, string bodyJsonString) {
        var request = new UnityWebRequest(Config.server_url + url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }
}