using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
	public uint Speed = 5;
	public GameObject Head;
	public Sprite[] SpriteHeads;
	public GameObject Body;
	public Sprite[] SpriteBodies;
	public GameObject Legs;
	public AudioClip PickUp;
	public GameObject WeaponPos;
	public GameObject _Weapon = null;
	public AudioClip[] DeadSounds = new AudioClip[4];
	public bool _Dead = false;
	private bool _Stun = false;
	private Vector2 _Vector;
	private float _Timer = 0f;

	private void Start()
	{
		if (Head != null && SpriteHeads.Length > 0)
			Head.GetComponent<SpriteRenderer>().sprite = SpriteHeads[Random.Range(0, SpriteHeads.Length)];
		if (Body != null && SpriteBodies.Length > 0)
			Body.GetComponent<SpriteRenderer>().sprite = SpriteBodies[Random.Range(0, SpriteBodies.Length)];
	}

	private void Update()
	{
		if (_Stun == true)
		{
			_Timer += Time.deltaTime;
			if (_Timer > 5f)
			{
				_Stun = false;
				_Timer = 0;
			}
		}
	}

	public void Move(Vector2 vectorMove)
	{
		if (_Dead == false && _Stun == false)
		{
			if (vectorMove == Vector2.zero)
				Legs.GetComponent<Animator>().Play("stay");
			else
				Legs.GetComponent<Animator>().Play("go");
			gameObject.GetComponent<Rigidbody2D>().AddForce(vectorMove * Speed, ForceMode2D.Force);
		}
	}

	public void Rotate(float grad)
	{
		if (_Dead == false && _Stun == false)
		{
			transform.rotation = Quaternion.Euler(0f, 0f, grad);
		}
	}

	public void Direct(Vector3 position)
	{
		if (_Dead == false && _Stun == false)
		{
			_Vector = position - transform.position;
			_Vector.Normalize();
			float rot_z = Mathf.Atan2(_Vector.y, _Vector.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90f);
		}
	}

	public void UpElement(GameObject newElement)
	{
		if (newElement.tag == "Weapon" && _Weapon == null && _Stun == false)
		{
			gameObject.GetComponent<AudioSource>().clip = PickUp;
			gameObject.GetComponent<AudioSource>().Play();
			newElement.transform.SetParent(WeaponPos.transform);
			_Weapon = newElement;
			_Weapon.GetComponent<WeaponParameters>().SetUse(true);
			_Weapon.transform.position = WeaponPos.transform.position;
			_Weapon.transform.rotation = WeaponPos.transform.rotation;
		}
	}

	public void DownElement()
	{
		if (_Weapon != null)
		{
			_Weapon.GetComponent<WeaponParameters>().SetUse(false);
			_Weapon.transform.parent = null;
			if (_Dead == false)
				_Weapon = null;
		}
	}

	public void ToShot(string tag)
	{
        
		if (_Weapon != null && _Stun == false)
		{
			_Weapon.GetComponent<WeaponParameters>().Shot(_Vector, tag);
		}
	}

	public uint GetNumberShot()
	{
		if (_Weapon != null)
			return (_Weapon.GetComponent<WeaponParameters>().CurNumberShot);
		else
			return(0);
	}

	private void _ToDead()
	{
		gameObject.GetComponent<AudioSource>().clip = DeadSounds[Random.Range(0, DeadSounds.Length)];
		gameObject.GetComponent<AudioSource>().Play();
		gameObject.GetComponent<Rigidbody2D>().AddTorque(Speed / 5f, ForceMode2D.Impulse);
		DownElement();
	}
	public bool GetDead()
	{
		return (_Dead);
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ammo") && collision.gameObject.tag != gameObject.tag)
		{
			Destroy(collision.gameObject);
   			if (_Dead == false)
   			{
   				_Dead = true;
				_ToDead();
				Destroy(gameObject, 1f);
   			}
		}
		if (collision.gameObject.tag == "Weapon" && gameObject.transform.tag == "Enemy")
		{
			if (collision.gameObject.GetComponent<WeaponParameters>().Saber() && _Dead == false)
			{
 				_Dead = true;
				_ToDead();
				Destroy(gameObject, 1f);
   			}
   			else if (_Dead == false)
   			{
   				_Stun = true;
   			}
		}
	}
}
