using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts {
    [RequireComponent(typeof(CrabController))]
    public class EnemyCrabController : MonoBehaviour {
        [NaughtyAttributes.BoxGroup("Behaviours")]
        public bool Roam;

        [NaughtyAttributes.ShowIf("Roam")]
        public float RoamSpeed;

        [NaughtyAttributes.ShowIf("Roam")]
        public float RoamIdleTime;

        [NaughtyAttributes.BoxGroup("Behaviours")]
        public bool Jump;

        [NaughtyAttributes.ShowIf("Jump")]
        public float JumpIdleTime;

        [NaughtyAttributes.BoxGroup("Behaviours")]
        public bool Chase;

        [NaughtyAttributes.ShowIf("Chase")]
        public float AggroThreshold;

        [NaughtyAttributes.ShowIf("Chase")]
        public float ChaseSpeed;

        protected CrabController controller;

        public EnemyProjectile LaunchProjectilePrefab;

        public enum EnemyState {
            Idle,
            Roaming,
            Chasing,
            Jumping
        }

        [NaughtyAttributes.ReadOnly]
        public EnemyState State = EnemyState.Idle;

        private Tween idleTween;

        private float roamDirection;

        private float wallDirection;

        private float chaseDirection;

        private void Awake() {
            controller = GetComponent<CrabController>();

            controller.Died += () => { SFXManager.PlaySound(SFXManager.SFX.damageEnemy); };

            roamDirection = -1;
        }

        private void Update() {
            var playerPosition = PlayerController.Instance.transform.position;

            if (Chase) {
                var position = transform.position;
                var distance = Vector3.Distance(playerPosition, position);
                var xDistance = playerPosition.x - position.x;
                if (distance < AggroThreshold && Mathf.Abs(xDistance) > 0.1f) {
                    State = EnemyState.Chasing;
                    chaseDirection = Mathf.Sign(xDistance);
                } else {
                    if (State == EnemyState.Chasing) {
                        State = EnemyState.Idle;
                    }
                }
            }

            switch (State) {
                case EnemyState.Idle:
                    controller.Handle(0, 0);

                    if (idleTween == null) {
                        if (Roam) {
                            idleTween = DOVirtual.DelayedCall(RoamIdleTime, () => {
                                State = EnemyState.Roaming;
                                idleTween = null;
                            });
                        } else if (Jump) {
                            idleTween = DOVirtual.DelayedCall(JumpIdleTime, () => {
                                State = EnemyState.Jumping;
                                idleTween = null;
                            });
                        }
                    }

                    break;
                case EnemyState.Roaming:
                    controller.Speed = RoamSpeed;
                    controller.Handle(roamDirection, 0);
                    if (Math.Abs(wallDirection - roamDirection) < 0.1f) {
                        FlipRoam();
                    }

                    break;
                case EnemyState.Chasing:
                    controller.Speed = ChaseSpeed;
                    var vertical = Mathf.Abs(wallDirection) > 0.1f ? 1 : 0;
                    var horizontal = chaseDirection;
                    controller.Handle(horizontal, vertical);
                    break;
                case EnemyState.Jumping:
                    controller.Handle(0, 1);
                    State = EnemyState.Idle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void OnTriggerEnter2D(Collider2D other) {
            // Kill by player punch
            switch (other.gameObject.layer) {
                case GameConstants.PLAYER_LAYER:
                    controller.Die();
                    break;
                case GameConstants.SHELL_LAYER:
                    // Kill by helmet power
                    controller.Die();
                    var transform1 = transform;
                    var projectile = Instantiate(LaunchProjectilePrefab, transform1.position, transform1.rotation);
                    projectile.direction = PlayerController.Instance.transform.localScale.x;
                    break;
                case GameConstants.PROJECTILE_LAYER:
                    // Kill by projectile
                    controller.Die();
                    other.gameObject.SetActive(false);
                    break;
                case GameConstants.ENEMY_BARRIER:
                    // Hit barrier while roaming
                    if (State == EnemyState.Roaming) {
                        wallDirection = roamDirection;
                        controller.Speed = 0;
                        FlipRoam();
                    }

                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            HandleCollision(other);
        }

        private void OnCollisionStay2D(Collision2D other) {
            HandleCollision(other);
        }

        private void HandleCollision(Collision2D other) {
            if (other.collider.gameObject.layer == GameConstants.GROUND_LAYER) {
                wallDirection = 0;
                foreach (var contact in other.contacts) {
                    var contactX = contact.normal.x;
                    if (Math.Abs(contactX) > 0.001f) {
                        wallDirection = -contactX;
                    }
                }
            }
        }

        private void FlipRoam() {
            roamDirection *= -1;
            State = EnemyState.Idle;
        }
    }
}