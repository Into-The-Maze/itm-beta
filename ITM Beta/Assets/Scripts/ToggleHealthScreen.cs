using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;

public class ToggleHealthScreen : MonoBehaviour
{
    public RectTransform healthRectTransform;
    [SerializeField] private CanvasGroup healthAlpha;
    Stopwatch s = new();
    public static bool invIsOpen = false;
    public static ToggleHealthScreen h;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            toggleHealthScreen(ref invIsOpen);
        }
    }
    private void Awake() {
        healthAlpha.DOFade(0f, 0f);
        h = this;
    }

    private void toggleHealthScreen(ref bool invIsOpen) {
        healthRectTransform = GetComponent<RectTransform>();
        if (!invIsOpen) {
            if (!ToggleEquipMenu.invIsOpen) {
                open();
            }
            else {
                ToggleEquipMenu.e.close();
                ToggleEquipMenu.invIsOpen = false;
                open();
            }
        }
        else {
            close();
        }
        invIsOpen = !invIsOpen;
    }

    public void open() {
        healthRectTransform.transform.position = new Vector3(1560f, 640f, 0f);
        healthAlpha.DOFade(1f, 0.2f);
    }

    public void close() {
        healthAlpha.DOFade(0f, 0.2f);
        s.Start();
        if (s.ElapsedMilliseconds == 200) { healthRectTransform.transform.position = new Vector3(2280f, 640f, 0f); }
        s.Stop();
    }
}
