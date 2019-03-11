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

    public static bool isOutOfBound(int x, int y)
    {
        if (x < 0) return true;
        if (y < 0) return true;
        if (x >= 7) return true;
        if (y >= 6) return true;
        return false;
    }

    public static int indexOf(int x, int y)
    {
        return 7 * y + x;
    }

    public static int numOfRemove(int[] gameBoard, int playerKey)
    {
        int num = 0;
        for (int i = 0; i < 7; i++)
        {
            if (canRemove(gameBoard, i, playerKey)) num++;
        }
        return num;
    }

    public static int getIndexOfRemove(int[] gameBoard, int index, int playerKey)
    {
        int counter = -1;
        for (int i = 0; i < 7; i++)
        {
            if (canRemove(gameBoard, i, playerKey)) counter++;
            if (counter == index - 7) return counter;
        }
        return -1;
    }

    public static bool canRemove(int[] gameBoard, int index, int playerKey)
    {
        return gameBoard[indexOf(index, 5)] == playerKey;
    }

    public static int[] removeFromGrid(int[] gameBoard, int x, int y)
    {
        for (int i = y; i > 0; i--)
        {
            gameBoard[indexOf(x, i)] = gameBoard[indexOf(x, i - 1)];
        }
        gameBoard[indexOf(x, 0)] = 0;
        return gameBoard;
    }

    public static int[] copyGameGrid(int[] gameBoard)
    {
        int[] newGrid = new int[7 * 6];
        for (int i = 0; i < 7 * 6; i++)
        {
            newGrid[i] = gameBoard[i];
        }
        return newGrid;
    }


    public static bool canInsert(int[] gameBoard, int index)
    {
        return gameBoard[indexOf(index, 0)] == 0;
    }

    public static void displayGrid(int[] gameBoard)
    {
        for (int i = 0; i < 6; i++)
        {
            string output = "";
            for (int j = 0; j < 7; j++)
            {
                output = output + "[" + gameBoard[indexOf(j, i)] + "]";
            }
            Debug.Log(output);
        }
        Debug.Log("*****************");
    }

    public static Vector2 insertingPosition(int[] gameBoard, int index)
    {
        for (int i = 1; i < 6; i++)
        {
            if (gameBoard[indexOf(index, i)] != 0)
            {
                return new Vector2(index, i - 1);
            }
        }
        return new Vector2(index, 5);
    }

    public static int[] insertToGrid(int[] gameBoard, Vector2 pos, int chesskey)
    {
        gameBoard[indexOf((int)pos.x, (int)pos.y)] = chesskey;
        return gameBoard;
    }
}
