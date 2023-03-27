using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public GameObject PlayerSprite;
    public GameObject PlayerPos;

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
    public GameObject[] LightBleedBlood;
    WaitForSeconds lightBleedDelay = new WaitForSeconds(2);
    public float lightBleedDamage;
    private bool thoraxIsLightBleed = false;
    private bool headIsLightBleed = false;
    private bool rArmIsLightBleed = false;  
    private bool lArmIsLightBleed = false;
    private bool rLegIsLightBleed = false;
    private bool lLegIsLightBleed = false;
    #endregion

    #region heavyBleed
    public GameObject[] HeavyBleedBlood;
    WaitForSeconds heavyBleedDelay = new WaitForSeconds(1);
    public float heavyBleedDamage;
    private bool thoraxIsHeavyBleed = false;
    private bool headIsHeavyBleed = false;
    private bool rArmIsHeavyBleed = false;
    private bool lArmIsHeavyBleed = false;
    private bool rLegIsHeavyBleed = false;
    private bool lLegIsHeavyBleed = false;
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
    private float thoraxHP = maxThoraxHP;
    private float headHP = maxHeadHP;
    private float rArmHP = maxRArmHP;
    private float lArmHP = maxLArmHP;
    private float rLegHP = maxRLegHP;
    private float lLegHP = maxLLegHP;
    private float totalHP = maxTotalHP;
    #endregion

    private void Awake() {
        h = this;
        handleDamage();
        handleTotalHP();
    }

    #region bleedHandlers
    public void handleLightBleed(float bleedChance) {
        if (Random.Range(0f, 100f) < bleedChance) {
            Debug.Log("light bleed");
            switch(Random.Range(0, 6)) {
                case 0:
                    if (!thoraxIsLightBleed) {
                        thoraxIsLightBleed = true;
                        StartCoroutine(thoraxLightBleed());
                    }
                    break;
                case 1:
                    if (!headIsLightBleed) {
                        headIsLightBleed = true;
                        StartCoroutine(headLightBleed());
                    }
                    break;
                case 2:
                    if (!rArmIsLightBleed) {
                        rArmIsLightBleed = true;
                        StartCoroutine(rArmLightBleed());
                    }
                    break;
                case 3:
                    if (!lArmIsLightBleed) {
                        lArmIsLightBleed = true;
                        StartCoroutine(lArmLightBleed());
                    }
                    break;
                case 4:
                    if (!rLegIsLightBleed) {
                        rLegIsLightBleed = true;
                        StartCoroutine(rLegLightBleed());
                    }
                    break;
                case 5:
                    if (!lLegIsLightBleed) {
                        lLegIsLightBleed = true;
                        StartCoroutine(lLegLightBleed());
                    }
                    break;
            }
        }
    }
    public void handleLightBleed(float bleedChance, BodyParts targetLimb) {
        if (Random.Range(0f, 100f) < bleedChance) {
            switch (targetLimb) {
                case BodyParts.Thorax:
                    if (!thoraxIsLightBleed) {
                        thoraxIsLightBleed = true;
                        StartCoroutine(thoraxLightBleed());
                    }
                    break;
                case BodyParts.Head:
                    if (!headIsLightBleed) {
                        headIsLightBleed = true;
                        StartCoroutine(headLightBleed());  
                    }
                    break;
                case BodyParts.RArm:
                    if (!rArmIsLightBleed) {
                        rArmIsLightBleed = true;
                        StartCoroutine(rArmLightBleed());
                    }
                    break;
                case BodyParts.LArm:
                    if (!lArmIsLightBleed) {
                        lArmIsLightBleed = true;
                        StartCoroutine(lArmLightBleed());
                    }
                    break;
                case BodyParts.RLeg:
                    if (!rLegIsLightBleed) {
                        rLegIsLightBleed = true;
                        StartCoroutine(rLegLightBleed());
                    }
                    break;
                case BodyParts.LLeg:
                    if (!lLegIsLightBleed) {
                        lLegIsLightBleed = true;
                        StartCoroutine(lLegLightBleed());
                    }
                    break;
            }
        }
    }
    public void handleHeavyBleed(float bleedChance) {
        if (Random.Range(0f, 100f) < bleedChance) {
            Debug.Log("heavy bleed");
            switch (Random.Range(0, 6)) {
                case 0:
                    if (!thoraxIsHeavyBleed) {
                        thoraxIsHeavyBleed = true;
                        StartCoroutine(thoraxHeavyBleed());
                    }
                    break;
                case 1:
                    if (!headIsHeavyBleed) {
                        headIsHeavyBleed = true;
                        StartCoroutine(headHeavyBleed());
                    }
                    break;
                case 2:
                    if (!rArmIsHeavyBleed) {
                        rArmIsHeavyBleed = true;
                        StartCoroutine(rArmHeavyBleed());
                    }
                    break;
                case 3:
                    if (!lArmIsHeavyBleed) {
                        lArmIsHeavyBleed = true;
                        StartCoroutine(lArmHeavyBleed());
                    }
                    break;
                case 4:
                    if (!rLegIsHeavyBleed) {
                        rLegIsHeavyBleed = true;
                        StartCoroutine(rLegHeavyBleed());
                    }
                    break;
                case 5:
                    if (!lLegIsHeavyBleed) {
                        lLegIsHeavyBleed = true;
                        StartCoroutine(lLegHeavyBleed());
                    }
                    break;
            }
        }
    }
    public void handleHeavyBleed(float bleedChance, BodyParts targetLimb) {
        if (Random.Range(0f, 100f) < bleedChance) {
            switch (targetLimb) {
                case BodyParts.Thorax:
                    if (!thoraxIsHeavyBleed) {
                        thoraxIsHeavyBleed = true;
                        StartCoroutine(thoraxHeavyBleed());
                    }
                    break;
                case BodyParts.Head:
                    if (!headIsHeavyBleed) {
                        headIsHeavyBleed = true;
                        StartCoroutine(headHeavyBleed());
                    }
                    break;
                case BodyParts.RArm:
                    if (!rArmIsHeavyBleed) {
                        rArmIsHeavyBleed = true;
                        StartCoroutine(rArmHeavyBleed());
                    }
                    break;
                case BodyParts.LArm:
                    if (!lArmIsHeavyBleed) {
                        lArmIsHeavyBleed = true;
                        StartCoroutine(lArmHeavyBleed());
                    }
                    break;
                case BodyParts.RLeg:
                    if (!rLegIsHeavyBleed) {
                        rLegIsHeavyBleed = true;
                        StartCoroutine(rLegHeavyBleed());
                    }
                    break;
                case BodyParts.LLeg:
                    if (!lLegIsHeavyBleed) {
                        lLegIsHeavyBleed = true;
                        StartCoroutine(lLegHeavyBleed());
                    }
                    break;
            }
        }
    }

    #region bleedCoroutines
    IEnumerator thoraxLightBleed() {
        while (thoraxIsLightBleed) {
            yield return lightBleedDelay;
            Instantiate(LightBleedBlood[Random.Range(0, LightBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(lightBleedDamage, BodyParts.Thorax);
        }
    }
    IEnumerator headLightBleed() {
        while (headIsLightBleed) {
            yield return lightBleedDelay;
            Instantiate(LightBleedBlood[Random.Range(0, LightBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(lightBleedDamage, BodyParts.Head);
        }
    }
    IEnumerator rArmLightBleed() {
        while (rArmIsLightBleed) {
            yield return lightBleedDelay;
            Instantiate(LightBleedBlood[Random.Range(0, LightBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(lightBleedDamage, BodyParts.RArm);
        }
    }
    IEnumerator lArmLightBleed() {
        while (lArmIsLightBleed) {
            yield return lightBleedDelay;
            Instantiate(LightBleedBlood[Random.Range(0, LightBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(lightBleedDamage, BodyParts.LArm);
        }
    }
    IEnumerator rLegLightBleed() {
        while (rLegIsLightBleed) {
            yield return lightBleedDelay;
            Instantiate(LightBleedBlood[Random.Range(0, LightBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(lightBleedDamage, BodyParts.RLeg);
        }
    }
    IEnumerator lLegLightBleed() {
        while (lLegIsLightBleed) {
            yield return lightBleedDelay;
            Instantiate(LightBleedBlood[Random.Range(0, LightBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(lightBleedDamage, BodyParts.LLeg);
        }
    }

    IEnumerator thoraxHeavyBleed() {
        while (thoraxIsHeavyBleed) {
            yield return heavyBleedDelay;
            Instantiate(HeavyBleedBlood[Random.Range(0, HeavyBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(heavyBleedDamage, BodyParts.Thorax);
        }
    }
    IEnumerator headHeavyBleed() {
        while (headIsHeavyBleed) {
            yield return heavyBleedDelay;
            Instantiate(HeavyBleedBlood[Random.Range(0, HeavyBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(heavyBleedDamage, BodyParts.Head);
        }
    }
    IEnumerator rArmHeavyBleed() {
        while (rArmIsHeavyBleed) {
            yield return heavyBleedDelay;
            Instantiate(HeavyBleedBlood[Random.Range(0, HeavyBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(heavyBleedDamage, BodyParts.RArm);
        }
    }
    IEnumerator lArmHeavyBleed() {
        while (lArmIsHeavyBleed) {
            yield return heavyBleedDelay;
            Instantiate(HeavyBleedBlood[Random.Range(0, HeavyBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(heavyBleedDamage, BodyParts.LArm);
        }
    }
    IEnumerator rLegHeavyBleed() {
        while (rLegIsHeavyBleed) {
            yield return heavyBleedDelay;
            Instantiate(HeavyBleedBlood[Random.Range(0, HeavyBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(heavyBleedDamage, BodyParts.RLeg);
        }
    }
    IEnumerator lLegHeavyBleed() {
        while (lLegIsHeavyBleed) {
            yield return heavyBleedDelay;
            Instantiate(HeavyBleedBlood[Random.Range(0, HeavyBleedBlood.Length)], PlayerPos.transform.position, Quaternion.identity);
            handleDamageF(heavyBleedDamage, BodyParts.LLeg);
        }
    }
    #endregion

    #endregion

    #region damageHandlers

    public void handleDamageF(float damageDealt) {
        damage(damageDealt + Random.Range(-Mathf.Sqrt(damageDealt), Mathf.Sqrt(damageDealt)));
        handleDamage();
    }
    public void handleDamageF(float damageDealt, BodyParts targetLimb) {
        damage(damageDealt + Random.Range(-Mathf.Sqrt(damageDealt), Mathf.Sqrt(damageDealt)), targetLimb);
        handleDamage();
    }

    private void damage(float damageDealt) { 
        float damageOverflow;
        switch (Random.Range(0, 6)) {
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

    #region damageFunctions
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
            Destroy(PlayerSprite);
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
