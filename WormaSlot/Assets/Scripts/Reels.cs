using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;


public class Reels : MonoBehaviour
{
    
    // Class variables
    string[] itemsToDisplay = new string[9];
    private int Balance;
    private int _winnings = 0;                 
    private int _jackpot = 5000;               
    private int _turn = 0;                 
    private int BetAmount = 2;                
    private float _winNumber = 0.0f;            
    private float _lossNumber = 0.0f;
    private int scatterBalance;
    
    
    void Start()
    {
        GameObject.Find("MainTheme").GetComponent<AudioSource>().Play();
        PlayerPrefs.GetInt("Funds");     
        Balance = PlayerPrefs.GetInt("Funds") + PlayerPrefs.GetInt("BonusWinBalance");
        PlayerPrefs.SetInt("BonusWinBalance", + 0);
    }

   
    void OnGUI()
    {
        
        _PictureBoxes();
        
        PlayerPrefs.SetInt("Funds", +Balance);
        GameObject.Find("Balance").GetComponent<Text>().text = "€" + Balance;
        GameObject.Find("BetAmount").GetComponent<Text>().text = "€" + BetAmount;
        GameObject.Find("ScatterRounds").GetComponent<Text>().text = "Scatter rounds: " + scatterBalance / BetAmount;
    }

    public void AddFunds(int Balance)
    {
        this.Balance = Balance + PlayerPrefs.GetInt("Funds");
    }
   
    void _PictureBoxes()
    {
        // First box
        GUI.Box(new Rect(Screen.width - 1410, Screen.height - 600, 125, 105), Resources.Load((itemsToDisplay[0] == null) ? "K" : itemsToDisplay[0]) as Texture2D);

        // Second box
        GUI.Box(new Rect(Screen.width - 1260, Screen.height - 600, 125, 105), Resources.Load((itemsToDisplay[1] == null) ? "A" : itemsToDisplay[1]) as Texture2D);

        // Third box ( Right most - first row)
        GUI.Box(new Rect(Screen.width - 1110, Screen.height - 600, 125, 105), Resources.Load((itemsToDisplay[2] == null) ? "J" : itemsToDisplay[2]) as Texture2D);

        // Fourth box
        GUI.Box(new Rect(Screen.width - 1410, Screen.height - 490, 125, 105), Resources.Load((itemsToDisplay[3] == null) ? "Bonus" : itemsToDisplay[3]) as Texture2D);

        // fifth box
        GUI.Box(new Rect(Screen.width - 1260, Screen.height - 490, 125, 105), Resources.Load((itemsToDisplay[4] == null) ? "Scatter" : itemsToDisplay[4]) as Texture2D);

        // sixth box (right most - second row)
        GUI.Box(new Rect(Screen.width - 1110, Screen.height - 490, 125, 105), Resources.Load((itemsToDisplay[5] == null) ? "Wild" : itemsToDisplay[5]) as Texture2D);

        // seventh box
        GUI.Box(new Rect(Screen.width - 1410, Screen.height - 380, 125, 105), Resources.Load((itemsToDisplay[6] == null) ? "Worm" : itemsToDisplay[6]) as Texture2D);

        // eighth box
        GUI.Box(new Rect(Screen.width - 1260, Screen.height - 380, 125, 105), Resources.Load((itemsToDisplay[7] == null) ? "K" : itemsToDisplay[7]) as Texture2D);

        // ninth box (right most - third row)
        GUI.Box(new Rect(Screen.width - 1110, Screen.height - 380, 125, 105), Resources.Load((itemsToDisplay[8] == null) ? "J" : itemsToDisplay[8]) as Texture2D);
    }

    // Player Stats
    private void ShowPlayerStats()
    {
        float _winRatio = 0.0f;             // Ratio of winning
        float _lossRatio = 0.0f;            // Ratio of losing

        
        _winRatio = _winNumber / _turn;
        _lossRatio = _lossNumber / _turn;
        string stats = "";
        
        stats += ("Player Money: " + Balance + "\n");
        stats += ("Turn: " + _turn + "\n");
        stats += ("Wins: " + _winNumber + "\n");
        stats += ("Losses: " + _lossNumber + "\n");
        stats += ("Win Ratio: " + (_winRatio * 100) + "%\n");
        stats += ("Loss Ratio: " + (_lossRatio * 100) + "%\n");

        // Stats message
        EditorUtility.DisplayDialog("Player Stats", "Statistics from last play \n" + stats, "OK");
    }


