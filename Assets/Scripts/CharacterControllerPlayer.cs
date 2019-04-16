using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControllerPlayer : MonoBehaviour
{
	public CharacterMove Character;
	public Text TextNumberShot;
	public bool winflag;
    public bool followFlag;
    public Vector2 trigPos;


	private void Awake()
	{
		Character = gameObject.GetComponent<CharacterMove>();
	}

	//private void Start()
	//{
 //       playerPos = GameObject.FindWithTag("Player").GetComponent<CharacterControllerPlayer>().playerPos;
	//}

	private void Update()
	{
        
		_MoveCharacter();
		_DirectCharacter();
		_ActionElement();
		_MouseKey();
		_UpdateNumberShot();
	}


	private void _UpdateNumberShot()
	{
		if (TextNumberShot != null)
		{
			uint number = Character.GetComponent<CharacterMove>().GetNumberShot();
			if (number > 100)
				TextNumberShot.text = "none";
			else
				TextNumberShot.text = number.ToString();
		}
	}

	private void _MoveCharacter()
	{
		Vector2 vectorMove = Vector2.zero;
		if (Input.GetKey("w"))
			vectorMove += Vector2.up;
		if (Input.GetKey("s"))
			vectorMove -= Vector2.up;
		if (Input.GetKey("a"))
			vectorMove += Vector2.left;
		else if (Input.GetKey("d"))
			vectorMove += Vector2.right;
		Character.Move(vectorMove);
	}

	private void _DirectCharacter()
	{
		Character.Direct(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	public void _ActionElement()
	{
		if (Input.GetKey("e"))
		{
			Collider2D[] UpElements = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 0.1f);
			foreach (Collider2D element in UpElements)
				Character.UpElement(element.gameObject);
		}
	}

	public void _MouseKey()
	{
		if (Input.GetButton("Fire1"))
			Character.ToShot("Player");
		if (Input.GetButton("Fire2"))
			Character.DownElement();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag.Equals("fin"))
			winflag = true;
     
        
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.transform.tag.Equals("follow"))
        {
            followFlag = true;
            trigPos = new Vector2(collision.transform.position.x, collision.transform.position.y);
        }
	}


}
