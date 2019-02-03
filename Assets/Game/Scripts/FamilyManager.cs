using UnityEngine.SceneManagement;

namespace Game.Scripts {
    using UnityEngine;

    public class FamilyManager : MonoBehaviour {
        private static FamilyManager instance;

        public bool hasFather;
        public bool hasMother;
        public bool hasSister;

        public FinalGate finalGate;
        public ShellHud shelllHud;

        public static FamilyManager Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<FamilyManager>();
                }

                return instance;
            }
        }

        private void Start() {
            UpdateInfo();
        }

        public void UpdateInfo() {
            hasFather = PlayerPrefs.GetInt(GameConstants.FATHER_SHELL_NAME) == 1;
            hasMother = PlayerPrefs.GetInt(GameConstants.MOTHER_SHELL_NAME) == 1;
            hasSister = PlayerPrefs.GetInt(GameConstants.SISTER_SHELL_NAME) == 1;

            finalGate.UpdateGate(hasFather, hasMother, hasSister);
            shelllHud.UpdateHud(hasFather, hasMother, hasSister);
        }

        public static void ResetData() {
            PlayerPrefs.SetInt(GameConstants.FATHER_SHELL_NAME, 0);
            PlayerPrefs.SetInt(GameConstants.MOTHER_SHELL_NAME, 0);
            PlayerPrefs.SetInt(GameConstants.SISTER_SHELL_NAME, 0);
        }
        
        private static void CheatData() {
            PlayerPrefs.SetInt(GameConstants.FATHER_SHELL_NAME, 1);
            PlayerPrefs.SetInt(GameConstants.MOTHER_SHELL_NAME, 1);
            PlayerPrefs.SetInt(GameConstants.SISTER_SHELL_NAME, 1);
        }


        private void Update() {
            if (Input.GetKeyDown(KeyCode.F1)) {
                ResetData();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(KeyCode.F2)) {
                CheatData();
                UpdateInfo();
            }
        }
    }
}