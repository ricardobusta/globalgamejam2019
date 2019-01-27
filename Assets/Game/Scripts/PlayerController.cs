using UnityEngine;

namespace Game.Scripts
{
    using System;
    using UnityEngine.SceneManagement;

    [RequireComponent(typeof(CrabController))]
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController instance;

        public static PlayerController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PlayerController>();
                }

                return instance;
            }
        }

        private CrabController controller;

        private bool upInUse;

        // Start is called before the first frame update
        private void Awake()
        {
            controller = GetComponent<CrabController>();

            controller.Died += () =>
            {
                SFXManager.StopOnce(SFXManager.SFX.walk);
                SceneManager.LoadScene("Stage1");
            };
        }

        // Update is called once per frame
        private void Update()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");
            var attackInput = Input.GetButtonDown("Fire1");
            var specialInput = Input.GetButtonDown("Fire2");

            if (attackInput)
            {
                controller.Attack();
            }

            if (specialInput)
            {
                controller.Special();
            }

            if (controller.Walking)
            {
                SFXManager.PlayOnce(SFXManager.SFX.walk);
            }
            else
            {
                SFXManager.StopOnce(SFXManager.SFX.walk);
            }

            controller.Handle(horizontalInput, JumpAxisDown(verticalInput));
        }

        private int JumpAxisDown(float verticalInput)
        {
            var v = 0;
            if (!upInUse && verticalInput > 0)
            {
                upInUse = true;
                v = 1;
            }

            if (upInUse && verticalInput <= 0)
            {
                upInUse = false;
            }

            return v;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.gameObject.layer == GameConstants.EnemyLayer)
            {
                controller.Die();
            }
        }
    }
}