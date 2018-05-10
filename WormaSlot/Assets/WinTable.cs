using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTable : MonoBehaviour {




    
    void Start()
    {
        GameObject.Find("Theme").GetComponent<AudioSource>().Play();
    }
        

    public void ReturnToMainScreen()
    {
        SceneManager.LoadScene("Character");
    }
}
