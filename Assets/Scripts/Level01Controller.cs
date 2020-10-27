using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [SerializeField] Text _currentScoreTextView;
    [SerializeField] GameObject PauseMenu;

    int _currentScore;
    bool paused = false;
    private void Update()
    {
        // Increase Score
        //TODO replace with real implementation later
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //IncreaseScore(5);
        }
        // Exit Level
        //TODO bring up popup menu for navigation
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Pause();
            }
            else if (paused)
            {
                Resume();
                Cursor.visible = false;
            }
            
        }
    }
    public void ExitLevel()
    {
        // compare score to high score
        int highScore = PlayerPrefs.GetInt("HighScore");
        if (_currentScore > highScore)
        {
            // save current score as new high score
            PlayerPrefs.SetInt("HighScore", _currentScore);
            Debug.Log("New high score: " + _currentScore);
        }
        Resume();
        // load new level
        SceneManager.LoadScene("MainMenu");
    }
    public void Retry()
    {
        // compare score to high score
        int highScore = PlayerPrefs.GetInt("HighScore");
        if (_currentScore > highScore)
        {
            // save current score as new high score
            PlayerPrefs.SetInt("HighScore", _currentScore);
            Debug.Log("New high score: " + _currentScore);
        }
        // reload scene
        SceneManager.LoadScene("Level01");
    }
    public void IncreaseScore(int scoreIncrease)
    {
        // increase score
        _currentScore += scoreIncrease;
        // update score display, so we can see the new score
        _currentScoreTextView.text =
            "Score: " + _currentScore.ToString();
    }
    public void Resume()
    {
        //disable pause menu
        PauseMenu.SetActive(false);
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Pause()
    {
        //enable pause menu
        PauseMenu.SetActive(true);
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
