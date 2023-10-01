using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardUnitManager : MonoBehaviour
{
    public GameObject BoardUnitPrefab;
    public GameObject BoardUnitAttackPrefab;
    public GameObject BlockVisualizerPrefab;

    public BoardPlayer boardPlayer;
    public BoardAI boardEnemy;

    public int[] ShipSizes = { 2, 3, 3, 4, 5 };
    public int ShipSize = 2;
    public bool Vertical = true;

    [Header("Player Piece Model Prefact Reference")]
    public List<GameObject> boardPiecesPref;

    [Header("----")]
    //public int blockSize = 3;
    //public bool Oerientation = false;

    bool PLACE_BLOCK = true;

    [SerializeField]
    private int currentShipID;

    GameObject tmpHighlight = null;
    RaycastHit tmpHitHighlight;

    GameObject tmpBlockHolder = null;

    private bool OK_TO_PLACE = true;

    [SerializeField]
    private int count = 0;

    bool placeEnemyShips = true;

    GameObject tmpAttackHighlight = null;
    RaycastHit tmpAttackHitHighlight;

    GameObject tmpAttackBlockHolder = null;
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
        boardPlayer = new BoardPlayer(BoardUnitPrefab);
        boardPlayer.CreatePlayerBoard();

        boardEnemy = new BoardAI(BoardUnitAttackPrefab, BlockVisualizerPrefab);
        boardEnemy.CreateAiBoard();

        currentShipID = 0;
        ShipSize = 0;
    }
    void Update()
    {
        PlacePlayerPieces();
    }

    private void PlacePlayerPieces()
    {
        // capture the mouse position and cast a ray to see what object we hit
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      

        if (Input.mousePosition != null)
        {
            if (Physics.Raycast(ray, out tmpHitHighlight, 100))
            {
                BoardUnit tmpUI = tmpHitHighlight.transform.GetComponent<BoardUnit>();
                if (tmpHitHighlight.transform.tag.Equals("PlayerBoardUnit") && !tmpUI.occupied)
                {
                    BoardUnit boardData = boardPlayer.board[tmpUI.row, tmpUI.col].transform.GetComponent<BoardUnit>();

                    if (tmpHighlight != null)
                    {
                        if (boardData.occupied)
                            tmpHighlight.GetComponent<Renderer>().material.color = Color.red;
                        else
                            tmpHighlight.GetComponent<Renderer>().material.color = Color.white;
                    }

                    if (tmpBlockHolder != null)
                    {
                        Destroy(tmpBlockHolder);
                    }

                    if (PLACE_BLOCK)
                    {
                        tmpBlockHolder = new GameObject();
                        OK_TO_PLACE = true;

                        if (!Vertical && (tmpUI.row <= 10 - ShipSize))
                        {
                            for (int i = 0; i < ShipSize; i++)
                            {
                                GameObject visual = GameObject.Instantiate(BlockVisualizerPrefab, new Vector3(tmpUI.row + i,
                                    BlockVisualizerPrefab.transform.position.y, tmpUI.col), BlockVisualizerPrefab.transform.rotation) as GameObject;
                                GameObject bp = boardPlayer.board[tmpUI.row + i, tmpUI.col];
                                BoardUnit bpUI = bp.GetComponentInChildren<BoardUnit>();
                                if (!bpUI.occupied)
                                {
                                    visual.GetComponent<Renderer>().material.color = Color.grey;
                                }
                                else
                                {
                                    //visual.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                    visual.GetComponent<Renderer>().material.color = Color.yellow; //not ok
                                    OK_TO_PLACE = false;
                                }

                                visual.transform.parent = this.tmpBlockHolder.transform;
                            }
                        }
                        if (Vertical && (tmpUI.col <= 10 - ShipSize))
                        {

                            for (int i = 0; i < ShipSize; i++)
                            {

                                GameObject visual = GameObject.Instantiate(BlockVisualizerPrefab, new Vector3(tmpUI.row,
                                    BlockVisualizerPrefab.transform.position.y, tmpUI.col + i), BlockVisualizerPrefab.transform.rotation) as GameObject;
                                GameObject bp = boardPlayer.board[tmpUI.row, tmpUI.col + i];
                                BoardUnit bpUI = bp.GetComponentInChildren<BoardUnit>();
                                if (!bpUI.occupied)
                                {
                                    visual.GetComponent<Renderer>().material.color = Color.gray; //okay to place
                                }
                                else
                                {
                                    //visual.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                    visual.GetComponent<Renderer>().material.color = Color.yellow; //not ok
                                    OK_TO_PLACE = false;
                                }
                                visual.transform.parent = this.tmpBlockHolder.transform;
                            }

                        }

                    }
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            // capture the mouse position and cast a ray to see what object we hit
            RaycastHit hit;

            RaycastHit[] hits = Physics.RaycastAll(ray, 100);
            foreach (var x in hits)
            {
                Debug.Log("Hit object: " +  x.transform.name);
            }


            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag.Equals("PlayerBoardUnit"))
                {
                    BoardUnit tmpUI = hit.transform.GetComponentInChildren<BoardUnit>();

                    if (PLACE_BLOCK && OK_TO_PLACE)
                    {
                        if (!Vertical)
                        {
                            for(int i = 0; i < ShipSize; i++)
                            {
                                GameObject sB = boardPlayer.board[tmpUI.row + i, tmpUI.col];

                                BoardUnit bu = sB.transform.GetComponentInChildren<BoardUnit>();
                                bu.occupied = true;
                           //     bu.CubePrefab.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                            //    bu.CubePrefab.gameObject.SetActive(true);
                                bu.GetComponent<MeshRenderer>().material.color = Color.green;
                                boardPlayer.board[tmpUI.row + i, tmpUI.col] = sB;

                            }
                        }

                        if (Vertical)
                        {
                            for(int i = 0; i < ShipSize; i++)
                            {
                                GameObject sB = boardPlayer.board[tmpUI.row, tmpUI.col + i];

                                BoardUnit bu = sB.transform.GetComponentInChildren<BoardUnit>();
                                bu.occupied = true;
                                bu.GetComponent<MeshRenderer>().material.color = Color.green;

                                boardPlayer.board[tmpUI.row, tmpUI.col + i] = sB;

                            }
                        }
                    }
                }
            }
        }
    }
}
