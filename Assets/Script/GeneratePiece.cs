using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePiece : MonoBehaviour
{
    public Chess redChess;
    public Chess yellowChess;
    public Transform[] spawnPosition;

    private bool redTurn;
    public bool vsComputer = true;

    public KeyCode testCode;
    private float countdown;
    public float dropTime = 1.0f;
    public float computerThinkTime = 1.0f;

    //1 for red, 2 for yellow, 0 for none
    public int[] gameGrid;

    // Start is called before the first frame update
    void Start()
    {
        countdown = 0;
        gameGrid = new int[7 * 6];
        for (int i = 0; i < gameGrid.Length; i++)
        {
                gameGrid[i] = 0;
        }
        redTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        countdown += Time.deltaTime;
        for (int i = 1; i < 8; i++)
        {
            if (Input.GetKeyDown(i.ToString())) buttonPressed(i - 1);
        }
        if (vsComputer && !redTurn)
        {
            spawnChess(MiniMaxScript.miniMaxResult(gameGrid, 4, 2));
        }
    }

    public void buttonPressed(int i)
    {
        if (vsComputer && redTurn)
            spawnChess(i);
    }

    public void spawnChess(int index)
    {
        if (countdown >= dropTime && BoardUtility.canInsert(gameGrid, index))
        {
            countdown = 0;
            int chessKey = 0;
            //get the position to insert
            Vector2 pos = BoardUtility.insertingPosition(gameGrid, index);
            Chess chs;
            if (redTurn)
            {
                chs = Instantiate(redChess) as Chess;
                chs.gameObject.transform.position = spawnPosition[index].position;
                chessKey = 1;
            }
            else
            {
                chs = Instantiate(yellowChess) as Chess;
                chs.gameObject.transform.position = spawnPosition[index].position;
                chessKey = 2;
            }
            chs.x = (int)pos.x;
            chs.y = (int)pos.y;

            redTurn = !redTurn;

            //put it into the gamegrid
            gameGrid = BoardUtility.insertToGrid(gameGrid, pos, chessKey);
            BoardUtility.displayGrid(gameGrid);
        }
    }
}
