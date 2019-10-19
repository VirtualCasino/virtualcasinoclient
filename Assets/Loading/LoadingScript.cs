using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScript : MonoBehaviour
{
    
    public RectTransform mainIcon;
    public float timeStep;
    public float oneStepAngle;

    float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime >= timeStep) {
            Vector3 iconAngle = mainIcon.localEulerAngles;
            iconAngle.z += oneStepAngle;

            mainIcon.localEulerAngles = iconAngle;

            startTime = Time.time;
        }
    }
}
