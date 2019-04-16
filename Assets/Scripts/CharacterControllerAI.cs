using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerAI : MonoBehaviour
{
	public float SearchForwardDistance = 10.0f;
	public float SearchForwardAngle = 135;
	public float SearchBackDistance = 0.5f;
	public bool Attention = false;
	public bool Patrulate = true;
	public float PatrulTime = 2;
	public float TimeAttention = 5f;
	public float PatrulGrad = 90f;
	public AudioClip[] SoundAttention = new AudioClip[2];
	private CharacterMove Character;
    private static Vector3 _PlayerPosition;
	private static Vector3 _playerPos;
	private static bool _PlayerDead;
	private float _Timer = 0;
	private GameObject[] Doors;
    public bool shot;

    public bool _active = true;

    private Vector2 vectorMove;

    private bool _playerShot;
    public GameObject cumShot;


    [SerializeField] private CharacterControllerPlayer characterControllerPlayer;

    public bool _shotActive = true;

	private void Awake()
	{
		Character = gameObject.GetComponent<CharacterMove>();
	}

    private void Start()
    {
        cumShot = GameObject.Find("playerShot");
        _playerPos = characterControllerPlayer.gameObject.transform.position;
    }

	private void Update()
	{
      //  _playerPos = characterControllerPlayer.gameObject.transform.position;
		_SearchPlayer();
		_RelaxAtention();
		_ToMove();
        _ToPatrulate();
	}

	private void _SearchPlayer()
	{

      //  _playerShot = cumShot.GetComponent<playershot>().playerShot;
        Vector3 direction = (_playerPos - gameObject.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + direction / 2, direction);
        Debug.DrawRay(transform.position + direction / 2, direction);

        if (hit.transform.tag == "Player" || _playerShot)
        {

            Collider2D[] SearchElements = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), SearchForwardDistance);
            foreach (Collider2D element in SearchElements)
            {
                Debug.Log("element: " + element.tag);
                if (_playerShot && element.tag == "follow" && _active)
                {
                  //  Debug.Log("vectorMoveShot: " + vectorMove);
                    vectorMove = new Vector2(element.transform.position.x - transform.position.x, element.transform.position.y - transform.position.y);
                    shot = true;
                }
                
                if (element.gameObject.tag == "Player")
                {
                    if (element.gameObject.GetComponent<CharacterMove>())
                        _PlayerDead = element.gameObject.GetComponent<CharacterMove>().GetDead();

                    _PlayerPosition = element.gameObject.transform.position;
                    //Vector3 direction = (_PlayerPosition - gameObject.transform.position).normalized;
                    float angle = Vector3.Angle(-gameObject.transform.up, direction);
                    float distance = Vector3.Distance(gameObject.transform.position, _PlayerPosition);

                    if (distance < SearchBackDistance)
                    {
                        _ToAttention();
                        _ShotToCharacter();
                    }
                    if (Physics2D.Raycast(transform.position, direction, distance) && angle < SearchForwardAngle / 2f)
                    {
                        _ToAttention();
                        _ShotToCharacter();
                    }
                }
            }
        }
	}

    public void SetPlayerShot(bool status)
    {
        _playerShot = status;
    }

	private	void _RelaxAtention()
	{
		if (Attention == false)
		{

		}
		else if (Attention == true)
		{
			if (_Timer >= TimeAttention)
			{
				Attention = false;
				_Timer = 0;
			}
			Debug.Log("Attention");
		}
	}

	private void _ToAttention()
	{
		if (Attention == false)
		{
			Patrulate = false;
			Attention = true;
			gameObject.GetComponent<AudioSource>().clip = SoundAttention[Random.Range(0, SoundAttention.Length)];
			gameObject.GetComponent<AudioSource>().Play();
		}
	}

	private void _ToMove()
    {
        //if (Attention && characterControllerPlayer.followFlag && transform.position.x.Equals(characterControllerPlayer.trigPos.x) && transform.position.y.Equals(characterControllerPlayer.trigPos.y))
            //characterControllerPlayer.followFlag = false;
        if (Attention == true)
        {
            if (shot)
            {
                vectorMove.Normalize();
                Character.Move(vectorMove);
            }
            else if (characterControllerPlayer.followFlag)
            {
                
                vectorMove = new Vector2 (characterControllerPlayer.trigPos.x - transform.position.x, characterControllerPlayer.trigPos.y - transform.position.y);
              //  Debug.Log("vectorMoveFollow: " + vectorMove);
                vectorMove.Normalize();
                Character.Move(vectorMove);
            }
            else
            {
              //  Debug.Log("vectorMoveAttention: " + vectorMove);
                vectorMove = new Vector2(_PlayerPosition.x - gameObject.transform.position.x, _PlayerPosition.y - gameObject.transform.position.y);
                vectorMove.Normalize();
                Character.Move(vectorMove);
            }
		}

	}

	private void _ShotToCharacter()
	{
		if (Attention == true && _PlayerDead == false)
		{
			Character.Direct(_PlayerPosition);
			Character.ToShot("Enemy");
		}
	}

	private void _ToPatrulate()
	{
		if (Patrulate == true)
		{
			Vector2 vectorMove = Vector2.zero;
			_Timer += Time.deltaTime;
		
			if (_Timer > PatrulTime && _Timer < PatrulTime * 2)
			{
				Character.Rotate(+PatrulGrad);
				vectorMove += Vector2.up;
			}
			else if (_Timer > PatrulTime * 3 && _Timer < PatrulTime * 4)
			{
				Character.Rotate(-PatrulGrad);
				vectorMove += Vector2.down;
			}
			else if (_Timer > PatrulTime * 5)
			{
				_Timer = 0;
			}
			Character.Move(vectorMove);
		}

    }

	private void OnTriggerStay2D(Collider2D collision)
	{
        if (collision.tag == "follow")
        {
            Debug.Log("??????????????????????");
            _active = false;
            characterControllerPlayer.followFlag = false;
            shot = false;
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.tag == "follow")
        {
            //_active = true;
        }
	}
}
