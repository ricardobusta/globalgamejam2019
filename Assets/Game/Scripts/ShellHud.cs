namespace Game.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ShellHud : MonoBehaviour
    {
        public Image fatherImage;
        public Image motherImage;
        public Image sisterImage;
        
        public void UpdateHud(bool father, bool mother, bool sister)
        {
            fatherImage.gameObject.SetActive(father);
            motherImage.gameObject.SetActive(mother);
            sisterImage.gameObject.SetActive(sister);
        }
    }
}