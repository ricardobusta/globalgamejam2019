using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class FamilyShell : MonoBehaviour
{
    public enum Family
    {
        father,
        mother,
        sister
    }

    public Family owner;

    private void Start()
    {
        if (FamilyManager.Instance.hasFather && owner == Family.father)
        {
            gameObject.SetActive(false);
        }

        else if (FamilyManager.Instance.hasMother && owner == Family.mother)
        {
            gameObject.SetActive(false);
        }

        else if (FamilyManager.Instance.hasSister && owner == Family.sister)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == GameConstants.PlayerLayer)
        {
            gameObject.SetActive(false);
            PlayerPrefs.SetInt(GetOwnerName(), 1);

            FamilyManager.Instance.UpdateInfo();
        }
    }

    private string GetOwnerName()
    {
        switch (owner)
        {
            case Family.father:
                return GameConstants.fatherShellName;
                break;
            case Family.mother:
                return GameConstants.motherShellName;
                break;
            case Family.sister:
                return GameConstants.sisterShellName;
                break;
        }

        return string.Empty;
    }
}