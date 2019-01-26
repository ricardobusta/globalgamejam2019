using UnityEngine;

namespace Game.Scripts {
    [RequireComponent(typeof(Rigidbody2D))]
    public class CrabController : MonoBehaviour {
        public float Speed = 5;
        public float JumpSpeed = 10;

        public Transform Shell;
        public Transform ShellAnchor;
        
        [NaughtyAttributes.ReadOnly]
        public bool Grounded = false;

        public LayerMask GroundLayer = 8;

        private bool hasShell = true;

        private Rigidbody2D body;

        public void Handle(float horizontalInput, float verticalInput) {
            var horizontalSpeed = horizontalInput * Speed;
            var jump = verticalInput * JumpSpeed;

            float verticalSpeed = 0;

            if (Grounded && jump > 0) {
                verticalSpeed = jump;
                Grounded = false;
            } else {
                verticalSpeed = body.velocity.y;
            }

            body.velocity = new Vector2(horizontalSpeed, verticalSpeed);
        }

        private void Awake() {
            body = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if ((GroundLayer | 1 << other.collider.gameObject.layer) != 0) {
                foreach (var contact in other.contacts) {
                    if (contact.normal == Vector2.up) {
                        Grounded = true;
                        return;
                    }
                }
            }
        }

        private void Update()
        {
            if (hasShell)
            {
                Shell.position = ShellAnchor.position;
            }
        }
    }
}