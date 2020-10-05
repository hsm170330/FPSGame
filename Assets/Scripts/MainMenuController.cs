using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] AudioClip _startingSong;
    [SerializeField] Text _highScoreTextView;

    // Start is called before hte first frame update
    void Start()
    {
        // load high score display
        int highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreTextView.text = highScore.ToString();
        if(_startingSong != null)
        {
            AudioManager.Instance.PlaySong(_startingSong);
        }
    }
    public void ResetData()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        Debug.Log("Reset High score");
        int highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreTextView.text = highScore.ToString();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
