using UnityEngine;

namespace Game.Scripts {
    using System;

    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class CrabController : MonoBehaviour {
        public Cooldown punchCooldown;

        private float punchRemainingCooldown;

        private const int SpeedAnimationIndex = 0;

        public float Speed = 5;
        public float JumpSpeed = 10;

        public Shell Shell;
        public Transform ShellAnchor;

        [NaughtyAttributes.ReadOnly]
        public bool hasShell;

        [NaughtyAttributes.ReadOnly]
        public bool Grounded = false;

        private Rigidbody2D body;
        private Animator animator;

        public float timeSinceJump = 0;

        public float dashRemaining = 0;

        public event Action Died;
        public event Action<bool> GroundedStateChanged;

        public bool Walking;

        public Rigidbody2D Anemone;
        private static readonly int AttackProp = Animator.StringToHash("Attack");
        private static readonly int HelmetSpecial = Animator.StringToHash("HelmetSpecial");
        private static readonly int AnemoneSpecial = Animator.StringToHash("AnemoneSpecial");
        private static readonly int LampSpecial = Animator.StringToHash("LampSpecial");
        private static readonly int SpeedProp = Animator.StringToHash("Speed");
        private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        private static readonly int GroundedProp = Animator.StringToHash("Grounded");

        public void Attack() {
            if (hasShell) {
                if (Shell.SpecialCooldown.Trigger()) {
                    InvokeSpecial(Shell.Type);
                }
            } else if (punchCooldown.Trigger()) {
                animator.SetTrigger(AttackProp);
            }
        }

        public void ThrowAnemone() {
            SFXManager.PlaySound(SFXManager.SFX.throwAnemone);
            var anemone = Instantiate(Anemone, transform.position, Quaternion.identity);
            anemone.velocity = new Vector2(transform.localScale.x * 2, 4);
        }

        private void InvokeSpecial(Shell.ShellType shellType) {
            Debug.Log("Special");
            switch (shellType) {
                case Shell.ShellType.Basic:
                    break;
                case Shell.ShellType.Helmet:
                    Debug.Log("Helmet Special");
                    animator.SetTrigger(HelmetSpecial);
                    break;
                case Shell.ShellType.Anemone:
                    animator.SetTrigger(AnemoneSpecial);
                    break;
                case Shell.ShellType.Lamp:
                    animator.SetTrigger(LampSpecial);
                    dashRemaining = 5.0f / 6.0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shellType), shellType, null);
            }
        }

        public void Handle(float horizontalInput, float verticalInput) {
            var horizontalSpeed = horizontalInput * Speed;
            var jump = verticalInput * JumpSpeed;

            float verticalSpeed = 0;

            if (dashRemaining <= 0) {
                body.gravityScale = 1;
                if (Grounded && jump > 0) {
                    verticalSpeed = jump;
                    timeSinceJump = 0;
                    SetGroundedState(false);
                } else {
                    verticalSpeed = body.velocity.y;
                }
            } else {
                body.gravityScale = 0;
                horizontalSpeed = transform.localScale.x * Speed * 2;
                verticalSpeed = 0;
            }

            Walking = Grounded && dashRemaining <= 0 && Mathf.Abs(horizontalSpeed) > 0.1f;

            body.velocity = new Vector2(horizontalSpeed, verticalSpeed);
            animator.SetFloat(SpeedProp, Mathf.Abs(horizontalSpeed));
            if (Math.Abs(horizontalInput) > 0.1f) {
                transform.localScale = new Vector3(horizontalInput, 1, 1);
            }
        }

        public void PlayHelmetWind() {
            SFXManager.PlaySound(SFXManager.SFX.helmetWind);
        }

        public void PlayHelmetHit() {
            SFXManager.PlaySound(SFXManager.SFX.helmetHit);
        }

        public void PlayPunchWind() {
            SFXManager.PlaySound(SFXManager.SFX.punchWind);
        }

        public void PlayDashEletric() {
            SFXManager.PlaySound(SFXManager.SFX.dashEletric);
        }

        private void Awake() {
            body = GetComponent<Rigidbody2D>();
            hasShell = Shell != null;
            animator = GetComponent<Animator>();
        }

        private void Start() {
            if (hasShell) {
                Shell.ActivateInstant(ShellAnchor);
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            HandleCollision(other);
        }

        private void OnCollisionStay2D(Collision2D other) {
            HandleCollision(other);
        }

        private void HandleCollision(Collision2D other) {
            switch (other.collider.gameObject.layer) {
                case GameConstants.GroundLayer:
                    if (Grounded || timeSinceJump < 0.5f) {
                        return;
                    }

                    foreach (var contact in other.contacts) {
                        if (contact.normal == Vector2.up) {
                            SetGroundedState(true);
                            return;
                        }
                    }

                    break;
                case GameConstants.ShellLayer:
                    //other.gameObject.SetActive(false);
                    SetShell(other.gameObject.GetComponent<Shell>());
                    break;
                case GameConstants.HazardLayer:
                    Die();
                    break;
            }
        }

        private void SetGroundedState(bool grounded) {
            Grounded = grounded;
            animator.SetBool(GroundedProp, grounded);
            GroundedStateChanged?.Invoke(grounded);
        }

        private void SetShell(Shell shell) {
            RemoveShell();

            SFXManager.PlaySound(SFXManager.SFX.collectShell);
            shell.Activate(ShellAnchor,
                () => {
                    hasShell = true;
                    if (Shell != null) {
                        Shell.Deactivate();
                    }

                    Shell = shell;
                });
        }

        private void RemoveShell() {
            if (hasShell) {
                hasShell = false;
                Shell.Deactivate();
                Shell = null;
            }
        }

        public void Die() {
            Died?.Invoke();
            RemoveShell();
            gameObject.SetActive(false);
        }

        private void Update() {
            punchCooldown.Update(Time.deltaTime);

            timeSinceJump += Time.deltaTime;

            if (!Grounded) {
                animator.SetFloat(VerticalSpeed, body.velocity.y);
            }

            if (dashRemaining > 0) {
                dashRemaining -= Time.deltaTime;
            }

            if (hasShell) {
                var t = Shell.transform;
                t.position = ShellAnchor.position;
                t.rotation = ShellAnchor.rotation;
            }
        }
    }
}