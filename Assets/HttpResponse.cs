using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpResponse {
    
    public HttpResponse(bool isError, string response) {
        this.isError = isError;
        this.response = response;
    }

    public bool isError;

    public string response;

}