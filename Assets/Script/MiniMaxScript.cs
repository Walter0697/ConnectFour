using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxScript : MonoBehaviour
{
    public struct Node
    {
        public int index;
        public int value;
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool isConnect(int[] gameGrid, int key, int num, int x, int y, int dx, int dy)
    {
        for (int i = 0; i < num; i++)
        {
            int cx = x + dx * i;
            int cy = y + dy * i;
            if (BoardUtility.isOutOfBound(cx, cy)) return false;
            if (gameGrid[BoardUtility.indexOf(cx, cy)] != key) return false;
        }
        return true;
    }

    public static int heristic1(int[] gameGrid, int key)
    {
        return numOfConnect(gameGrid, key, 4) * 10000 +
               numOfConnect(gameGrid, key, 3) * 50 +
               numOfConnect(gameGrid, key, 2) * 2 -
               numOfConnect(gameGrid, (key + 1) % 2, 4) * 80000 -
               numOfConnect(gameGrid, (key + 1) % 2, 3) * 100 -
               numOfConnect(gameGrid, (key + 1) % 2, 2) * 5;
    }

    public static int numOfConnect(int[] gameGrid, int key, int num)
    {
        int returnNum = 0;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            { 
                if (isConnect(gameGrid, key, num, i, j, 0, 1)) { returnNum += 1; i += 1; }
                if (isConnect(gameGrid, key, num, i, j, 1, 0)) { returnNum += 1; i += 1; }
                if (isConnect(gameGrid, key, num, i, j, 1, 1)) { returnNum += 1; i += 1; }
                if (isConnect(gameGrid, key, num, i, j, 1, -1)) { returnNum += 1; i += 1; }
            }
        }

        return returnNum;
    }

    public static int miniMaxResultWithRemove(int[] gameGrid, int depth, int playerKey)
    {
        return minimaxWithRemove(gameGrid, depth, -9999999, 9999999, true, playerKey, -1).index;
    }

    public static int miniMaxResult(int[] gameGrid, int depth, int playerKey)
    {
        return minimax(gameGrid, depth, -9999999, 9999999, true, playerKey, -1).index;
    }

    public static Node minimax(int[] gameGrid, int depth, int alpha, int beta, bool maximizingPlayer, int playerKey, int index)
    {

        if (depth == 0)
        {
            Node n = new Node();
            n.value = heristic1(gameGrid, playerKey);
            n.index = index;
            return n;
        }
        else
        {
            if (maximizingPlayer)
            {
                Node n = new Node();
                n.value = -9999999;
                int j = -1;
                for (int i = 0; i < 7; i++)
                {
                    if (!BoardUtility.canInsert(gameGrid, i)) break;
                    int[] newGrid = BoardUtility.copyGameGrid(gameGrid);
                    BoardUtility.insertToGrid(newGrid, BoardUtility.insertingPosition(newGrid, i), playerKey);
                    Node n1 = minimax(newGrid, depth - 1, alpha, beta, false, playerKey, i);
                    if (n1.value >= n.value)
                    {
                        n = n1;
                        j = i;
                    }
                    if (n1.value >= alpha) alpha = n1.value;
                    if (beta <= alpha) break;
                }
                n.index = j;
                return n;
            }
            else
            {
                Node n = new Node();
                n.value = 99999999;
                int key = (playerKey + 1) % 2;
                int j = -1;
                for (int i = 0; i < 7; i++)
                {
                    if (!BoardUtility.canInsert(gameGrid, i)) break;
                    int[] newGrid = BoardUtility.copyGameGrid(gameGrid);
                    BoardUtility.insertToGrid(newGrid, BoardUtility.insertingPosition(newGrid, i), key);
                    Node n1 = minimax(newGrid, depth - 1, alpha, beta, true, playerKey, i);
                    if (n1.value <= n.value)
                    {
                        n = n1;
                        j = i;
                    }
                    if (beta >= n1.value) beta = n1.value;
                    if (beta <= alpha) break;
                }
                n.index = j;
                return n;
            }
        }
    }

    public static Node minimaxWithRemove(int[] gameGrid, int depth, int alpha, int beta, bool maximizingPlayer, int playerKey, int index)
    {

        if (depth == 0)
        {
            Node n = new Node();
            n.value = heristic1(gameGrid, playerKey);
            n.index = index;
            return n;
        }
        else
        {
            if (maximizingPlayer)
            {
                Node n = new Node();
                n.value = -9999999;
                int j = -1;
                for (int i = 0; i < 14; i++)
                {
                    if (i < 7 && !BoardUtility.canInsert(gameGrid, i)) break;
                    else if (i >= 7 && !BoardUtility.canRemove(gameGrid, i - 7, playerKey)) break;

                    int[] newGrid = BoardUtility.copyGameGrid(gameGrid);

                    if (i < 7) BoardUtility.insertToGrid(newGrid, BoardUtility.insertingPosition(newGrid, i), playerKey);
                    else BoardUtility.removeFromGrid(newGrid, i - 7, 5);

                    Node n1 = minimax(newGrid, depth - 1, alpha, beta, false, playerKey, i);
                    if (n1.value >= n.value)
                    {
                        n = n1;
                        j = i;
                    }
                    if (n1.value >= alpha) alpha = n1.value;
                    if (beta <= alpha) break;
                }
                n.index = j;
                return n;
            }
            else
            {
                Node n = new Node();
                n.value = 99999999;
                int key = (playerKey + 1) % 2;
                int j = -1;
                for (int i = 0; i < 7; i++)
                {
                    if (i < 7 && !BoardUtility.canInsert(gameGrid, i)) break;
                    else if (i >= 7 && !BoardUtility.canRemove(gameGrid, i - 7, key)) break;

                    int[] newGrid = BoardUtility.copyGameGrid(gameGrid);

                    if (i < 7) BoardUtility.insertToGrid(newGrid, BoardUtility.insertingPosition(newGrid, i), key);
                    else BoardUtility.removeFromGrid(newGrid, i - 7, 5);

                    Node n1 = minimax(newGrid, depth - 1, alpha, beta, true, playerKey, i);
                    if (n1.value <= n.value)
                    {
                        n = n1;
                        j = i;
                    }
                    if (beta >= n1.value) beta = n1.value;
                    if (beta <= alpha) break;
                }
                n.index = j;
                return n;
            }
        }
    }

}
