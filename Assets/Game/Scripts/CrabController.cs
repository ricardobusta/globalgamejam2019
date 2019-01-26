using UnityEngine;

namespace Game.Scripts
{
    using System;

    [RequireComponent(typeof(Rigidbody2D))]
    public class CrabController : MonoBehaviour
    {
        public float Speed = 5;
        public float JumpSpeed = 10;

        public Shell Shell;
        public Transform ShellAnchor;

        [NaughtyAttributes.ReadOnly] public bool Grounded = false;

        private Rigidbody2D body;

        private bool hasShell;


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
            if (Math.Abs(horizontalInput) > 0.1f)
            {
                transform.localScale = new Vector3(horizontalInput, 1, 1);
            }
        }

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            hasShell = Shell != null;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (Utils.LayerOnMask(other.collider.gameObject.layer, GameConstants.GroundLayer))
            {
                Debug.Log("Collide with ground");
                foreach (var contact in other.contacts)
                {
                    if (contact.normal == Vector2.up)
                    {
                        Grounded = true;
                        return;
                    }
                }
            }
            else if (Utils.LayerOnMask(other.collider.gameObject.layer, GameConstants.ShellLayer))
            {
                Debug.Log("Collide with shell");
                //other.gameObject.SetActive(false);
                var shell = other.gameObject.GetComponent<Shell>();
                SetShell(shell);
            }
        }

        private void SetShell(Shell shell)
        {
            shell.Activate(ShellAnchor,
                () =>
                {
                    hasShell = true;
                    Shell = shell;
                });
        }

        private void RemoveShell()
        {
            hasShell = false;
            Shell.Deactivate();
            Shell = null;
        }

        private void Update()
        {
            if (hasShell)
            {
                var t = Shell.transform;
                t.position = ShellAnchor.position;
                t.rotation = ShellAnchor.rotation;
            }
        }
    }
}