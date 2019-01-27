namespace Game.Scripts
{
    using UnityEngine;

    public class SelfDestructOnTime : MonoBehaviour
    {
        public float time;

        private void Update()
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}