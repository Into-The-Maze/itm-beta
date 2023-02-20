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
            rectTransform.transform.position = new Vector3(40, 1040, 0);
            alpha.DOFade(1f, 0.2f);
            shutFOV();
            invIsOpen = !invIsOpen;
        }
        else {
            alpha.DOFade(0f, 0.2f);
            openFOV();
            s.Start();
            if (s.ElapsedMilliseconds == 200) { rectTransform.transform.position = new Vector3(-680, 1040, 0); }
            s.Stop();
            invIsOpen = !invIsOpen;
        }
    }

    void shutFOV() {
        visionLight.pointLightInnerAngle = 0;
        visionLight.pointLightOuterAngle = 0;
    }
    void openFOV() {
        visionLight.pointLightInnerAngle = 80;
        visionLight.pointLightOuterAngle = 100;
    }
}
