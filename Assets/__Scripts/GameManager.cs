using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;

    public TextMeshProUGUI lblJumps;
    public TextMeshProUGUI lblScore;
    public TextMeshProUGUI lblGameOverScore;
    public Image imgJumps;
    public Image imgScore;
    public Image imgGameOverBackGround;
    public Image imgGameOverScore;
    public TextMeshProUGUI txtGameOver;
    
    
    private int score = 0;
    
    
    private PlayerShip playerShip;
    private AsteroidsScriptableObject asteroidSO;

    static public GameManager getInstance
    {
        get
        {
            return instance;
        }
        private set
        {
            if (instance != null)
            {
                Debug.LogWarning("GameManager instance already exists!");
            }
            instance = value;
        }
    }

    void Awake()
    {
        getInstance = this;
        
        score = 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ChangeHUD(false);
        playerShip = PlayerShip.S;
        asteroidSO =AsteroidsScriptableObject.S;
    }

    // Update is called once per frame
    void Update()
    {
        lblJumps.text = playerShip.GetJumpsLeftToString() + " jumps";
        lblScore.text = score.ToString();
    }

    public void GameOver()
    {
        ChangeHUD(true);
        
        Invoke("ResetGame", 7f);
    }

    private void ChangeHUD(bool isGameOver)
    {
        imgJumps.gameObject.SetActive(!isGameOver);
        imgScore.gameObject.SetActive(!isGameOver);
        lblJumps.gameObject.SetActive(!isGameOver);
        lblScore.gameObject.SetActive(!isGameOver);
        
        lblGameOverScore.text = "Score: " + score ;
        imgGameOverScore.gameObject.SetActive(isGameOver);
        imgGameOverBackGround.gameObject.SetActive(isGameOver);
        lblGameOverScore.gameObject.SetActive(isGameOver);
        txtGameOver.gameObject.SetActive(isGameOver);
    }

    public void IncrementScore(int sizeIndex)
    {
        score += asteroidSO.pointsForAsteroidSize[sizeIndex];
        Debug.Log("Score updated: " + score + "( +" + asteroidSO.pointsForAsteroidSize[sizeIndex] + ")");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
