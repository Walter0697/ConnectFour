using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePiece : MonoBehaviour
{
    public enum InputChoice
    {
        Player,
        Random,
        HeuristicOne,
        HeuristicTwo
    };

    public Chess redChess;
    public Chess yellowChess;
    public Transform[] spawnPosition;
    [HideInInspector]
    public List<Chess> all_chess;

    private bool redTurn;
    public InputChoice player1 = InputChoice.Player;
    public InputChoice player2 = InputChoice.HeuristicOne;
    public bool allowRemove = true;

    [HideInInspector]
    public KeyCode[] removeCode = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U };
    private float countdown;
    public float dropTime = 1.0f;
    public float computerThinkTime = 2.0f;
    public int depth = 5;

    //1 for red, 2 for yellow, 0 for none
    [HideInInspector]
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
        all_chess = new List<Chess>();
        redTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        countdown += Time.deltaTime;
        //player move
        //only usable when player is allowed to move
        if ((player1 == InputChoice.Player && redTurn) || (player2 == InputChoice.Player && !redTurn))
        {
            for (int i = 1; i < 8; i++)
            {
                if (Input.GetKeyDown(i.ToString())) buttonPressed(i - 1);
            }
            for (int i = 0; i < 7; i++)
            {
                if (allowRemove)
                {
                    if (Input.GetKeyDown(removeCode[i])) removeButtonPressed(i);
                }
                else
                    break;
            }
        }
        //if the first player is also a computer but with only random move
        if ((player1 == InputChoice.Random && redTurn) || (player2 == InputChoice.Random && !redTurn))
        {
            if (countdown >= computerThinkTime)
            {
                int move = MiniMaxScript.randomMove(gameGrid, 1, allowRemove);
                if (move < 7) spawnChess(move);
                else removeChess(BoardUtility.getIndexOfRemove(gameGrid, move, 1));
            }
        }

        //if vsComputer
        if ((player1 == InputChoice.HeuristicOne && redTurn) || (player2 == InputChoice.HeuristicOne && !redTurn) ||
            (player1 == InputChoice.HeuristicTwo && redTurn) || (player2 == InputChoice.HeuristicTwo && !redTurn))
        {
            int heuristic;
            int player;
            if (player1 == InputChoice.HeuristicOne && redTurn) { heuristic = 1; player = 1; }
            else if (player1 == InputChoice.HeuristicOne && !redTurn) { heuristic = 1; player = 2; }
            else if (player1 == InputChoice.HeuristicTwo && redTurn) { heuristic = 2; player = 1; }
            else { heuristic = 2; player = 2; }
            if (countdown >= computerThinkTime)
            {
                //set minimax to also allow remove
                if (allowRemove)
                {
                    //gameGrid, depth, playerKey, heristic
                    int num = MiniMaxScript.miniMaxResultWithRemove(gameGrid, depth, player, heuristic);
                    if (num < 7) spawnChess(num);
                    else removeChess(num - 7);
                }
                else
                    spawnChess(MiniMaxScript.miniMaxResult(gameGrid, depth, player, heuristic));
            }
        }
    }

    public void removeButtonPressed(int i)
    {
        if (player1 == InputChoice.Player && redTurn)
            removeChess(i);
        else if (player2 == InputChoice.Player && !redTurn)
            removeChess(i);
    }

    public void buttonPressed(int i)
    {
        if (player1 == InputChoice.Player && redTurn)
            spawnChess(i);
        else if (player2 == InputChoice.Player && !redTurn)
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
            all_chess.Add(chs);

            redTurn = !redTurn;

            //put it into the gamegrid
            gameGrid = BoardUtility.insertToGrid(gameGrid, pos, chessKey);
            BoardUtility.displayGrid(gameGrid);
        }
    }

    public void removeChess(int index)
    {
        int playerKey = (redTurn ? 1 : 2);
        if (countdown >= dropTime && BoardUtility.canRemove(gameGrid, index, playerKey))
        {
            for (int i = 0; i < all_chess.Count; i++)
            {
                if (all_chess[i].x == index && all_chess[i].y != 5)
                {
                    all_chess[i].x++;
                }
                else if (all_chess[i].x == index && all_chess[i].y == 5)
                {
                    Chess chs = all_chess[i];
                    all_chess.Remove(chs);
                    Destroy(chs.gameObject);
                }
            }

            redTurn = !redTurn;

            //remove it from the gamegrid
            gameGrid = BoardUtility.removeFromGrid(gameGrid, index, 5);
            BoardUtility.displayGrid(gameGrid);
        }
    }
}
