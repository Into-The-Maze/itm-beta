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


    private void Awake() {
        frequency = (InstantiateMaze.weather == "storm") ? 10 : 5;
    }
    private void Update() {
        
        if (!isFlickering) {
            StartCoroutine(Flicker());
        }
    }

    IEnumerator Flicker() {
        light2D = gameObject.transform.GetChild(0).GetComponent<Light2D>();
        isFlickering = true;
        yield return new WaitForSeconds(r.Next(2, 100 / frequency));
        float originalIntensity = light2D.intensity;
        for (int i = 0; i < r.Next(frequency, frequency * 2); i++) {
            int random = r.Next(1, 6);
            if (random == 1 || random == 2) { 
                light2D.intensity = originalIntensity;
                yield return new WaitForSeconds(r.Next(1, 3) / 100f);
            }
            else if (random == 3 || random == 4) { 
                light2D.intensity = originalIntensity / 2.5f;
                yield return new WaitForSeconds(r.Next(1, 3) / 100f);
            }
            else {
                light2D.intensity = 0f;
                yield return new WaitForSeconds(r.Next(1, 3) / 100f);
            }
        }
        light2D.intensity = 0f;
        yield return new WaitForSeconds(0.375f);
        light2D.intensity = originalIntensity;
        isFlickering = false;
    }
}
