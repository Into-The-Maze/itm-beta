using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LabsBreachController : MonoBehaviour
{
    private bool isFlashing = false;
    Light2D light2D;
    Light2D wallLight2D;
    float maxIntensity;
    float maxWallIntensity;
    WaitForSeconds timeIncrementer = new WaitForSeconds(0.001f);
    WaitForSeconds darknessTimer = new WaitForSeconds(1f);

    void Update()
    {
        if (!isFlashing) {
            StartCoroutine(Flash());
        }   
    }
    public void GetLights(Light2D global, Light2D globalWall) {
        light2D = global;
        wallLight2D = globalWall;
        maxIntensity = light2D.intensity;
        maxWallIntensity = wallLight2D.intensity;
    }
    private IEnumerator Flash() {
        isFlashing = true;
        while (light2D.intensity > 0) {
            light2D.intensity -= (maxIntensity / 50f);
            wallLight2D.intensity -= (maxWallIntensity / 50f);
            yield return timeIncrementer;
        }
        light2D.intensity = 0f;
        wallLight2D.intensity = 0f;
        yield return darknessTimer;
        while (light2D.intensity < maxIntensity) {
            light2D.intensity += (maxIntensity / 50f);
            wallLight2D.intensity += (maxWallIntensity / 50f);
            yield return timeIncrementer;
        }
        light2D.intensity = maxIntensity;
        wallLight2D.intensity = maxWallIntensity;
        isFlashing = false;
    }
}
