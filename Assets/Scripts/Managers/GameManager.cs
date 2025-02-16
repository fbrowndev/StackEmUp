using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game UI")]
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TMP_Text _scoreText;

    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Game Over Methods
    public void GameOver()
    {
        Debug.Log("Game Over!");
        //_gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    public void AddScore(int points)
    {
        score += points;
        _scoreText.text = "Score: " + score;
    }
}
