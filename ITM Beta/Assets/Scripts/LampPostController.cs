using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = System.Random;

public class LampPostController : MonoBehaviour
{
    private Random r = new Random();
    private bool isFlickering = false;
    Light2D light2D;
    int frequency;
    float maxIntensity;


    private void Awake() {
        frequency = (InstantiateMaze.weather == "storm") ? 10 : 5;
        light2D = gameObject.transform.GetChild(0).GetComponent<Light2D>();
        maxIntensity = light2D.intensity;
    }
    private void Update() {
        if (!isFlickering) {
            StartCoroutine(Flicker());
        }
    }

    private IEnumerator Flicker() {
        isFlickering = true;
        for (int i = 0; i < 100; i++) {
            light2D.intensity -= (maxIntensity / 100f);
            yield return new WaitForSeconds(0.01f);
        }
        light2D.intensity = 0f;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 100; i++) {
            light2D.intensity += (maxIntensity / 100f);
            yield return new WaitForSeconds(0.01f);
        }
        light2D.intensity = maxIntensity;
        yield return new WaitForSeconds(0.25f);
        isFlickering = false;
    }
}
