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
               numOfConnect(gameGrid, (key + 1) % 2, 4) * 50000 -
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

    public static int miniMaxResult(int[] gameGrid, int depth, int playerKey)
    {
        return minimax(gameGrid, depth, true, playerKey, -1).index;
    }

    public static Node minimax(int[] gameGrid, int depth, bool maximizingPlayer, int playerKey, int index)
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
                    Node n1 = minimax(newGrid, depth - 1, false, playerKey, i);
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
                }
                //n.value = heristic1(gameGrid, playerKey) - heristic1(gameGrid, (playerKey + 1) % 2);
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
                    Node n1 = minimax(newGrid, depth - 1, true, playerKey, i);
                    if (n1.value < n.value)
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
                }
                //n.value = heristic1(gameGrid, playerKey) - heristic1(gameGrid, (playerKey + 1) % 2);
                n.index = j;
                return n;
            }
        }
    }
}
