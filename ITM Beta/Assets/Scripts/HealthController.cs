using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

    public Image Thorax;
    public Image Head;
    public Image RArm;
    public Image LArm;
    public Image RLeg;
    public Image LLeg;


    private const float maxThoraxHP = 80;
    private const float maxHeadHP = 20;
    private const float maxRArmHP = 50;
    private const float maxLArmHP = 50;
    private const float maxRLegHP = 50;
    private const float maxLLegHP = 50;
    private const float maxTotalHP = 300;


    [SerializeField] private float thoraxHP = 80;
    [SerializeField] private float headHP = 20;
    [SerializeField] private float rArmHP = 50;
    [SerializeField] private float lArmHP = 50;
    [SerializeField] private float rLegHP = 50;
    [SerializeField] private float lLegHP = 50;
    [SerializeField] private float totalHP = 300;

    private void Awake() {
        handleDamage();
        handleTotalHP();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) { //v for violence :)
            testDamage();
        }
    }

    private void testDamage() { //terrible script as it allows negative hp values
        System.Random r = new();
        switch (r.Next(0, 6)) {
            case 0:
                thoraxHP -= r.Next(1, 11);
                handleThoraxDamageColour();
                break;
            case 1:
                headHP -= r.Next(1, 11);
                handleHeadDamageColour();
                break;
            case 2:
                rArmHP -= r.Next(1, 11);
                handleRArmDamageColour();
                break;
            case 3:
                lArmHP -= r.Next(1, 11);
                handleLArmDamageColour();
                break;
            case 4:
                rLegHP -= r.Next(1, 11);
                handleRLegDamageColour();
                break;
            case 5:
                lLegHP -= r.Next(1, 11);
                handleLLegDamageColour();
                break;
        }
        handleTotalHP();
    }

    private void handleDamage() {
        handleThoraxDamageColour();
        handleHeadDamageColour();
        handleRArmDamageColour();
        handleLArmDamageColour();
        handleRLegDamageColour();
        handleLLegDamageColour();
        handleTotalHP();
    }

    private void handleTotalHP() {
        totalHP = thoraxHP + headHP + rArmHP + lArmHP + rLegHP + lLegHP;
    }

    private void handleThoraxDamageColour() {
        if (thoraxHP > 0) {
            Thorax.color = new Color32(255, (byte)Mathf.RoundToInt(255 * (thoraxHP / maxThoraxHP)), (byte)Mathf.RoundToInt(255 * (thoraxHP / maxThoraxHP)), 255);
        }
        else {
            thoraxHP = 0;
            Thorax.color = Color.black;
        }
    }

    private void handleHeadDamageColour() {
        if (headHP > 0) {
            Head.color = new Color32(255, (byte)Mathf.RoundToInt(255 * (headHP / maxHeadHP)), (byte)Mathf.RoundToInt(255 * (headHP / maxHeadHP)), 255);
        }
        else {
            headHP = 0;
            Head.color = Color.black;
        }
    }

    private void handleRArmDamageColour() {
        if (rArmHP > 0) {
            RArm.color = new Color32(255, (byte)Mathf.RoundToInt(255 * (rArmHP / maxRArmHP)), (byte)Mathf.RoundToInt(255 * (rArmHP / maxRArmHP)), 255);
        }
        else {
            rArmHP = 0;
            RArm.color = Color.black;
        }
    }

    private void handleLArmDamageColour() {
        if (lArmHP > 0) {
            LArm.color = new Color32(255, (byte)Mathf.RoundToInt(255 * (lArmHP / maxLArmHP)), (byte)Mathf.RoundToInt(255 * (lArmHP / maxLArmHP)), 255);
        }
        else {
            lArmHP = 0;
            LArm.color = Color.black;
        }
    }

    private void handleRLegDamageColour() {
        if (rLegHP > 0) {
            RLeg.color = new Color32(255, (byte)Mathf.RoundToInt(255 * (rLegHP / maxRLegHP)), (byte)Mathf.RoundToInt(255 * (rLegHP / maxRLegHP)), 255);
        }
        else {
            rLegHP = 0;
            RLeg.color = Color.black;
        }
    }

    private void handleLLegDamageColour() {
        if (lLegHP > 0) {
            LLeg.color = new Color32(255, (byte)Mathf.RoundToInt(255 * (lLegHP / maxLLegHP)), (byte)Mathf.RoundToInt(255 * (lLegHP / maxLLegHP)), 255);
        }
        else {
            lLegHP = 0;
            LLeg.color = Color.black;
        }
    }
}
