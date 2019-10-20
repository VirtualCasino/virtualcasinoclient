using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMenuScript : MonoBehaviour
{
    
    public int timeOfErrorMessageInSeconds = 2;
    public GameObject loadingHandle;
    public GameObject errorToastHandle;
    public GameObject errorTextMessageHandle;
    public GameObject nickValue;
    public GameObject tokensValue;
    HttpClient httpClient;
    LoadingDisplayer loadingDisplayer;
    ErrorMessageDisplayer errorMessageDisplayer;

    public void Start() {
        errorMessageDisplayer = new ErrorMessageDisplayer(errorToastHandle, errorTextMessageHandle);
        loadingDisplayer = new LoadingDisplayer(loadingHandle);
        httpClient = new HttpClient();
    }

    public void OnEnable()
    {
        string nick = PlayerPrefs.GetString("Nick");
        int tokensCount = PlayerPrefs.GetInt("Tokens");
        nickValue.GetComponent<TextMeshProUGUI>().text = nick;
        tokensValue.GetComponent<TextMeshProUGUI>().text = Convert.ToString(tokensCount);
    }

    public void createGame() {
        loadingDisplayer.showLoading();
        string clientId = PlayerPrefs.GetString("Id");
        ReserveRouletteTable reserveRouletteTable = new ReserveRouletteTable(new ClientId(clientId));
        string reserveTableJson = JsonUtility.ToJson(reserveRouletteTable) ?? "";
        Debug.Log("Reserving table");
        StartCoroutine(reserveTableCorutine(reserveTableJson));
    }

    private IEnumerator reserveTableCorutine(string reserveTableJson) {
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Post("/virtual-casino/casino-services/tables/roulette", reserveTableJson), (output) => result = output);
        Debug.Log("Table reserved");
        tableReserved(result);
    }

     private void tableReserved(HttpResponse result) {
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
            string tableUri = result.headers["Location"];
            StartCoroutine(getTable(tableUri));
        }
    }

    private IEnumerator getTable(string tableUri) {
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Get(tableUri), (output) => result = output);
        Debug.Log("Got table: " + result.response);
        TableView tableView = JsonUtility.FromJson<TableView>(result.response);
        gotTable(tableView);
    }

    private void gotTable(TableView tableView) {
        PlayerPrefs.SetString("TableId", tableView.tableViewId);
        loadingDisplayer.hideLoading();
        SceneManager.LoadScene("Game");
    }

    public static IEnumerator Run<T>(IEnumerator target, Action<T> output) {
         object result = null;
         while (target.MoveNext())
         {
             result = target.Current;
             yield return result;
         }
         output((T)result);
     }
}
