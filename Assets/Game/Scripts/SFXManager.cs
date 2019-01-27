using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource Walk;
    public AudioSource Jump;
    public AudioSource Land;
    public AudioSource Damage;
    public AudioSource HelmetWind;
    public AudioSource HelmetHit;
    public AudioSource PunchWind;

    private static SFXManager instance;

    private Dictionary<SFX, AudioSource> SFXMap;

    private static SFXManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SFXManager>();
            }

            return instance;
        }
    }

    public static void PlaySound(SFX sound)
    {
        Instance.SFXMap[sound].Play();
    }

    public static void PlayOnce(SFX sound)
    {
        var sfx = Instance.SFXMap[sound];
        if (!sfx.isPlaying)
        {
            sfx.Play();
        }
    }

    public static void StopSound(SFX sound)
    {
        Instance.SFXMap[sound].Stop();
    }

    public static void StopOnce(SFX sound)
    {
        var sfx = Instance.SFXMap[sound];
        if (sfx.isPlaying)
        {
            sfx.Stop();
        }
    }

    private void Awake()
    {
        SFXMap = new Dictionary<SFX, AudioSource>()
        {
            {SFX.walk, Walk},
            {SFX.jump, Jump},
            {SFX.land, Land},
            {SFX.damage, Damage},
            {SFX.helmetWind, HelmetWind},
            {SFX.helmetHit, HelmetHit},
            {SFX.punchWind, PunchWind},
        };
    }

    public enum SFX
    {
        walk,
        jump,
        land,
        damage,
        helmetWind,
        helmetHit,
        punchWind,
    }
}