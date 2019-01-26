namespace Game.Scripts
{
    using UnityEngine;

    public class Shell : MonoBehaviour
    {
        public Collider2D shellCollider;
        public void Activate()
        {
            shellCollider.enabled = false;
        }

        public void Deactivate()
        {
            shellCollider.enabled = true;
        }
    }
}