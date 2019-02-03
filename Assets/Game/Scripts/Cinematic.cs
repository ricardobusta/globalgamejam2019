using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour {
    public Image BlackScreen;
    public CanvasGroup GGJLogo;
    public GameObject Enemy;
    public List<GameObject> DisableStuff;

    public List<Animator> BalloonAnimators;
    private static readonly int What = Animator.StringToHash("What");

    void Start() {
        GGJLogo.gameObject.SetActive(true);
        BlackScreen.gameObject.SetActive(true);
        Enemy.gameObject.SetActive(false);

        GGJLogo.alpha = 0;

        // DO NOT TRY THIS AT HOME! GAME JAM STUFF ONLY

        // Delay
        DOVirtual.DelayedCall(1,
            // Show
            () => DOVirtual.Float(0, 1, 1, t => { GGJLogo.alpha = t; })
                // Hide
                .onComplete += () => DOVirtual.DelayedCall(0.5f, () =>
                DOVirtual.Float(1, 0, 1, t => { GGJLogo.alpha = t; })
                    // Hide black screen
                    .onComplete += () => {
                    DOVirtual.Float(1, 0, 1, t => { BlackScreen.color = new Color(0, 0, 0, t); })
                            // Wait
                            .onComplete +=
                        () => DOVirtual.DelayedCall(1, () => {
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
                                    () => DOVirtual.DelayedCall(1, () => {
                                        // Set Balloons to ?
                                        foreach (var animator in BalloonAnimators) {
                                            animator.SetTrigger(What);
                                        }

                                        SceneManager.LoadScene("Stage1");
                                    });
                            };
                        });
                }));
    }
}