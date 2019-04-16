using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParameters : MonoBehaviour
{
	public bool Use = false;
	public uint ImpulseFly = 10;
	public uint CurNumberShot = 0;
	public Sprite[] SpriteFullGun = new Sprite[12];
	public Sprite[] SpriteBodyGun = new Sprite[12];
	public Sprite[] SpriteShot = new Sprite[12];
	public float[] TimeAction = new float[12];
	public uint[] NumberShot = new uint[12];
	public AudioClip[] ShotSounds = new AudioClip[12];

    //public bool playerShot;

    [SerializeField] private CharacterControllerAI characterControllerAI;
    public GameObject cumShot;

	private int Iter = 0;
	private float _Timer = 0f;
	private float _TimeDelay = 0.2f;
	private bool _Shot = true;

	private void Start()
	{
        cumShot = GameObject.Find("playerShot");
		Iter = Random.Range(0,12);
		CurNumberShot = NumberShot[Iter];
		gameObject.GetComponent<AudioSource>().clip = ShotSounds[Iter];
		if (Use == true)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = SpriteBodyGun[Iter];
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			gameObject.GetComponent<Collider2D>().isTrigger = true;
		}
		else
		{ 
			gameObject.GetComponent<SpriteRenderer>().sprite = SpriteFullGun[Iter];
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
			gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			gameObject.GetComponent<Rigidbody2D>().drag = 5f;
			gameObject.GetComponent<Rigidbody2D>().angularDrag = 5f;
			gameObject.GetComponent<Collider2D>().isTrigger = false;
		}
	}

	private void Update()
	{
		_Timer += Time.deltaTime;
		if (_Timer > _TimeDelay && CurNumberShot > 0)
		{
			_Timer = 0;
			_Shot = true;
		}
	}

	public void SetUse(bool status)
	{
		if (status == true)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = SpriteBodyGun[Iter];
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
			gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			gameObject.GetComponent<Collider2D>().isTrigger = true;
		}
		if (status == false)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = SpriteFullGun[Iter];
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
			gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			gameObject.GetComponent<Rigidbody2D>().drag = 5f;
			gameObject.GetComponent<Rigidbody2D>().angularDrag = 5f;
			gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.down * ImpulseFly, ForceMode2D.Impulse);
			gameObject.GetComponent<Rigidbody2D>().AddTorque(ImpulseFly, ForceMode2D.Impulse);
			gameObject.GetComponent<Collider2D>().isTrigger = false;
		}
	}

	public bool Saber()
	{
		if (TimeAction[Iter] <= 0.1f)
			return(true);
		else
			return(false);	
	}

	public void Shot(Vector2 vector, string tag)
	{
		if (_Shot == true)
		{
            if (tag == "Player")
            {
              //  cumShot.GetComponent<playershot>().playerShot = true;
                //gameObject.transform.GetComponentInParent(typeof(Transform)).GetComponent<CharacterControllerAI>().SetPlayerShot(playerShot);
                //Debug.Log("lal " + gameObject.transform.parent.name);
                //characterControllerAI.SetPlayerShot(cumShot.GetComponent<playershot>().playerShot);
                //Debug.Log("p: " + playerShot);
                CurNumberShot--;
            }
			_Shot = false;
			gameObject.GetComponent<AudioSource>().Play();

			GameObject ShotElement = new GameObject();
			ShotElement.name = "Ammo";
			ShotElement.tag = tag;
			ShotElement.layer = LayerMask.NameToLayer("Ammo");
			ShotElement.transform.position = gameObject.transform.position;
			ShotElement.transform.rotation = gameObject.transform.rotation;
			ShotElement.transform.RotateAround(ShotElement.transform.position, Vector3.forward, -90);
			ShotElement.AddComponent<SpriteRenderer>().sprite = SpriteShot[Iter];
			ShotElement.GetComponent<SpriteRenderer>().sortingLayerName = "hero";
			ShotElement.GetComponent<SpriteRenderer>().sortingOrder = 2;
			ShotElement.AddComponent<CircleCollider2D>();
			ShotElement.AddComponent<Rigidbody2D>().gravityScale = 0.0f;
			ShotElement.GetComponent<Rigidbody2D>().AddForce(vector * ImpulseFly * 2, ForceMode2D.Impulse);
			ShotElement.AddComponent<WeaponAmmo>();
			Destroy(ShotElement, TimeAction[Iter]);
		}
	}
}
