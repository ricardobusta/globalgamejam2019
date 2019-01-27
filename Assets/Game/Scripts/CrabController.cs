using UnityEngine;

namespace Game.Scripts
{
    using System;

    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class CrabController : MonoBehaviour
    {
        public Cooldown punchCooldown;

        private float punchRemainingCooldown;

        private const int SpeedAnimationIndex = 0;

        public float Speed = 5;
        public float JumpSpeed = 10;

        public Shell Shell;
        public Transform ShellAnchor;

        [NaughtyAttributes.ReadOnly] public bool hasShell;
        [NaughtyAttributes.ReadOnly] public bool Grounded = false;

        private Rigidbody2D body;
        private Animator animator;

        public void Attack()
        {
            if (punchCooldown.Trigger())
            {
                animator.SetTrigger("Attack");
            }
        }

        public void Special()
        {
            if (hasShell)
            {
                if (Shell.SpecialCooldown.Trigger())
                {
                    InvokeSpecial(Shell.Type);
                }
            }
        }

        private void InvokeSpecial(Shell.ShellType shellType)
        {
            Debug.Log("Special");
            switch (shellType)
            {
                case Shell.ShellType.Basic:
                    break;
                case Shell.ShellType.Helmet:
                    Debug.Log("Helmet Special");
                    animator.SetTrigger("HelmetSpecial");
                    break;
                case Shell.ShellType.Anemone:
                    break;
                case Shell.ShellType.Lamp:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shellType), shellType, null);
            }
        }

        public void Handle(float horizontalInput, float verticalInput)
        {
            var horizontalSpeed = horizontalInput * Speed;
            var jump = verticalInput * JumpSpeed;

            float verticalSpeed = 0;

            if (Grounded && jump > 0)
            {
                verticalSpeed = jump;
                Grounded = false;
            }
            else
            {
                verticalSpeed = body.velocity.y;
            }

            body.velocity = new Vector2(horizontalSpeed, verticalSpeed);
            animator.SetFloat("Speed", Mathf.Abs(horizontalSpeed));
            if (Math.Abs(horizontalInput) > 0.1f)
            {
                transform.localScale = new Vector3(horizontalInput, 1, 1);
            }
        }

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            hasShell = Shell != null;
            animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            HandleCollision(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            HandleCollision(other);
        }

        private void HandleCollision(Collision2D other)
        {
            switch (other.collider.gameObject.layer)
            {
                case GameConstants.GroundLayer:
                    Debug.Log("Collide with ground");
                    foreach (var contact in other.contacts)
                    {
                        if (contact.normal == Vector2.up)
                        {
                            Grounded = true;
                            return;
                        }
                    }

                    break;
                case GameConstants.ShellLayer:
                    Debug.Log("Collide with shell");
                    //other.gameObject.SetActive(false);
                    SetShell(other.gameObject.GetComponent<Shell>());
                    break;
                case GameConstants.HazardLayer:
                    Die();
                    break;
            }
        }

        private void SetShell(Shell shell)
        {
            RemoveShell();

            shell.Activate(ShellAnchor,
                () =>
                {
                    hasShell = true;
                    if (Shell != null)
                    {
                        Shell.Deactivate();
                    }

                    Shell = shell;
                });
        }

        private void RemoveShell()
        {
            if (hasShell)
            {
                hasShell = false;
                Shell.Deactivate();
                Shell = null;
            }
        }

        public void Die()
        {
            RemoveShell();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            punchCooldown.Update(Time.deltaTime);

            if (hasShell)
            {
                var t = Shell.transform;
                t.position = ShellAnchor.position;
                t.rotation = ShellAnchor.rotation;
            }
        }
    }
}