using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUtility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int[,] removeFromGrid(int[,] gameBoard, int x, int y)
    {
        for (int i = y; i > 0; i--)
        {
            gameBoard[x, i] = gameBoard[x, i - 1];
        }
        gameBoard[x, 0] = 0;
        return gameBoard;
    }

    public static int[,] copyGameGrid(int[,] gameBoard)
    {
        int[,] newGrid = new int[7, 6];
        for (int i = 0; i < newGrid.Length / newGrid.GetUpperBound(0); i++)
        {
            for (int j = 0; j < newGrid.GetUpperBound(0); j++)
            {
                newGrid[i, j] = gameBoard[i, j];
            }
        }
        return newGrid;
    }

    public static bool canInsert(int[,] gameBoard, int index)
    {
        return gameBoard[index, 0] == 0;
    }

    public static void displayGrid(int[,] gameBoard)
    {
        for (int i = 0; i < gameBoard.Length / gameBoard.GetUpperBound(0); i++)
        {
            string output = "";
            for (int j = 0; j < gameBoard.GetUpperBound(0); j++)
            {
                output = output + "[" + gameBoard[i, j] + "]";
            }
            Debug.Log(output);
        }
    }

    public static int[,] insertToGrid(int[,] gameBoard, int index, int chesskey)
    {
        bool inserted = false;
        for (int i = 1; i < gameBoard.GetUpperBound(0); i++)
        {
            if (gameBoard[index, i] != 0)
            {
                gameBoard[index, i - 1] = chesskey;
                inserted = true;
                break;
            }
        }
        if (!inserted) gameBoard[index, 5] = chesskey;
        return gameBoard;
    }
}
