using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "Enemy")
	    	Destroy(gameObject.gameObject);
   	}	
}
