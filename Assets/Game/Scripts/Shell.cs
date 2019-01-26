namespace Game.Scripts
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    public class Shell : MonoBehaviour
    {
        public Collider2D shellCollider;

        private Tweener tweener;

        public void Activate(Transform target, Action afterActivation)
        {
            tweener?.Kill();

            shellCollider.enabled = false;
            var transform1 = transform;
            var position = transform1.position;
            var rotation = transform1.rotation;
            tweener = DOVirtual.Float(0, 1, 0.5f, t =>
            {
                transform1.position = Vector3.Lerp(position, target.position, t);
                transform1.rotation = Quaternion.Lerp(rotation, target.rotation, t);
            });
            tweener.onComplete += afterActivation.Invoke;
        }

        public void Deactivate()
        {
            shellCollider.enabled = true;
        }
    }
}