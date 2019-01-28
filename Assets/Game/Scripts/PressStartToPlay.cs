using System;
using Game.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressStartToPlay : MonoBehaviour
{
    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var b = Input.GetButton("Fire1") ||
                Input.GetButton("Fire2") ||
                Input.GetButton("Submit") ||
                Input.GetButton("Cancel");
        if (Math.Abs(h) > 0.1f || Math.Abs(v) > 0.1f || b)
        {
            FamilyManager.ResetData();
            SceneManager.LoadScene("Cinematic");
        }
    }
}