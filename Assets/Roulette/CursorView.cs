using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CursorView
{

    public static void setAsNoBets(GameObject cursor)
    {
        var fieldRenderer = cursor.GetComponent<SpriteRenderer>();
        var tempColor = fieldRenderer.color;
        tempColor = Color.green;
        tempColor.a = 0.4f;
        fieldRenderer.color = tempColor;
    }

    public static void setAsHasBets(GameObject cursor)
    {
        var fieldRenderer = cursor.GetComponent<SpriteRenderer>();
        var tempColor = fieldRenderer.color;
        tempColor = Color.red;
        tempColor.a = 0.4f;
        fieldRenderer.color = tempColor;
    }
}
