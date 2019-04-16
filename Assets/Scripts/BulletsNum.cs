using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsNum : MonoBehaviour
{
    public GameObject Weapon;
    public Text numofbullets;

    private void Awake()
    {
        numofbullets = GetComponent<Text>();
    }

    void Update()
    {
        if (Weapon.transform.GetChild(0) != null)
            numofbullets.text = "Bullets: " + Weapon.transform.GetChild(0).GetComponentInChildren<WeaponParameters>()
                                    .CurNumberShot.ToString();
    }
}
