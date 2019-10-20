using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class TablesMenuScript : MonoBehaviour
{
    
    public int timeOfErrorMessageInSeconds = 2;
    public GameObject tableNameText;
    public GameObject loadingHandle;
    public GameObject errorToastHandle;
    public GameObject errorTextMessageHandle;
    public ScrollRect tablesScrollView;
    public GameObject tablesScrollContent;
    public GameObject tablePageNumberText;
    public GameObject tablesScrollItemPrefab;
    HttpClient httpClient;
    LoadingDisplayer loadingDisplayer;
    ErrorMessageDisplayer errorMessageDisplayer;

    int currentPage = 0;

    void Start()
    {
        errorMessageDisplayer = new ErrorMessageDisplayer(errorToastHandle, errorTextMessageHandle);
        loadingDisplayer = new LoadingDisplayer(loadingHandle);
        httpClient = new HttpClient();
    }

    public void OnEnable()
    {
        errorMessageDisplayer = new ErrorMessageDisplayer(errorToastHandle, errorTextMessageHandle);
        loadingDisplayer = new LoadingDisplayer(loadingHandle);
        httpClient = new HttpClient();
        clearTablesList();
        StartCoroutine(getTablePageCoroutine("", 0));
    }

    public void nextPage() {
        clearTablesList();
        string tableName = tableNameText.GetComponent<Text>().text ?? "";
        StartCoroutine(getTablePageCoroutine(tableName, currentPage + 1));
    }

    public void previousPage() {
        clearTablesList();
        string tableName = tableNameText.GetComponent<Text>().text ?? "";
        int pageToGet = currentPage - 1;
        if(pageToGet < 0) {
            pageToGet = 0;
        }
        StartCoroutine(getTablePageCoroutine(tableName, pageToGet));
    }

    public void refresh() {
        clearTablesList();
        string tableName = tableNameText.GetComponent<Text>().text ?? "";
        StartCoroutine(getTablePageCoroutine(tableName, currentPage));
    }

    private IEnumerator getTablePageCoroutine(string tableName, int pageNumber) {
        loadingDisplayer.showLoading();
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Get(
            "/virtual-casino/casino-services/tables/roulette?" + "searchedPlayerNick=" + tableName + "&pageNumber=" + pageNumber
            ), 
            (output) => result = output
        );
        Debug.Log("Got table page");
        gotTablePage(result);
    }

    private void gotTablePage(HttpResponse result) {
        TablePageView tablePageView = JsonUtility.FromJson<TablePageView>(result.response);
        tablePageNumberText.GetComponent<TextMeshProUGUI>().text = (tablePageView.currentPageNumber + 1) + "/" + (tablePageView.totalPages + 1);
        currentPage = tablePageView.currentPageNumber;
        foreach(TableView tableView in tablePageView.content) {
            generateTableScrollItem(tableView);
        }
        tablesScrollView.verticalNormalizedPosition = 1;
        loadingDisplayer.hideLoading();
    }

    private void generateTableScrollItem(TableView tableView) {
        GameObject tableScrollItemObj = Instantiate(tablesScrollItemPrefab);
        tableScrollItemObj.transform.SetParent(tablesScrollContent.transform, false);
        tableScrollItemObj.transform.Find("Table_Name").gameObject.GetComponent<TextMeshProUGUI>().text = tableView.firstPlayerNick;
        tableScrollItemObj.transform.Find("Clients_Count").gameObject.GetComponent<TextMeshProUGUI>().text = tableView.playersIds.Count + "/" + tableView.maxPlayersCount;
    }

    private void clearTablesList() {
        while(tablesScrollContent.transform.childCount > 0) {
            Destroy(tablesScrollContent.transform.GetChild(0).gameObject);
        }
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
