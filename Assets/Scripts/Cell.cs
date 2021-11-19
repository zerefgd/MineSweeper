using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool hasClicked;

    public Choice mychoice;

    public int row, col;

    [SerializeField]
    Sprite unrevealed, bombed, flagged, defuse;

    [SerializeField]
    List<Sprite> sprites;

    SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        hasClicked = false;
        renderer = GetComponent<SpriteRenderer>();
        mychoice = GameManager.instance.myBoard.GetNumberAt(row, col);
        renderer.sprite = unrevealed;
        if (mychoice == Choice.BOMB) GameManager.instance.bombs.Add(gameObject);
        GameManager.instance.allCells[new GridPos { row = row, col = col }] = gameObject;
        //UpdateTurn();
    }

    public void UpdateTurn()
    {
        Sprite current = mychoice == Choice.ZERO ? sprites[0] : mychoice == Choice.ONE? sprites[1] :
                         mychoice == Choice.TWO? sprites[2] : mychoice == Choice.THREE ? sprites[3]:
                         mychoice == Choice.FOUR ? sprites[4] : mychoice == Choice.FIVE ? sprites[5]:
                         mychoice == Choice.SIX? sprites[6] : mychoice == Choice.SEVEN ? sprites[7]:
                         mychoice == Choice.EIGHT ? sprites[8] : bombed;
        renderer.sprite = current;
        hasClicked = true;
    }

    public void UpdateBomb()
    {
        renderer.sprite = bombed;
        hasClicked = true;
    }

    public void DefuseBomb()
    {
        renderer.sprite = defuse;
        hasClicked = true;
    }

    public void UpdateFlag()
    {
        renderer.sprite = flagged;
        hasClicked = true;
    }
}
