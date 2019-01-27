namespace Game.Scripts
{
    using UnityEngine;

    public class EnemyProjectile: MonoBehaviour
    {
        public float direction;
        
        private void Update()
        {
            transform.position += Vector3.right * direction * Time.deltaTime * 3;
        }
    }
}