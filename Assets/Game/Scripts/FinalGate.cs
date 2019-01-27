namespace Game.Scripts
{
    using UnityEngine;

    public class FinalGate : MonoBehaviour
    {
        private bool open;

        public SpriteRenderer fatherIcon;
        public SpriteRenderer motherIcon;
        public SpriteRenderer sisterIcon;

        public void UpdateGate(bool father, bool mother, bool sister)
        {
            fatherIcon.gameObject.SetActive(father);
            motherIcon.gameObject.SetActive(mother);
            sisterIcon.gameObject.SetActive(sister);
            
            if (father && mother && sister)
            {
                open = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (open && other.gameObject.layer == GameConstants.PlayerLayer)
            {
                gameObject.SetActive(false);
            }
        }
    }
}