using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    public Text shots;
    public Canvas canvaswin;
    public Canvas canvasloose;
    public Canvas canvasshots;
    public AudioClip[] music;
    bool m_Play;
    private Color color;
    AudioSource m_MyAudioSource;
    bool m_ToggleChange = true;
    public Transform target;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public GameObject Hero;
    private bool isplaying = false;
    Scene m_Scene;
    // Start is called before the first frame update

    
    void Start ()
    {
        GetComponent<AudioSource>().
        GetComponent<AudioSource>().clip = music[Random.Range(0, music.Length)];
        m_MyAudioSource =  GetComponent<AudioSource>();
        m_Play = true;
        target = Hero.GetComponent<CharacterControllerPlayer>().Character.transform;
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(target.position.x, target.position.y+0.75f,target.position.z));
        Vector3 delta = new Vector3(target.position.x, target.position.y+0.75f,target.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;


        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
       color.a = 0;
     //  GameObject.Find("Retry").GetComponent<Image>().color = color;
        canvasloose.GetComponent<CanvasGroup>().alpha = 0;
        canvasloose.GetComponent<CanvasGroup>().interactable = false;
        canvasloose.GetComponent<CanvasGroup>().blocksRaycasts = false;
        canvaswin.GetComponent<CanvasGroup>().alpha = 0;
        canvaswin.GetComponent<CanvasGroup>().interactable = false;
        canvaswin.GetComponent<CanvasGroup>().blocksRaycasts = false;
        canvasshots.GetComponent<CanvasGroup>().alpha = 1;

    }
      
    IEnumerator WinSound() 
    {
        m_MyAudioSource.clip = canvaswin.GetComponent<AudioSource>().clip;
        GetComponent<AudioSource>().PlayOneShot(m_MyAudioSource.clip);
        yield return  new WaitForSeconds (10);
    }
        
    IEnumerator LooseSound() 
    {
        m_MyAudioSource.clip = canvasloose.GetComponent<AudioSource>().clip;
        GetComponent<AudioSource>().PlayOneShot(m_MyAudioSource.clip);
        yield return  new WaitForSeconds (10);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (m_Play == true && m_ToggleChange == true)
        {
      
            m_MyAudioSource.Play();
            //Ensure audio doesn’t play more than once
            m_ToggleChange = false;
        }

        if (m_Play == false && m_ToggleChange == true)
        {

            m_ToggleChange = false;
        }
       
     if (Hero.GetComponent<CharacterMove>()._Dead)
        {

            if (isplaying == false)
            {
                isplaying = true;
                StartCoroutine(LooseSound());
            }

            canvasloose.GetComponent<CanvasGroup>().alpha = 1;
            canvasloose.GetComponent<CanvasGroup>().interactable = true;
            canvasloose.GetComponent<CanvasGroup>().blocksRaycasts = true;
            canvaswin.GetComponent<CanvasGroup>().interactable = false;
            canvaswin.GetComponent<CanvasGroup>().blocksRaycasts = false;
            canvasshots.GetComponent<CanvasGroup>().alpha = 0;

            
        }

        else if (Hero.GetComponent<CharacterControllerPlayer>().winflag == true)
        {
            m_Scene = SceneManager.GetActiveScene();
            if (m_Scene.name == "level0")
            {
                if (isplaying == false)
                {
                    isplaying = true;
                    StartCoroutine(WinSound());
                }

                m_Play = false;
                m_ToggleChange = true;
                SceneManager.LoadScene ("level1");
            }
            else
            {
                if (isplaying == false)
                {
                    isplaying = true;
                    StartCoroutine(WinSound());
                }

                m_Play = false;
                m_ToggleChange = true;
                canvaswin.GetComponent<CanvasGroup>().alpha = 1;
                canvaswin.GetComponent<CanvasGroup>().interactable = true;
                canvaswin.GetComponent<CanvasGroup>().blocksRaycasts = true;
                canvasloose.GetComponent<CanvasGroup>().interactable = false;
                canvasloose.GetComponent<CanvasGroup>().blocksRaycasts = false;
                canvasshots.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
     else
     {
         
     }
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(target.position.x, target.position.y+0.75f,target.position.z));
        Vector3 delta = new Vector3(target.position.x, target.position.y+0.75f,target.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;


        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

    }
}
