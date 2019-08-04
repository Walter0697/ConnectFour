using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxScript : MonoBehaviour
{
    public static bool alphaBeta = true;

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

    //check if the pieces are connecting to each other in the location
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

    //check if it is possible to connect in the location
    //such as [x][x][ ][x]
    //but will return false if it is for example [x][x][o][x]
    public static bool canConnect(int[] gameGrid, int key, int num, int x, int y, int dx, int dy)
    {
        int checkkey = (key + 1) % 2;
        for (int i = 0; i < num; i++)
        {
            int cx = x + dx * i;
            int cy = y + dy * i;
            if (BoardUtility.isOutOfBound(cx, cy)) return false;
            if (gameGrid[BoardUtility.indexOf(cx, cy)] == checkkey) return false;
        }
        return true;
    }

    public static int heristic(int[] gameGrid, int key, int h)
    {
        return heristic1(gameGrid, key);
    }

    //check the number of connect and and give the heuristic value
    public static int heristic1(int[] gameGrid, int key)
    {
        return numOfConnect(gameGrid, key, 4) * 10000 +
               numOfConnect(gameGrid, key, 3) * 50 +
               numOfConnect(gameGrid, key, 2) * 2 -
               numOfConnect(gameGrid, (key + 1) % 2, 4) * 100000 -
               numOfConnect(gameGrid, (key + 1) % 2, 3) * 100 -
               numOfConnect(gameGrid, (key + 1) % 2, 2) * 5;
    }

    //check the number of possible connection
    //not a very good AI
    public static int heristic2(int[] gameGrid, int key)
    {
        return numOfCanConnect(gameGrid, key, 4) * 1000 - numOfCanConnect(gameGrid, (key + 1) % 2, 4) * 10000;
    }

    public static int numOfCanConnect(int[] gameGrid, int key, int num)
    {
        int returnNum = 0;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (canConnect(gameGrid, key, num, i, j, 0, 1)) { returnNum += 1; i++; }
                if (canConnect(gameGrid, key, num, i, j, 1, 0)) { returnNum += 1; i++;  }
                if (canConnect(gameGrid, key, num, i, j, 1, 1)) { returnNum += 1; i++;  }
                if (canConnect(gameGrid, key, num, i, j, 1, -1)) { returnNum += 1; i++;  }
            }
        }
        return returnNum;
    }

    public static int numOfConnect(int[] gameGrid, int key, int num)
    {
        int returnNum = 0;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            { 
                if (isConnect(gameGrid, key, num, i, j, 0, 1)) { returnNum += 1; }
                if (isConnect(gameGrid, key, num, i, j, 1, 0)) { returnNum += 1; }
                if (isConnect(gameGrid, key, num, i, j, 1, 1)) { returnNum += 1; }
                if (isConnect(gameGrid, key, num, i, j, 1, -1)) { returnNum += 1; }
            }
        }

        return returnNum;
    }

    public static int randomMove(int[] gameGrid, int key, bool allowRemove)
    {
        if (!allowRemove) return Random.Range(0, 7);
        return Random.Range(0, 7 + BoardUtility.numOfRemove(gameGrid, key));
    }

    //return the index of the highest value from minimax algorithm
    public static int miniMaxResultWithRemove(int[] gameGrid, int depth, int playerKey, int h)
    {
        return minimaxWithRemove(gameGrid, depth, -9999999, 9999999, true, playerKey, -1, h).index;
    }

    //return the index of the highest value from minimax algorithm
    public static int miniMaxResult(int[] gameGrid, int depth, int playerKey, int h)
    {
        return minimax(gameGrid, depth, -9999999, 9999999, true, playerKey, -1, h).index;
    }

    //minimax algorithm that will expend its search and return the heuristic value for different result
    public static Node minimax(int[] gameGrid, int depth, int alpha, int beta, bool maximizingPlayer, int playerKey, int index, int h)
    {

        if (depth == 0)
        {
            Node n = new Node();
            n.value = heristic(gameGrid, playerKey, h);
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
                    Node n1 = minimax(newGrid, depth - 1, alpha, beta, false, playerKey, i, h);
                    if (n1.value > n.value)
                    {
                        n = n1;
                        j = i;
                    }
                    else if (n1.value == n.value)
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            n = n1;
                            j = i;
                        }
                    }
                    if (alphaBeta && n1.value >= alpha) alpha = n1.value;
                    if (alphaBeta && beta <= alpha) break;
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
                    Node n1 = minimax(newGrid, depth - 1, alpha, beta, true, playerKey, i, h);
                    if (n1.value <= n.value)
                    {
                        n = n1;
                        j = i;
                    }
                    else if (n1.value == n.value)
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            n = n1;
                            j = i;
                        }
                    }
                    if (alphaBeta && beta >= n1.value) beta = n1.value;
                    if (alphaBeta && beta <= alpha) break;
                }
                n.index = j;
                return n;
            }
        }
    }

    //minimax algorithm that will expend its search and return the heuristic value for different result
    //with remove option
    public static Node minimaxWithRemove(int[] gameGrid, int depth, int alpha, int beta, bool maximizingPlayer, int playerKey, int index, int h)
    {

        if (depth == 0)
        {
            Node n = new Node();
            n.value = heristic(gameGrid, playerKey, h);
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

                    Node n1 = minimax(newGrid, depth - 1, alpha, beta, false, playerKey, i, h);
                    if (n1.value >= n.value)
                    {
                        n = n1;
                        j = i;
                    }
                    else if (n1.value == n.value)
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            n = n1;
                            j = i;
                        }
                    }
                    if (alphaBeta && n1.value >= alpha) alpha = n1.value;
                    if (alphaBeta && beta <= alpha) break;
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

                    Node n1 = minimax(newGrid, depth - 1, alpha, beta, true, playerKey, i, h);
                    if (n1.value <= n.value)
                    {
                        n = n1;
                        j = i;
                    }
                    else if (n1.value == n.value)
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            n = n1;
                            j = i;
                        }
                    }
                    if (alphaBeta && beta >= n1.value) beta = n1.value;
                    if (alphaBeta && beta <= alpha) break;
                }
                n.index = j;
                return n;
            }
        }
    }

}
