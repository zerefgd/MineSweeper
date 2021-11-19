using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int seconds, minutes, score;

    [SerializeField]
    Text secondsText, minutesText, scoreText;

    [SerializeField]
    Image smiley;

    [SerializeField]
    Sprite win, lose;

    public static GameManager instance;

    public Board myBoard;

    bool hasGameFinished;

    public List<GameObject> bombs;
    public Dictionary<GridPos, GameObject> allCells;

    private void Update()
    {
        if (hasGameFinished) return;
        if(Input.GetMouseButtonDown(0))
        {
            //Raycast
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (!hit.collider) return;

            if(hit.collider.CompareTag("Cell"))
            {
                Cell cell = hit.collider.gameObject.GetComponent<Cell>();
                cell.UpdateTurn();

                if(cell.mychoice == Choice.BOMB)
                {
                    hasGameFinished = true;
                    LoseGame();
                    return;
                }

                int prevScore = score;
                score = myBoard.GetScore(cell.row, cell.col);
                if (prevScore > score)
                {
                    foreach (GridPos bomb in myBoard.GetBombs())
                    {
                        allCells[bomb].GetComponent<Cell>().UpdateFlag();
                    }
                }
                scoreText.text = score.ToString();

                if(score == 0)
                {
                    hasGameFinished = true;
                    WinGame();
                    return;
                }
            }
        }
    }

    void WinGame()
    {
        StopAllCoroutines();
        foreach(GameObject bomb in bombs)
        {
            bomb.GetComponent<Cell>().DefuseBomb();
        }
        smiley.sprite = win;
    }

    void LoseGame()
    {
        StopAllCoroutines();
        foreach(GameObject bomb in bombs)
        {
            bomb.GetComponent<Cell>().UpdateBomb();
        }
        foreach(KeyValuePair<GridPos,GameObject> pair in allCells)
        {
            pair.Value.GetComponent<Cell>().UpdateTurn();
        }
        smiley.sprite = lose;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        hasGameFinished = false;
        score = 5;
        minutes = 0;
        seconds = 0;
        secondsText.text = "00";
        minutesText.text = "00";
        scoreText.text = "5";
        myBoard = new Board();
        allCells = new Dictionary<GridPos, GameObject>();
        bombs = new List<GameObject>();
        StartCoroutine(RunTimer());
    }

    IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(1f);
        seconds++;
        if(seconds >= 60)
        {
            seconds = 0;
            minutes++;
            minutesText.text = minutes > 9 ? minutes.ToString() : "0" + minutes;
        }
        secondsText.text = seconds> 9 ? seconds.ToString() : "0" + seconds;
        StartCoroutine(RunTimer());
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
