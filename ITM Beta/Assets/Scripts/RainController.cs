using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Random = System.Random;

public class RainController : MonoBehaviour
{
    private Random r = new Random();
    private ParticleSystem rainEmitter;
    private Quaternion target = Quaternion.identity;
    private float speed = 20f;
    private float rate = 800;
    private int next = -20;

    void Awake()
    {
        rainEmitter = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (transform.rotation != target) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, speed * Time.deltaTime);
            rate = Mathf.Lerp(rate, 800 + (next * -40f), speed * 1000 * Time.deltaTime);
#pragma warning disable CS0618 // Type or member is obsolete
            rainEmitter.emissionRate = rate;
            rainEmitter.startSpeed = Mathf.Lerp(rainEmitter.startSpeed, (next / -2) + 50, speed * 100 * Time.deltaTime);
#pragma warning restore CS0618 // Type or member is obsolete
        }
        else {
            next = r.Next(-60, -5);
            target = Quaternion.Euler(0f, 0f, next);
            speed = r.Next(4, 20);
        }
    }
}
