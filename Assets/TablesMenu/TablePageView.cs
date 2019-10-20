using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class TablePageView {

    public TablePageView(int totalPages, int currentPageNumber, List<TableView> content) {
        this.totalPages = totalPages;
        this.currentPageNumber = currentPageNumber;
        this.content = content;
    }

    public int totalPages;
    public int currentPageNumber;
    public List<TableView> content;

}