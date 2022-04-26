using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private float score = 0.0f;
    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 10;
    private int scoreToNextLevel = 10;
    private bool isDead = false;
    public Text scoreText;
    public Text levelText;

    public DeathMenu deathMenu;
    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;
            
        if (score >= scoreToNextLevel)
            LevelUp();
            
        score += Time.deltaTime * difficultyLevel;
        scoreText.text = ((int)score).ToString();
        levelText.text = "Kecepatan : " + ((int)difficultyLevel).ToString() + "x";
    }
    void LevelUp()
    {
        if(difficultyLevel == maxDifficultyLevel)
            return;

        scoreToNextLevel *= 2;
        difficultyLevel++;

        GetComponent<PlayerMotorSwipe>().SetSpeed(difficultyLevel);

        Debug.Log(difficultyLevel);
    }

    public void OnDeath()
    {
        isDead = true;

        if(PlayerPrefs.GetFloat("Highscore") <score)
            PlayerPrefs.SetFloat("Highscore", score);

        deathMenu.ToggleEndMenu(score);
    }
}
