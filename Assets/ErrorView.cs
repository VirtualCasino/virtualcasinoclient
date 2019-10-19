using System.Collections;
using System.Collections.Generic;

public class ErrorView {

    public ErrorView(string code, Dictionary<string, string> param) {
        this.code = code;
        this.param = param;
    }

    public ErrorView(string code) {
        this.code = code;
    }

    public string code;

    public Dictionary<string, string> param;

}