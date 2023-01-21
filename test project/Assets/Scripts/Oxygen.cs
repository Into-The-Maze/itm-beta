using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    public Slider oxygenBar;
    static float maxOxygen = 50f;
    float oxygen = maxOxygen;

    void Start()
    {
        oxygenBar.maxValue = maxOxygen;
    }

    void Update()
    {
        oxygenBar.value = oxygen;
    }

    //oxygen for now is a placeholder, but it should go down when:
    //underwater
    //ADSing
    //using melee
    //when out of O2, if you are underwater you take health damage
    //if not underwater, your aim becomes less accurate/weapon swings slower
}
