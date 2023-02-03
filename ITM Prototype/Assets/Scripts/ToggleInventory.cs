using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics;
using UnityEngine.Rendering.Universal;

public class ToggleInventory : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField] private CanvasGroup alpha;
    Stopwatch s = new();
    public Light2D visionLight;
    
    public static bool invIsOpen = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ToggleInv(ref invIsOpen);
        }
    }
    
    void ToggleInv(ref bool invIsOpen) {
        

        rectTransform = GetComponent<RectTransform>();
        if (!invIsOpen) {
            rectTransform.transform.position = new Vector3(0, 1080, 0);
            alpha.DOFade(1f, 0.2f);
            shutFOV();
            invIsOpen = !invIsOpen;
        }
        else {
            alpha.DOFade(0f, 0.2f);
            openFOV();
            s.Start();
            if (s.ElapsedMilliseconds == 200) { rectTransform.transform.position = new Vector3(-640, 1080, 0); }
            s.Stop();
            invIsOpen = !invIsOpen;
        }
    }

    void shutFOV() {
        visionLight.pointLightInnerAngle = 10;
        visionLight.pointLightOuterAngle = 10;
    }
    void openFOV() {
        visionLight.pointLightInnerAngle = 100;
        visionLight.pointLightOuterAngle = 100;
    }
}
