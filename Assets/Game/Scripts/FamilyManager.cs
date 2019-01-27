namespace Game.Scripts
{
    using UnityEngine;

    public class FamilyManager : MonoBehaviour
    {
        private static FamilyManager instance;

        public bool hasFather;
        public bool hasMother;
        public bool hasSister;

        public FinalGate finalGate;
        public ShellHud shelllHud;
        
        public static FamilyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<FamilyManager>();
                }

                return instance;
            }
        }

        private void Start()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            hasFather = PlayerPrefs.GetInt(GameConstants.fatherShellName) == 1;
            hasMother = PlayerPrefs.GetInt(GameConstants.motherShellName) == 1;
            hasSister = PlayerPrefs.GetInt(GameConstants.sisterShellName) == 1;

            finalGate.UpdateGate(hasFather, hasMother, hasSister);
            shelllHud.UpdateHud(hasFather, hasMother, hasSister);
        }
        
        public static void ResetData()
        {
            PlayerPrefs.SetInt(GameConstants.fatherShellName, 0);
            PlayerPrefs.SetInt(GameConstants.motherShellName, 0);
            PlayerPrefs.SetInt(GameConstants.sisterShellName, 0);
        }
    }
}