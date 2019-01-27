using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {
    public AudioSource Walk;
    public AudioSource Jump;
    public AudioSource Land;

    private SFXManager instance;

    private Dictionary<SFX, AudioSource> SFXMap;

    public SFXManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<SFXManager>();
            }

            return instance;
        }
    }

    public void PlaySound(SFX sound) {
        SFXMap[sound].Play();
    }

    public void StopSound(SFX sound) {
        SFXMap[sound].Stop();
    }

    private void Awake() {
        SFXMap = new Dictionary<SFX, AudioSource>() {
            {SFX.walk, Walk},
            {SFX.jump, Jump},
            {SFX.land, Land},
        };
    }

    public enum SFX {
        walk,
        jump,
        land
    }
}