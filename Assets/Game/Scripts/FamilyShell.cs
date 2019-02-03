using System;
using UnityEngine;

namespace Game.Scripts {
    public class FamilyShell : Shell {
        public enum Family {
            Father,
            Mother,
            Baby
        }

        public Family owner;

        private void Start() {
            if (FamilyManager.Instance.hasFather && owner == Family.Father) {
                gameObject.SetActive(false);
            } else if (FamilyManager.Instance.hasMother && owner == Family.Mother) {
                gameObject.SetActive(false);
            } else if (FamilyManager.Instance.hasSister && owner == Family.Baby) {
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.layer == GameConstants.PLAYER_LAYER) {
                gameObject.SetActive(false);
                PlayerPrefs.SetInt(GetOwnerName(), 1);

                FamilyManager.Instance.UpdateInfo();
            }
        }

        private string GetOwnerName() {
            switch (owner) {
                case Family.Father:
                    return GameConstants.FATHER_SHELL_NAME;
                case Family.Mother:
                    return GameConstants.MOTHER_SHELL_NAME;
                case Family.Baby:
                    return GameConstants.SISTER_SHELL_NAME;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}