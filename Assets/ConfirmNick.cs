using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class ConfirmNick : MonoBehaviour
{

    public GameObject errorToastHandle;
    public GameObject errorTextMessageHandle;
    public GameObject nickInput;
    public HttpClient httpClient;

    public ErrorMessageDisplayer errorMessageDisplayer;
    UnityWebRequest result;
    
    public void Start() {
        errorMessageDisplayer = new ErrorMessageDisplayer(errorToastHandle, errorTextMessageHandle);
        httpClient = new HttpClient();
    }

    public void sendNick() {
        string typedNick = nickInput.GetComponent<Text>().text;
        RegisterClient registerClient = new RegisterClient(new Nick(typedNick));
        string registerClientJson = JsonUtility.ToJson(registerClient) ?? "";
        Debug.Log("Registering client");
        StartCoroutine(sendNickCorutine(registerClientJson));
    }

    private IEnumerator sendNickCorutine(string registerClientJson) {
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Post("/virtual-casino/casino-services/clients", registerClientJson), (output) => result = output);
        Debug.Log("Client registered");
        nickSent(result);
    }

    private void nickSent(HttpResponse result) {
        if(result == null) {
            errorMessageDisplayer.DisplayErrorMessage(new ErrorView("internalServerError"));
        }
        if (result.isError){
            Debug.Log("Error:");
            Debug.Log(result.response);
            errorMessageDisplayer.DisplayErrorMessage(result.response);
        }
        else {
            Debug.Log("Request sended!");
        }
    }

    public static IEnumerator Run<T>(IEnumerator target, Action<T> output)
     {
         object result = null;
         while (target.MoveNext())
         {
             result = target.Current;
             yield return result;
         }
         output((T)result);
     }
}

[Serializable]
class RegisterClient {

    public RegisterClient(Nick nick) {
        this.nick = nick;
    }

    public Nick nick;

}

[Serializable]
class Nick {

    public Nick(string value) {
        this.value = value;
    }

    public string value;

}