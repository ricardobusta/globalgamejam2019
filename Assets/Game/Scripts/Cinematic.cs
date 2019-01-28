using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour {
    public Image BlackScreen;
    public Image GGJLogo;
    public GameObject Enemy;
    public GameObject[] DisableStuff;

    void Start() {
        GGJLogo.gameObject.SetActive(true);
        BlackScreen.gameObject.SetActive(true);
        Enemy.gameObject.SetActive(false);

        GGJLogo.color = new Color(1, 1, 1, 0);

        // DO NOT TRY THIS AT HOME! GAME JAM STUFF ONLY

        // Delay
        DOVirtual.Float(0, 0, 1, t => { })
            // Show
            .onComplete += () =>
            DOVirtual.Float(0, 1, 1, t => { GGJLogo.color = new Color(1, 1, 1, t); })
                // Hide
                .onComplete += () =>
                DOVirtual.Float(1, 0, 1, t => { GGJLogo.color = new Color(1, 1, 1, t); })
                    // Hide black screen
                    .onComplete += () => {
                    DOVirtual.Float(1, 0, 1, t => { BlackScreen.color = new Color(0, 0, 0, t); })
                            // Wait
                            .onComplete +=
                        () => {
                            DOVirtual.Float(0, 0, 1, t => { })
                                // Enemy Attack
                                .onComplete += () => {
                                Enemy.gameObject.SetActive(true);
                                DOVirtual.Float(-6, -1, 0.5f,
                                        t => { Enemy.transform.position = new Vector3(t, 0.4f, 0); })
                                    // Steal stuff
                                    .SetEase(Ease.Linear)
                                    .onComplete += () => {
                                    DOVirtual.Float(0, 1, 0.1f, t => { BlackScreen.color = new Color(1, 1, 1, t); })
                                        .onComplete += () => {
                                        foreach (var stuff in DisableStuff) {
                                            stuff.SetActive(false);
                                        }

                                        DOVirtual.Float(1, 0, 0.1f,
                                            t => { BlackScreen.color = new Color(1, 1, 1, t); });
                                    };
                                    // Get out
                                    DOVirtual.Float(-1, 6, 0.5f,
                                                t => { Enemy.transform.position = new Vector3(t, 0.4f, 0); })
                                            .SetEase(Ease.Linear)
                                            .onComplete +=
                                        () => {
                                            DOVirtual.DelayedCall(1, () => { SceneManager.LoadScene("Stage1"); });
                                        };
                                };
                            };
                        };
                };
    }
}