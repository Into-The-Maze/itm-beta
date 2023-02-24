using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics;
using UnityEngine.Rendering.Universal;
using System.Threading.Tasks;

public class ToggleInventory : MonoBehaviour
{
    RectTransform invRectTransform;
    [SerializeField] private CanvasGroup invAlpha;
    Stopwatch s = new();
    public Light2D visionLight;
    //static int innerAngle = 50;
    //static int outerAngle = 60;
    
    public static bool invIsOpen = false;

    private void Awake() {
        invAlpha.DOFade(0f, 0f);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ToggleInv(ref invIsOpen);
        }
    }
    
    void ToggleInv(ref bool invIsOpen) {
        invRectTransform = GetComponent<RectTransform>();

        if (!invIsOpen) {
            openInv();
        }
        else {
            closeInv();
        }

        invIsOpen = !invIsOpen;
    }

    private void closeInv() {
        invAlpha.DOFade(0f, 0.2f);
        //openFOVInv();
        s.Start();
        if (s.ElapsedMilliseconds == 200) { invRectTransform.transform.position = new Vector3(-680, 1040, 0); }
        s.Stop();
    }

    private void openInv() {
        invRectTransform.transform.position = new Vector3(40, 1040, 0);
        invAlpha.DOFade(1f, 0.2f);
        //shutFOVInv();
    }

    //void shutFOVInv() {
    //    visionLight.pointLightInnerAngle = 0;
    //    visionLight.pointLightOuterAngle = 0;
    //}
    //void openFOVInv() {
    //    visionLight.pointLightInnerAngle = innerAngle;
    //    visionLight.pointLightOuterAngle = outerAngle;
    //}

    //public static void shutFOVRun() {
    //    visionLight.pointLightInnerAngle = DOVirtual.EasedValue(innerAngle, 60, 0.3f, Ease.OutElastic);
    //    visionLight.pointLightOuterAngle = DOVirtual.EasedValue(outerAngle, 80, 0.3f, Ease.OutElastic);
    //}
    //public static void openFOVRun() {
    //    visionLight.pointLightInnerAngle = DOVirtual.EasedValue(60, innerAngle, 0.3f, Ease.OutElastic);
    //    visionLight.pointLightOuterAngle = DOVirtual.EasedValue(80, outerAngle, 0.3f, Ease.OutElastic);
    //}
}
