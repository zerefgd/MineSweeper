using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Choice { ZERO,ONE,TWO,THREE,FOUR,FIVE,SIX,SEVEN,EIGHT,BOMB};

public struct GridPos { public int row, col; };

public class Board
{
    List<List<Choice>> choices;
    List<GridPos> randomGrid;
    List<GridPos> bombs;
    Dictionary<GridPos, bool> hasClicked;

    public Board()
    {
        choices = new List<List<Choice>>();
        randomGrid = new List<GridPos>();

        bombs = new List<GridPos>();
        hasClicked = new Dictionary<GridPos, bool>();

        for(int i = 0; i < 5;i++)
        {
            List<Choice> temp = new List<Choice>();
            for (int j = 0; j < 5; j++)
            {
                temp.Add(Choice.ZERO);
                randomGrid.Add(new GridPos { row = i, col = j });
                hasClicked[new GridPos { row = i, col = j }] = false;
            }
            choices.Add(temp);
        }

        for (int i = 0; i < 5; i++)
        {
            GridPos temp = GetRandomGrid();
            bombs.Add(temp);
            choices[temp.row][temp.col] = Choice.BOMB;
        }

        SetUpTheNumbers();
    }

    void SetUpTheNumbers()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                GetNumberOfBombs(i, j);
            }
        }
    }

    void GetNumberOfBombs(int row,int col)
    {
        if (choices[row][col] == Choice.BOMB) return;
        int bombs = GetBombsAt(row - 1, col) + GetBombsAt(row + 1, col)
                    + GetBombsAt(row, col - 1) + GetBombsAt(row, col + 1)
                    + GetBombsAt(row - 1, col - 1) + GetBombsAt(row + 1, col + 1) 
                    + GetBombsAt(row + 1, col - 1) + GetBombsAt(row - 1, col + 1);
        choices[row][col] = bombs == 1 ? Choice.ONE : bombs == 2 ? Choice.TWO
                           : bombs == 3 ? Choice.THREE : bombs == 4 ? Choice.FOUR
                           : bombs == 5 ? Choice.FIVE : bombs == 6 ? Choice.SIX
                           : bombs == 7 ? Choice.SEVEN : bombs == 8 ? Choice.EIGHT
                           : Choice.ZERO;
    }

    int GetBombsAt(int row,int col)
    {
        if(row < 5 && col < 5 && col > -1 && row > -1)
        {
            return choices[row][col] == Choice.BOMB ? 1 : 0;
        }
        return 0;
    }
    GridPos GetRandomGrid()
    {
        GridPos temp;
        int index = Random.Range(0, randomGrid.Count);
        temp = randomGrid[index];
        randomGrid.RemoveAt(index);
        return temp;
    }

    public Choice GetNumberAt(int row,int col)
    {
        return choices[row][col];
    }

    public int GetScore(int row,int col)
    {
        int score = 0;
        hasClicked[new GridPos { row = row, col = col }] = true;
        foreach(GridPos bomb in bombs)
        {
            score += GetScoreAtPosition(bomb);
        }
        return score;
    }

    public List<GridPos> GetBombs()
    {
        List<GridPos> result = new List<GridPos>();

        foreach(GridPos bomb in bombs)
        {
            if (GetScoreAtPosition(bomb) == 0) result.Add(bomb);
        }

        return result;
    }

    int GetScoreAtPosition(GridPos temp)
    {
        List<GridPos> posList = new List<GridPos> {
            new GridPos { row = -1, col = 0 },
            new GridPos { row = 1, col = 0 },
            new GridPos { row = 0, col = 1 },
            new GridPos { row = 0, col = -1 },
            new GridPos { row = -1, col = -1 },
            new GridPos { row = 1, col = 1 },
            new GridPos { row = -1, col = 1 },
            new GridPos { row = 1, col = -1 }
        };

        foreach(GridPos cell in posList)
        {
            int temprow = temp.row + cell.row;
            int tempcol = temp.col + cell.col;
            if(temprow >= 0 && tempcol >= 0 && temprow < 5 && tempcol < 5)
            {
                if(!hasClicked[new GridPos { row = temprow, col = tempcol }])
                {
                    if(choices[temprow][tempcol] != Choice.BOMB)
                    {
                        return 1;
                    }
                }
            }
        }
        return 0;
    }
}
