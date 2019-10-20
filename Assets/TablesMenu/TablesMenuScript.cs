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
    public Button tablesScrollItemPrefab;
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
        Debug.Log("Next page");
        clearTablesList();
        string tableName = tableNameText.GetComponent<Text>().text ?? "";
        StartCoroutine(getTablePageCoroutine(tableName, currentPage + 1));
    }

    public void previousPage() {
        Debug.Log("Previous page");
        clearTablesList();
        string tableName = tableNameText.GetComponent<Text>().text ?? "";
        int pageToGet = currentPage - 1;
        if(pageToGet < 0) {
            pageToGet = 0;
        }
        StartCoroutine(getTablePageCoroutine(tableName, pageToGet));
    }

    public void refresh() {
        Debug.Log("Refresh");
        clearTablesList();
        string tableName = tableNameText.GetComponent<Text>().text ?? "";
        StartCoroutine(getTablePageCoroutine(tableName, currentPage));
    }

    public void findByName() {
        clearTablesList();
        string tableName = tableNameText.GetComponent<Text>().text ?? "";
        Debug.Log("Find by name:" + tableName);
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
        Button tableScrollItemObj = Instantiate(tablesScrollItemPrefab);
        tableScrollItemObj.transform.SetParent(tablesScrollContent.transform, false);
        tableScrollItemObj.transform.Find("Table_Name").gameObject.GetComponent<TextMeshProUGUI>().text = tableView.firstPlayerNick;
        tableScrollItemObj.transform.Find("Clients_Count").gameObject.GetComponent<TextMeshProUGUI>().text = tableView.playersIds.Count + "/" + tableView.maxPlayersCount;
        tableScrollItemObj.onClick.AddListener(() => {
            loadingDisplayer.showLoading();
            string clientIdValue = PlayerPrefs.GetString("Id");
            ClientId clientId = new ClientId(clientIdValue);
            TableId tableId = new TableId(tableView.tableViewId);
            JoinTable joinTable = new JoinTable(clientId, tableId);
            Debug.Log("Joining to table:s" + joinTable.tableId + " by client: " + joinTable.clientId);
            string joinTableJson = JsonUtility.ToJson(joinTable) ?? "";
            StartCoroutine(this.joinTable(joinTableJson));
        });
    }
    
    private IEnumerator joinTable(string joinTableJson) {
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Post("/virtual-casino/casino-services/tables/participation", joinTableJson), (output) => result = output);
        Debug.Log("Table reserved");
        joinedTable(result);
    }

    private void joinedTable(HttpResponse result) {
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

    private void clearTablesList() {
        Debug.Log("Clear tables list");
        for (var i = tablesScrollContent.transform.childCount - 1; i >= 0; i--) {
            var tableView = tablesScrollContent.transform.GetChild(i);
            tableView.transform.SetParent(null);
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
