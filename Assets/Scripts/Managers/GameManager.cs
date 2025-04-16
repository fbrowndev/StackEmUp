using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game UI")]
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TMP_Text _scoreText;

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
            

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Score Methods
    void UpdateScoreText()
    {
        if(_scoreText != null)
        {
            _scoreText.text = "Score: " + score.ToString();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public int GetScore() { return score; }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    #endregion


    #region Game Over Methods
    public void GameOver()
    {
        Debug.Log("Game Over!");
        //_gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    
}
