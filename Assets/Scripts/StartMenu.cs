using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    private float angle;
    public GameObject Title;
    public GameObject Startgame;
    private Color color;
    private bool Loading;
    public Image loadring;
    public string SceneName;
    public Text LoadindText;

    void OnMouseDown()
    {


    }

   // public []
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public void OnPlayStart() {
        Application.LoadLevel("Loading");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
    
    
}
