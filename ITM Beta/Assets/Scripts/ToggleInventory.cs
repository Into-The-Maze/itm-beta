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
    private WaitForSeconds delay = new(0.2f);
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
        StartCoroutine(moveInv()); 
    }

    IEnumerator moveInv() {
        yield return delay;
        invRectTransform.transform.position = new Vector3(2280f, 640f, 0f);
        StopCoroutine(moveInv());
    }

    private void openInv() {
        invRectTransform.transform.position = new Vector3(40, 1040, 0);
        invAlpha.DOFade(1f, 0.2f);
    }
}
