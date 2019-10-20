using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class ConfirmNick : MonoBehaviour
{

    public int timeOfErrorMessageInSeconds = 2;

    public GameObject loadingHandle;
    public GameObject errorToastHandle;
    public GameObject errorTextMessageHandle;
    public GameObject nickInput;
    public GameObject registerMenuHandle;
    public GameObject gameMenuHandle;
    HttpClient httpClient;
    LoadingDisplayer loadingDisplayer;
    ErrorMessageDisplayer errorMessageDisplayer;
    
    public void Start() {
        errorMessageDisplayer = new ErrorMessageDisplayer(errorToastHandle, errorTextMessageHandle);
        loadingDisplayer = new LoadingDisplayer(loadingHandle);
        httpClient = new HttpClient();
    }

    public void sendNick() {
        loadingDisplayer.showLoading();
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
        if(result == null || result.response == null) {
            loadingDisplayer.hideLoading();
            errorMessageDisplayer.DisplayErrorMessage(new ErrorView("internalServerError"));
            StartCoroutine(errorMessageDisplayer.hideErrorMessageAfterTime(timeOfErrorMessageInSeconds));
        }
        if (result.isError){
            Debug.Log("Error:");
            Debug.Log(result.response);
            loadingDisplayer.hideLoading();
            errorMessageDisplayer.DisplayErrorMessage(result.response);
            StartCoroutine(errorMessageDisplayer.hideErrorMessageAfterTime(timeOfErrorMessageInSeconds));
        }
        else {
            string clientUri = result.headers["Location"];
            StartCoroutine(getClient(clientUri));
        }
    }

    private IEnumerator getClient(string clientUri) {
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Get(clientUri), (output) => result = output);
        Debug.Log("Got client: " + result.response);
        ClientView clientView = JsonUtility.FromJson<ClientView>(result.response);
        gotClient(clientView);
    }

    private void gotClient(ClientView clientView) {
        registerMenuHandle.SetActive(false);
        PlayerPrefs.SetString("Id", clientView.clientViewId);
        PlayerPrefs.SetString("Nick", clientView.nick);
        PlayerPrefs.SetInt("Tokens", clientView.tokensCount);
        gameMenuHandle.SetActive(true);
        Debug.Log("Saved client data");
        loadingDisplayer.hideLoading();
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