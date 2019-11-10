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
            yield return new HttpResponse(
                request.isNetworkError || request.isHttpError,
             request.downloadHandler.text, 
             request.GetResponseHeaders());
        }
    }

    public IEnumerator Get(string url) {
        Debug.Log("Sending get request");
        UnityWebRequest request = null;
        using (request = CreateGetRequest(url))
        {
            yield return request.SendWebRequest();
            yield return new HttpResponse(
                request.isNetworkError || request.isHttpError, 
                request.downloadHandler.text, 
                request.GetResponseHeaders()
                );
        }
    }

    public IEnumerator Delete(string url, string bodyJsonString)
    {
        Debug.Log("Sending post request with body:" + bodyJsonString);
        UnityWebRequest request = null;
        using (request = CreateDeleteRequest(url, bodyJsonString))
        {
            yield return request.SendWebRequest();
            yield return new HttpResponse(
                request.isNetworkError || request.isHttpError,
             request.downloadHandler.text,
             request.GetResponseHeaders());
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

    private UnityWebRequest CreateDeleteRequest(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(Config.server_url + url, "DELETE");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }

    private UnityWebRequest CreateGetRequest(string url) {
        var request = new UnityWebRequest(Config.server_url + url, "GET");
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        return request;
    }
}