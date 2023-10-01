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
}
