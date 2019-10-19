using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpResponse {
    
    public HttpResponse(bool isError, string response, Dictionary<string,string> headers) {
        this.isError = isError;
        this.response = response;
        this.headers = headers;
    }

    public bool isError;

    public string response;

    public Dictionary<string,string> headers;

}