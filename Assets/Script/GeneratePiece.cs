using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePiece : MonoBehaviour
{
    public Chess redChess;
    public Chess yellowChess;
    public Transform[] spawnPosition;

    private bool redTurn;

    public KeyCode spawnCode;
    private float countdown;
    public float dropTime = 1.0f;

    //1 for red, 2 for yellow, 0 for none
    public int[,] gameGrid;

    // Start is called before the first frame update
    void Start()
    {
        countdown = 0;
        gameGrid = new int[7, 6];
        for (int i = 0; i < gameGrid.Length / gameGrid.GetUpperBound(0); i++)
        {
            for (int j = 0; j < gameGrid.GetUpperBound(0); j++)
            {
                gameGrid[i, j] = 0;
            }
        }
        redTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        countdown += Time.deltaTime;
        for (int i = 1; i < 8; i++)
        {
            if (Input.GetKeyDown(i.ToString())) spawnChess(i - 1);
        }
        //spawnChess(Random.Range(0, 7));
    }

    public void buttonPressed(int i)
    {
        //if (redTurn) return;    //if red turn is computer
        spawnChess(i);
    }

    public void spawnChess(int index)
    {
        if (countdown >= dropTime && BoardUtility.canInsert(gameGrid, index))
        {
            countdown = 0;
            int chessKey = 0;
            if (redTurn)
            {
                Chess chs = Instantiate(redChess) as Chess;
                chs.gameObject.transform.position = spawnPosition[index].position;
                chessKey = 1;
            }
            else
            {
                Chess chs = Instantiate(yellowChess) as Chess;
                chs.gameObject.transform.position = spawnPosition[index].position;
                chessKey = 2;
            }
            redTurn = !redTurn;

            //put it into the gamegrid
            gameGrid = BoardUtility.insertToGrid(gameGrid, index, chessKey);
            BoardUtility.displayGrid(gameGrid);
        }
    }
}
