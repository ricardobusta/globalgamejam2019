namespace Game.Scripts {
    using System;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;
    using Random = UnityEngine.Random;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Shell : MonoBehaviour {
        public Collider2D shellCollider;

        private static Tweener tweener;

        private new Rigidbody2D rigidbody;

        public ShellType Type;

        public Cooldown SpecialCooldown;

        private void Awake() {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void ActivateInstant(Transform target) {
            shellCollider.enabled = false;
            rigidbody.bodyType = RigidbodyType2D.Static;
        }

        public void Activate(Transform target, Action afterActivation) {
            ActivateInstant(target);
            var transform1 = transform;
            var position = transform1.position;
            var rotation = transform1.rotation;
            tweener = DOVirtual.Float(0, 1, 0.5f, t => {
                transform1.position = Vector3.Lerp(position, target.position, t);
                transform1.rotation = Quaternion.Lerp(rotation, target.rotation, t);
            });
            tweener.onComplete += afterActivation.Invoke;
        }

        public void Deactivate() {
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            shellCollider.enabled = true;
            gameObject.layer = GameConstants.DisabledShellLayer;
            rigidbody.velocity = new Vector2(Random.Range(-1.0f, 1.0f), 2);
            transform.DORotate(Vector3.zero, 0.5f);
            DOVirtual.DelayedCall(1, () => { gameObject.layer = GameConstants.ShellLayer; });
        }

        private void Update() {
            SpecialCooldown.Update(Time.deltaTime);
        }

        public enum ShellType {
            Basic,
            Helmet,
            Anemone,
            Lamp
        }
    }
}