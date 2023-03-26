using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public GameObject Player;

    public static HealthController h;

    #region images
    public Image Thorax;
    public Image Head;
    public Image RArm;
    public Image LArm;
    public Image RLeg;
    public Image LLeg;
    #endregion

    #region lightBleed
    public GameObject LightBleed;
    public bool thoraxIsLightBleed = false;
    public bool headIsLightBleed = false;
    public bool rArmIsLightBleed = false;  
    public bool lArmIsLightBleed = false;
    public bool rLegIsLightBleed = false;
    public bool lLegIsLightBleed = false;
    #endregion

    #region heavyBleed
    public GameObject HeavyBleed;
    public bool thoraxIsHeavyBleed = false;
    public bool headIsHeavyBleed = false;
    public bool rArmIsHeavyBleed = false;
    public bool lArmIsHeavyBleed = false;
    public bool rLegIsHeavyBleed = false;
    public bool lLegIsHeavyBleed = false;
    #endregion

    #region maxHPValues
    private const float maxThoraxHP = 80;
    private const float maxHeadHP = 20;
    private const float maxRArmHP = 50;
    private const float maxLArmHP = 50;
    private const float maxRLegHP = 50;
    private const float maxLLegHP = 50;
    private const float maxTotalHP = 300;
    #endregion

    #region currentHPValues
    [SerializeField] private float thoraxHP = 80;
    [SerializeField] private float headHP = 20;
    [SerializeField] private float rArmHP = 50;
    [SerializeField] private float lArmHP = 50;
    [SerializeField] private float rLegHP = 50;
    [SerializeField] private float lLegHP = 50;
    [SerializeField] private float totalHP = 300;
    #endregion

    private void Awake() {
        h = this;
        handleDamage();
        handleTotalHP();
    }

    #region bleedHandlers
    public void handleLightBleed(float bleedChance) {

    }

    public void handleLightBleed(float bleedChance, BodyParts targetLimb) {

    }

    public void handleHeavyBleed(float bleedChance) {

    }

    public void handleHeavyBleed(float bleedChance, BodyParts targetLimb) {

    }
    #endregion //NEED TO ADD TO THIS, BUT WONT BE HARD

    #region damageHandlers

    //can add bleed handlers to these scripts
    public void handleDamageF(float damageDealt) {
        
        damage(damageDealt + Random.Range(-Mathf.Sqrt(damageDealt), Mathf.Sqrt(damageDealt)));
        handleDamage();
    }
    public void handleDamageF(float damageDealt, BodyParts targetLimb) {
        damage(damageDealt + Random.Range(-Mathf.Sqrt(damageDealt), Mathf.Sqrt(damageDealt)), targetLimb);
        handleDamage();
    }

    private void damage(float damageDealt) { 
        System.Random r = new System.Random();
        float damageOverflow;
        switch (r.Next(0, 6)) {
            case 0:
                damageOverflow = damageThorax(damageDealt);
                if (damageOverflow > 0) 
                    damageHead(damageOverflow);
                break;
            case 1:
                damageOverflow = damageHead(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
            case 2:
                damageOverflow = damageRArm(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
            case 3:
                damageOverflow = damageLArm(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
            case 4:
                damageOverflow = damageRLeg(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
            case 5:
                damageOverflow = damageLLeg(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
        }
    }
    private void damage(float damageDealt, BodyParts targetLimb) {
        float damageOverflow;
        switch (targetLimb) {
            case BodyParts.Thorax:
                damageOverflow = damageThorax(damageDealt);
                if (damageOverflow > 0)
                    damageHead(damageOverflow);
                break;
            case BodyParts.Head:
                damageOverflow = damageHead(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
            case BodyParts.RArm:
                damageOverflow = damageRArm(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
            case BodyParts.LArm:
                damageOverflow = damageLArm(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
            case BodyParts.RLeg:
                damageOverflow = damageRLeg(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
            case BodyParts.LLeg:
                damageOverflow = damageLLeg(damageDealt);
                if (damageOverflow > 0)
                    damageThorax(damageOverflow);
                break;
        }
    }

    int getNumberOfIntactLimbs() {
        int limbsIntact = 0;
        if (headHP > 0)
            limbsIntact++;
        if (rArmHP > 0)
            limbsIntact++;
        if (lArmHP > 0)
            limbsIntact++;
        if (rLegHP > 0)
            limbsIntact++;
        if (lLegHP > 0)
            limbsIntact++;
        return limbsIntact;
    }
    void splitDamage(float damage) {
        if (headHP > 0)
            damageHead(damage);
        if (rArmHP > 0)
            damageRArm(damage);
        if (lArmHP > 0)
            damageLArm(damage);
        if (rLegHP > 0)
            damageRLeg(damage);
        if (lLegHP > 0)
            damageLLeg(damage);
    }

    private float damageThorax(float damageDealt) {
        thoraxHP -= damageDealt;
        if (thoraxHP < 0) {
            splitDamage(Mathf.Abs(thoraxHP) / getNumberOfIntactLimbs());
        }
        return 0f;
    }
    private float damageHead(float damage) {
        headHP -= damage;
        if (headHP < 0) {
            return Mathf.Abs(headHP);
        }
        return 0f;
    }
    private float damageRArm(float damage) {
        rArmHP -= damage;
        if (rArmHP < 0) {
            return Mathf.Abs(rArmHP);
        }
        return 0f;
    }
    private float damageLArm(float damage) {
        lArmHP -= damage;
        if (lArmHP < 0) {
            return Mathf.Abs(lArmHP);
        }
        return 0f;
    }
    private float damageRLeg(float damage) {
        rLegHP -= damage;
        if (rLegHP < 0) {
            return Mathf.Abs(rLegHP);
        }
        return 0f;
    }
    private float damageLLeg(float damage) {
        lLegHP -= damage;
        if (lLegHP < 0) {
            return Mathf.Abs(lLegHP);
        }
        return 0f;
    }
    #endregion

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
        if (totalHP <= 0) {
            Destroy(Player);
            Debug.Log("You died...");
        }
    }

    #region colourHandlers
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
    #endregion

    public enum BodyParts {
        Thorax,
        Head,
        RArm,
        LArm,
        RLeg,
        LLeg
    }
}