    // WinMessage
    private void ShowWinMessage()
    {
        if (scatterBalance != 0)
        {
            scatterBalance -= BetAmount;
        }
        else
        {
            // Reduce bet from balance
            Balance -= BetAmount;
        }
        
        // Add the win amount to balance
        Balance += _winnings;
        // Play the audio when player wins
        GameObject.Find("WinAudio").GetComponent<AudioSource>().Play();
        

        /* compare two random values to see if player wins Jackpot */
        var jackPotTry = Random.Range(1, 100);
        var jackPotWin = Random.Range(1, 100);
        if (jackPotTry == jackPotWin)
        {
            // Display the jackpot winning message and add to balance
            EditorUtility.DisplayDialog("Jackpot!", "You Won the €" + _jackpot + " Jackpot!!", "OK");
            Balance += _jackpot;
            // Display the win amount + jackpot
            GameObject.Find("WinAmount").GetComponent<Text>().text = "" + _winnings + " + " + _jackpot;
            GameObject.Find("SpinResult").GetComponent<Text>().text = "You Won: €" + _winnings + " + JACKPOT:  " +_jackpot;
        }
        else
        {
            // Display the win amount
            GameObject.Find("WinAmount").GetComponent<Text>().text = "€" + _winnings;
            GameObject.Find("SpinResult").GetComponent<Text>().text = "You Won: €" + _winnings;
            
        }

       
    }

