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
    int frequency = 10;

    private void Update() {
        if (!isFlickering) {
            StartCoroutine(Flicker());
        }
    }

    IEnumerator Flicker() {
        light2D = gameObject.transform.GetChild(0).GetComponent<Light2D>();
        isFlickering = true;
        float originalIntensity = light2D.intensity;
        for (int i = 0; i < 10; i++) {
            int random = r.Next(1, 5);
            if (random == 1) { light2D.intensity = originalIntensity; }
            else if (random == 2) { light2D.intensity = originalIntensity / 2; }
            else { light2D.intensity = 0f; }
            yield return new WaitForSeconds(r.Next(2, 10) / 10);
        }
        light2D.intensity = originalIntensity;
        yield return new WaitForSeconds(r.Next(2, 100 / frequency));
        isFlickering = false;
    }
}
