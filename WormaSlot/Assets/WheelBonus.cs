using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;


public class WheelBonus : MonoBehaviour
{
    public List<int> prize;
    public List<AnimationCurve> animationCurves;

    private bool spinning;
    private float anglePerItem;
    private int randomTime;
    public int itemNumber;

    private int bonusWin;
    public int bonusWinBalance;
    
    
    
    void Start()
    {
        spinning = false;
        anglePerItem = 360 / prize.Count;
        PlayerPrefs.SetInt("BonusWinBalance", +bonusWinBalance);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !spinning)
        {

            randomTime = Random.Range(1, 4);
            itemNumber = Random.Range(0, prize.Count);
            float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);

            StartCoroutine(SpinTheWheel(5 * randomTime, maxAngle));
        }
    }

    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
        spinning = true;

        float timer = 0.0f;
        float startAngle = transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle;

        int animationCurveNumber = Random.Range(0, animationCurves.Count);
        Debug.Log("Animation Curve No. : " + animationCurveNumber);

        while (timer < time)
        {
            //to calculate rotation
            float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
        }

        transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        spinning = false;

        Debug.Log("Prize: " + prize[itemNumber]); //Testing purposes

        int factor = prize[itemNumber];
        
        if (prize[itemNumber] == 3)
        {
            Debug.Log("YOU GOT END! BONUSGAME HAS ENDED!");
            EditorUtility.DisplayDialog("Congratulations!", "You Won €" + PlayerPrefs.GetInt("BonusWinBalance") + " in the BonusGame! ", "Proceed!");
            SceneManager.LoadScene("Character");
        }
        else 
        {
            bonusWin = PlayerPrefs.GetInt("BonusBET") * factor;
            bonusWinBalance += bonusWin;
            
        }
        PlayerPrefs.SetInt("BonusWin", bonusWin);
        PlayerPrefs.SetInt("BonusWinBalance", +bonusWinBalance);
    }
    
    
}
