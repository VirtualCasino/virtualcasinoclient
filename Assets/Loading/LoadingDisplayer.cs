using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoadingDisplayer {

    public LoadingDisplayer(GameObject loadingHandle) {
        this.loadingHandle = loadingHandle;
    }

    public GameObject loadingHandle;

    public void showLoading() {
        loadingHandle.SetActive(true);
    }

    public void hideLoading() {
        loadingHandle.SetActive(false);
    }

} 