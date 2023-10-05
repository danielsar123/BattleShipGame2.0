using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAI : Board 
{
    GameObject cubePrefab;
    int[] aiShipSizes = new int[5] { 2, 3, 3, 4, 5 };

    public BoardAI(GameObject unitPrefab, GameObject prefab)
    {
        BoardUnitPrefab = unitPrefab;
        cubePrefab = prefab;

        ClearBoard();
    }

    public void CreateAiBoard()
    {
        //create a 10x10 board - Board 2
        int row = 1;
        int col = 1;
        for (int i = 11; i < 21; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //instatiate prefab and place it properly on the scene
                GameObject tmp = GameObject.Instantiate(BoardUnitPrefab, new Vector3(i, 0, j), BoardUnitPrefab.transform.rotation) as GameObject;

                BoardUnit tmpUI = tmp.GetComponentInChildren<BoardUnit>();
;
                string name = string.Format("B2:[{0:00},{1:00}]", row, col);
                tmpUI.tmpBoardUnitLabel.text = name;
                tmpUI.col = col - 1;
                tmpUI.row = row - 1;

                board[tmpUI.row, tmpUI.col] = tmp;
                tmp.name = name;
                col++;
            }
            col = 1;
            row++;
        }

    }

    // Logic for placing enemy warships
    public void PlaceShips()
    {
        //placeEnemyShips = false;
        for (int i = 0; i < aiShipSizes.Length; i++)
        {
            Debug.Log("I AM IN THE PLACE ENEMY SHIP FUNCTION");

            int row = Random.Range(0, 9);
            int col = Random.Range(0, 9);

            bool ori = (Random.Range(0, 9) > 5) ? true : false;

            Debug.Log(string.Format("Placing ship {0} at location {1}, {2} with orientation: {3}", i, row, col, ori));

            CheckBoardForPlacement(row, col, aiShipSizes[i], ori);
        }
    }

    private void CheckBoardForPlacement(int row, int col, int size, bool hor)
    {
        bool shipPlaced = false;
        int maxAttempts = 100;
        int currentAttempt = 0;

        while (!shipPlaced && currentAttempt < maxAttempts)
        {
            bool okToPlace = true;

            // Check boundaries and occupancy
            if (hor)
            {
                if (col + size > 9)
                    okToPlace = false;

                for (int i = 0; i < size && okToPlace; i++)
                {
                    if (board[row, col + i].GetComponentInChildren<BoardUnit>().occupied)
                        okToPlace = false;
                }
            }
            else
            {
                if (row + size > 9)
                    okToPlace = false;

                for (int i = 0; i < size && okToPlace; i++)
                {
                    if (board[row + i, col].GetComponentInChildren<BoardUnit>().occupied)
                        okToPlace = false;
                }
            }

            if (okToPlace)
            {
                if (hor)
                {
                    for (int i = 0; i < size; i++)
                    {
                        board[row, col + i].GetComponentInChildren<BoardUnit>().occupied = true;
                        GameObject visual = GameObject.Instantiate(cubePrefab, new Vector3(row + 11, 0.4f, col + i), cubePrefab.transform.rotation);
                        visual.GetComponent<Renderer>().material.color = Color.yellow;
                    }
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        board[row + i, col].GetComponentInChildren<BoardUnit>().occupied = true;
                        GameObject visual = GameObject.Instantiate(cubePrefab, new Vector3(row + i + 11, 0.4f, col), cubePrefab.transform.rotation);
                        visual.GetComponent<Renderer>().material.color = Color.yellow;
                    }
                }

                shipPlaced = true;
            }
            else
            {
                row = Random.Range(0, 9);
                col = Random.Range(0, 9);
                hor = Random.Range(0, 2) > 0; // Randomly pick between horizontal and vertical again
                currentAttempt++;
            }
        }

        if (!shipPlaced)
        {
            Debug.LogWarning("Couldn't place ship after " + maxAttempts + " attempts! Consider another action here.");
        }
    }
}
