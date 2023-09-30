using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardUnitManager : MonoBehaviour
{
    public GameObject BoardUnitPrefab;

    public int[] ShipSizes = { 2, 3, 3, 4, 5 };
    public int ShipSize = 2;
    public bool Vertical = true;

    private void OnEnable()
    {
        UIManager.OnChangeShip += UIManager_OnChangeShip;
        UIManager.OnChangeOrientation += UIManager_OnChangeOrientation;
    }

    private void UIManager_OnChangeOrientation()
    {
        Vertical = !Vertical;
    }

    private void UIManager_OnChangeShip(int id)
    {
        ShipSize = ShipSizes[id];
    }

    private void OnDisable()
    {
        UIManager.OnChangeShip -= UIManager_OnChangeShip;
        UIManager.OnChangeOrientation -= UIManager_OnChangeOrientation;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayerBoard();
        CreateAIBoard();
    }

    // Update is called once per frame
    public void CreatePlayerBoard()
    {
        //create a 10x10 board - Board 1
        int row = 1;
        int col = 1;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //instatiate prefab and place it properly on the scene
                GameObject tmp = GameObject.Instantiate(BoardUnitPrefab, new Vector3(i, 0, j), BoardUnitPrefab.transform.rotation) as GameObject;

                BoardUnit tmpUI = tmp.GetComponentInChildren<BoardUnit>()
;
                string name = string.Format("B1:[{0:00},{1:00}]", row, col);
                tmpUI.tmpBoardUnitLabel.text = name;
                tmpUI.col = j;
                tmpUI.row = i;

                //    board[i, j] = temp;
                tmp.name = name;
                col++;
            }
            col = 1;
            row++;
        }

    }


    public void CreateAIBoard()
    {
        //create a 10x10 board - Board 1
        int row = 1;
        int col = 1;
        for (int i = 11; i < 21; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //instatiate prefab and place it properly on the scene
                GameObject tmp = GameObject.Instantiate(BoardUnitPrefab, new Vector3(i, 0, j), BoardUnitPrefab.transform.rotation) as GameObject;

                BoardUnit tmpUI = tmp.GetComponentInChildren<BoardUnit>()
;
                string name = string.Format("B1:[{0:00},{1:00}]", row, col);
                tmpUI.tmpBoardUnitLabel.text = name;
                tmpUI.col = j;
                tmpUI.row = i;

                //    board[i, j] = temp;
                tmp.name = name;
                col++;
            }
            col = 1;
            row++;
        }

    }
    RaycastHit tmpHitHighlight = new RaycastHit();
    private void Update()
    {
        // we need to get mouse position *
        // we need to do raycasting *
        // identify which board unit we are interacting with *


        // the orientation and piece is known 
        // determine if piece is allowed to be placed at the given location

        // capture the mouse position and cast a ray to see what object we hit
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out tmpHitHighlight, 100))
        {
            if (tmpHitHighlight.transform.tag.Equals("BoardUnit"))
            {
                // we need to get the actual location of the board
                BoardUnit boardUnit = tmpHitHighlight.transform.GetComponent<BoardUnit>();

                if (!boardUnit.occupied)
                {
                    //check orientation
                    //check size
                    //render place holder

                    for(int i=0; i < ShipSize; i++)
                    {
                        GameObject v = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        v.transform.position = new Vector3(boardUnit.row + i,v.transform.position.y,boardUnit.col);
                        v.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    }
                }
            }
        }
    }
}

                //Debug.Log($"[{buInfo.Row},{buInfo.Col}]->{buInfo.Occupied}");

             //   if (tmpBlockHolder != null)
            //    {
            //        Destroy(this.tmpBlockHolder);
              //  }
              /*
                if (PLACE_BLOCK)
                {
                    tmpBlockHolder = new GameObject();
                    OK_TO_PLACE = true;

                    if (!vertical && (buInfo.Row <= 10 - blockSize))
                    {
                        for (int i = 0; i < this.blockSize; i++)
                        {
                            GameObject visual = GameObject.
                                                    Instantiate(CubePrefab,
                                                                new Vector3(buInfo.Row + i,
                                                                            CubePrefab.transform.position.y,
                                                                            buInfo.Col),
                                                                CubePrefab.transform.rotation) as GameObject;

                            GameObject bp = boardPlayer[buInfo.Row + i, buInfo.Col];
                            BoardUnitInfo bpUI = bp.GetComponent<BoardUnitInfo>();
                            if (!bpUI.Occupied)
                            {
                                visual.GetComponent<Renderer>().material.color = Color.white; // ok to place
                            }
                            else
                            {
                                visual.GetComponent<Renderer>().material.color = Color.yellow; // ok to place
                                OK_TO_PLACE = false;
                            }

                            visual.transform.parent = tmpBlockHolder.transform;
                        }
                    }
                    if (vertical && (buInfo.Col <= 10 - blockSize))
                    {
                        for (int i = 0; i < blockSize; i++)
                        {
                            GameObject visual = GameObject.
                                                    Instantiate(CubePrefab,
                                                                new Vector3(buInfo.Row,
                                                                            CubePrefab.transform.position.y,
                                                                            buInfo.Col + i),
                                                                CubePrefab.transform.rotation) as GameObject;

                            GameObject bp = boardPlayer[buInfo.Row, buInfo.Col + i];
                            BoardUnitInfo bpUI = bp.GetComponent<BoardUnitInfo>();
                            if (!bpUI.Occupied)
                            {
                                visual.GetComponent<Renderer>().material.color = Color.white; // ok to place
                            }
                            else
                            {
                                visual.GetComponent<Renderer>().material.color = Color.yellow; // ok to place
                                OK_TO_PLACE = false;
                            }

                            visual.transform.parent = tmpBlockHolder.transform;
                        }
                    }
                }
            }
        }
    }
              
}
*/