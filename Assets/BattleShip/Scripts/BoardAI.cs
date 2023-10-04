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
        Debug.Log("Starting Verification Process");
        // logic is following:
        // 1) we need to check and see that at the given location, we can place our ship
        // 2) If it is avialble, then we will go ahead and put it in the location
        GameObject checkUnit = board[row, col];

        var bu = checkUnit.GetComponentInChildren<BoardUnit>();

        if (bu.occupied || (row + size > 9) || (col + size > 9))
        {
            Debug.Log(string.Format("Location occupied at [{0}, {1}]", row, col));
            // it is occupied, generate new random row/col and call function again...
            int r1 = Random.Range(0, 9);
            int c1 = Random.Range(0, 9);
            Debug.Log(string.Format("RE-TRY WITH NEW COORDINATES [{0}, {1}]", r1, c1));
            CheckBoardForPlacement(r1, c1, size, hor);
            return;
        }

        // We need one pass for verification 

        bool okToPlace = true;
        Debug.Log(string.Format("Starting pass 1 for plapcement. OK? {0}", okToPlace));

        // if (!hor && (row <= 10 - size))
        if (!hor && (row + size < 10))
        {
            for (int i = 0; i < size; i++)
            {
                GameObject bp = board[row = i, col];
                BoardUnit bpUI = bp.GetComponentInChildren<BoardUnit>();
                if (!bpUI.occupied)
                {
                    // visual.GetComponent<Renderer>().material.color = Color.magneta; // ok to placr
                    //okToPlace = true;

                }
                else
                {
                    //visual.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    //visual.GetComponent<Renderer>().material.color = Color.yellow; // not ok
                    okToPlace = false;
                }
                //visual transform.parent = this.tmpBlockHolder.transform;
            }
        }
        if(hor && (col + size < 10))
        {
            for(int i = 0; i < size; i++)
            {
                GameObject bp = board[row, col + i];
                BoardUnit bpUI = bp.GetComponentInChildren<BoardUnit>();
                if (!bpUI.occupied)
                {
                    //visual.GetComponent<Renderer>().material.color = Color.magneta; // ok to place
                    // okToPlace = true;
                }
                else
                {
                    // visual.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    // visual.GetComponent<Renderer>().material.color = Color.yellow; // not ok
                    okToPlace = false;
                }

                // visual.transform.parent = this.tmpBlockHolder.transform
            }
        }
        Debug.Log(string.Format("END PASS 1 for Placement, OK? {0}", okToPlace));

        // next pass for actual placement
        if (okToPlace)
        {
            if (!hor)
            {
                for(int i = 0; i < size; i++)
                {
                    // these game objects can be removed since they are only for debugging 
                    GameObject visual = GameObject.Instantiate(cubePrefab, new Vector3(row + i + 11, 0.9f, col),
                                                               cubePrefab.transform.rotation) as GameObject;
                    visual.GetComponent<Renderer>().material.color = Color.yellow;
                    //visual.tag = "enemyPrefabPH";

                    GameObject sB = board[row + i, col];
                    sB.GetComponentInChildren<BoardUnit>().occupied = true;
                    board[row + i, col] = sB;
                    visual.gameObject.name = string.Format("EN-C-[{0},{1}]", row, col + i);
                    Debug.Log(string.Format("Enemy ship will be place at location [{0}, {1}]", row + i, col));
                }
            }
            if (hor)
            {
                for (int i = 0; i < size; i++)
                {
                    // these game objects can be removed since they are only for debugging 
                    GameObject visual = GameObject.Instantiate(cubePrefab, new Vector3(row  + 11, 0.9f, col + i),
                                                               cubePrefab.transform.rotation) as GameObject;
                    visual.GetComponent<Renderer>().material.color = Color.yellow;
                    //visual.tag = "enemyPrefabPH";

                    GameObject sB = board[row, col + i];
                    sB.GetComponentInChildren<BoardUnit>().occupied = true;
                    board[row, col + i] = sB;
                    visual.gameObject.name = string.Format("EN-C-[{0},{1}]", row, col + i);
                    Debug.Log(string.Format("Enemy ship will be place at location [{0}, {1}]", row + i, col));
                }
            }
        }
        else
        {
            int r1 = Random.Range(0, 9);
            int c1 = Random.Range(0, 9);

            Debug.Log(string.Format("PLACEMENT WAS {2}, STARTING AGAIN, NEW LOCATION [{0}, {1}]", r1, c1, okToPlace));

            CheckBoardForPlacement(r1, c1, size, hor);
        }

    }
}
