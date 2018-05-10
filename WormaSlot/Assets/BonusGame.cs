using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class BonusGame : MonoBehaviour {
    
    
    
    
    void Start ()
    {
               
        GameObject.Find("Balance").GetComponent<Text>().text = "€" + PlayerPrefs.GetInt("Funds");
        GameObject.Find("BetAmount").GetComponent<Text>().text = "€" + PlayerPrefs.GetInt("BonusBET");
        
    }


    private void OnGUI()
    {
        GameObject.Find("bonusWinBalance").GetComponent<Text>().text = "€" + PlayerPrefs.GetInt("BonusWinBalance");

    }

    
    // this button is for testing
public void BackToMainGameButtonClick()
    {     
        SceneManager.LoadScene("Character");  
    }

    





}



