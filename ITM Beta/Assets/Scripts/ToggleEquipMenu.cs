using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics;
using System;

public class ToggleEquipMenu : MonoBehaviour
{
    public RectTransform equipRectTransform;
    [SerializeField] private CanvasGroup equipAlpha;
    Stopwatch s = new();
    public static bool invIsOpen = false;
    public static ToggleEquipMenu e;

    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            ToggleInv(ref invIsOpen);
        }
    }

    

    private void Awake() {
        equipAlpha.DOFade(0f, 0f);
        e = this;
    }

    private void ToggleInv(ref bool invIsOpen) {
        equipRectTransform = GetComponent<RectTransform>();

        if (!invIsOpen) {
            if (!ToggleHealthScreen.invIsOpen) {
                open();
            }
            else {
                ToggleHealthScreen.h.close();
                ToggleHealthScreen.invIsOpen = false;
                open();
            }
        }
        else {
            close();
        }
        invIsOpen = !invIsOpen;
    }

    public void open() {
        equipRectTransform.transform.position = new Vector3(1560f, 640f, 0f);
        equipAlpha.DOFade(1f, 0.2f);
    }

    public void close() {
        equipAlpha.DOFade(0f, 0.2f);
        s.Start();
        if (s.ElapsedMilliseconds == 200) { equipRectTransform.transform.position = new Vector3(2280f, 640f, 0f); }
        s.Stop();
    }
}