    // loss message
    private void ShowLossMessage()
    {

        if (scatterBalance != 0)
        {
            scatterBalance -= BetAmount;
        }
        else
        {
            // Reduce bet from balance
            Balance -= BetAmount;
        }
        
        // Display the loss message
        GameObject.Find("SpinResult").GetComponent<Text>().text = "You Lost!";
        // Clear the win amount
        GameObject.Find("WinAmount").GetComponent<Text>().text = "";

        
    }

    
    private string[] Spins()
    {
        _turn++;

        
        // array to store the spin results
        string[] spinResult = { " ", " ", " ", " ", " ", " ", " ", " ", " " };

        // Spin 9 times
        for (var spin = 0; spin < 9; spin++)
        {
            int randomNumber = Random.Range(1, 86);

            if (randomNumber <= 20 )
            {  
                spinResult[spin] = "Bonus";//J
            }
            else if (randomNumber >= 21 && randomNumber <= 36)
            { 
                spinResult[spin] = "K";
            }
            else if (randomNumber >= 37 && randomNumber <= 51)
            { 
                spinResult[spin] = "A";
            }
            else if (randomNumber >= 52 && randomNumber <= 62)
            { 
                spinResult[spin] = "Worm";
            }
            else if (randomNumber >= 63 && randomNumber <= 71)
            { 
                spinResult[spin] = "Wild";
            }
            else if (randomNumber >= 72 && randomNumber <= 79)
            { 
                spinResult[spin] = "Scatter";
            }
            else if (randomNumber >= 80 && randomNumber <= 86)
            { 
                spinResult[spin] = "Bonus";
            }
            
        }
        // Set the spin results to ItemsToDisplay, which will automatically update the view
        itemsToDisplay = spinResult;

        
        
           // return the spin Results
           return spinResult;
            
        
    }

    
    public void _determineResult(string[] spinResult)
    {
        
        Dictionary<string, int> SpinResults = new Dictionary<string, int>();

        
        // spinResult to Dictionary
        foreach (string item in spinResult)
        {
            // If the current item has already been added to the dictionary
            if (SpinResults.ContainsKey(item))
            {
                // Increment added item
                SpinResults[item]++;
            }
            
            else
            {
                // add item
                SpinResults.Add(item, 1);
            }
        }
        
      
        // factor to multiply BetAmount
        int factor = 0;
        

        // if SpinResults has an item coming 5 times
        if (SpinResults.ContainsValue(5))
            {
           
            // Get the item (key) with value of 5
            string item = SpinResults.FirstOrDefault(x => x.Value == 5).Key;
                switch (item)
                {
                    case "J": factor = 2; break;
                    case "K": factor = 3; break;
                    case "A": factor = 4; break;
                    case "Wild": factor = 50; break;
                    case "Scatter": factor = 48; break;
                    case "Bonus": factor = 51; break;
                    case "Worm": factor = 40; break;
                }
           
            }


            else if (SpinResults.ContainsValue(6))
            {
                // Get the item (key) with value of 6
                string item = SpinResults.FirstOrDefault(x => x.Value == 6).Key;
                switch (item)
                {
                    case "J": factor = 4; break;
                    case "K": factor = 6; break;
                    case "A": factor = 8; break;
                    case "Wild": factor = 100; break;
                    case "Scatter": factor = 48; break;
                    case "Bonus": factor = 51; break;
                    case "Worm": factor = 80; break;
                }
            }

            else if (SpinResults.ContainsValue(7))
            {
                // Get the item (key) with value of 7
                string item = SpinResults.FirstOrDefault(x => x.Value == 7).Key;
                switch (item)
                {
                    case "J": factor = 8; break;
                    case "K": factor = 12; break;
                    case "A": factor = 16; break;
                    case "Wild": factor = 150; break;
                    case "Scatter": factor = 48; break;
                    case "Bonus": factor = 51; break;
                    case "Worm": factor = 120; break;
                }
            }

            else if (SpinResults.ContainsValue(8))
            {
                // Get the item (key) with value of 8
                string item = SpinResults.FirstOrDefault(x => x.Value == 8).Key;
                switch (item)
                {
                    case "J": factor = 16; break;
                    case "K": factor = 24; break;
                    case "A": factor = 32; break;
                    case "Wild": factor = 200; break;
                    case "Scatter": factor = 48; break;
                    case "Bonus": factor = 51; break;
                    case "Worm": factor = 160; break;
                }
                
            }
        if (factor != 0 && factor != 51 && factor != 48) {
            // Update wins
            _winNumber++;          
            // set the winning amount
            _winnings = BetAmount * factor;
            // Display win message
            ShowWinMessage();
        }
        else if (factor == 0)
        {

            // update losses
            _lossNumber++;
            // display the loss message
            ShowLossMessage();
            
        }

        else if (factor == 51)
        {
            _winNumber++;
            _winnings = BetAmount * factor;
            ShowWinMessage();
            EditorUtility.DisplayDialog("Congratulations!", "You Won €" + _winnings + " and got into the BonusGame! ", "Proceed!");          
            SceneManager.LoadScene("BonusGame");
        }

        else if (factor == 48)
        {
            _winNumber++;
            _winnings = BetAmount * factor;
            ShowWinMessage();
            EditorUtility.DisplayDialog("Congratulations!", "You won €" +_winnings +  " and got 5 free rounds! ", "Proceed!");
            scatterBalance = scatterBalance += (BetAmount * 5);
            
        }
       
    }

   
    public void OnSpinButtonClick()
    {
        
        // if balance 0
        if (Balance == 0)
        {
            // OutOfMoney MSG
            if (EditorUtility.DisplayDialog("Out of Money", "You ran out of Money! Add funds to continue playing.", "OK"))
            {                         
                ShowPlayerStats();
                
            }

        }
        else if (BetAmount > Balance)
        {
            // If the bet is bigger than balance
            EditorUtility.DisplayDialog("Not Enough Money", "You don't have enough Money to place that bet.", "OK");
        }
        else if (BetAmount <= Balance)
        {
            
            string[] _spinResult = Spins();        
            
            _determineResult(_spinResult);
            
        }
    }

    
    public void Bet(int betAmount)
    {
        if (scatterBalance != 0)
        {
            EditorUtility.DisplayDialog("Whoopsie!", "You can't change your bet during Scatter-rounds!", "OK");
        }

        else
        {
            PlayerPrefs.GetInt("BonusBET");
            // set the BET
            BetAmount = betAmount;
            PlayerPrefs.SetInt("BonusBET", +betAmount);
        }
    }

    // Quit button
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
        ShowPlayerStats();

        // To reset balance (For testing)
         Balance = 0;
    }
    
    public void BonusGameButtonClick() // Testing
    {
        SceneManager.LoadScene("BonusGame");
        
    }
   

    public void WinTableButtonClick()
    {
        SceneManager.LoadScene("WinTable");
        
    }

    
}
