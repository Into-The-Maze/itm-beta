using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private GameObject Volume;

    void Awake()
    {
        UnityEngine.Rendering.Volume volume = GetComponent<UnityEngine.Rendering.Volume>();
    }
}
