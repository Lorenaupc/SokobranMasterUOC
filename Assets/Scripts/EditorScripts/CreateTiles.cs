using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTiles : MonoBehaviour
{
    public GameObject crate;
    public GameObject floor;
    public GameObject goal;
    public GameObject player;
    public GameObject wall;
    public GameObject separation;

    private Sprite normalCrate;
    private Sprite normalGoal;
    private Sprite normalPlayer;
    private Sprite normalWall;
    private Sprite normalFloor;

    public Sprite selectedCrate;
    public Sprite selectedGoal;
    public Sprite selectedPlayer;
    public Sprite selectedWall;
    public Sprite selectedFloor;

    public GameObject crateButton;
    public GameObject wallButton;
    public GameObject goalButton;
    public GameObject playerButton;
    public GameObject floorButton;

    internal GameObject selectedTile;
    internal GameObject boardPlayer;

    public Text columns;
    public Text rows;

    private float scale;

    internal Transform board;

    private void Start()
    {
        selectedTile = null;
        boardPlayer = null;

        normalCrate = crateButton.GetComponentInChildren<Image>().sprite;
        normalGoal = goalButton.GetComponent<Image>().sprite;
        normalPlayer = playerButton.GetComponent<Image>().sprite;
        normalWall = wallButton.GetComponent<Image>().sprite;
        normalFloor = floorButton.GetComponent<Image>().sprite;
    }

    public void CreateMap()
    {
        scale = GameManager.scale;
        if (columns.text != null && rows.text != null && rows.text != "0" && columns.text != "0")
        {
            if (board != null)
            {
                Destroy(board.gameObject);
            }

            board = new GameObject("Board").transform;
            GetComponent<LoadSaveLevel>().board = board;

            int maxRows = int.Parse(rows.text);
            int maxColumns = int.Parse(columns.text);
            
            
            for (int i = 0; i < maxRows; i++)
            {
                for (int z = 0; z < maxColumns; z++)
                {
                    GameObject tile = null;
                    if (z == 0 || z == maxColumns - 1)
                    {
                        tile = wall;
                    }
                    else if (i == 0 || i == maxRows - 1)
                    {
                        tile = wall;
                    }
                    else tile = floor;

                    GameObject instance = Instantiate(tile, new Vector3((z/scale)-5, ((maxRows-i)/scale)-5, 0), Quaternion.identity);
                    instance.transform.SetParent(board);

                    if (z == maxColumns - 1)
                    {
                        tile = separation;
                        instance = Instantiate(tile, new Vector3((z / scale) - 5, ((maxRows - i) / scale) - 5, 0), Quaternion.identity);
                        instance.transform.SetParent(board);
                    }
                }
            }

            float halfTile = 1 / (scale);
            board.position = new Vector3((maxRows / 2 - halfTile) / scale, (maxColumns / 2 + halfTile) / scale, 0);
        }
        else if (columns.text == "0" || rows.text == "0")
        {
            Debug.Log("El número debe ser positivo");
            //Dialogo de que debe ser >0
        }
    }

    public void disableSelectedTile()
    {
        if (selectedTile == wall)
        {
            wallButton.GetComponent<Image>().sprite = normalWall;
        }
        else if (selectedTile == crate)
        {
            crateButton.GetComponent<Image>().sprite = normalCrate;
        }
        else if (selectedTile == goal)
        {
            goalButton.GetComponent<Image>().sprite = normalGoal;
        }
        else if (selectedTile == player)
        {
            playerButton.GetComponent<Image>().sprite = normalPlayer;
        }
        else if (selectedTile == floor)
        {
            floorButton.GetComponent<Image>().sprite = normalFloor;
        }
        selectedTile = null;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (selectedTile == null)
                {
                    //Si no se ha seleccionado ningun elemento
                    if (hit.transform.tag != "Untagged" && hit.transform.parent != board)
                    {
                        GameObject hitted = hit.transform.gameObject;
                        switch (hit.transform.tag)
                        {
                            case ("Wall"):
                                selectedTile = wall;
                                hitted.GetComponent<Image>().sprite = selectedWall;
                                break;
                            case ("Crate"):
                                selectedTile = crate;
                                hitted.GetComponent<Image>().sprite = selectedCrate;
                                break;
                            case ("Player"):
                                selectedTile = player;
                                hitted.GetComponent<Image>().sprite = selectedPlayer;
                                break;
                            case ("Floor"):
                                selectedTile = floor;
                                hitted.GetComponent<Image>().sprite = selectedFloor;
                                break;
                            case ("Goal"):
                                selectedTile = goal;
                                hitted.GetComponent<Image>().sprite = selectedGoal;
                                break;
                        }
                    }
                }
                else
                {
                    //Si se ha seleccionado un elemento y se ha pulsado en algún sitio del tablero
                    if (hit.transform.parent == board)
                    {
                        if (selectedTile == player)
                        {
                            //Si ya hay un player en escena
                            if (boardPlayer != null)
                            {
                                //Invocamos el nuevo player en la nueva posicion
                                Vector3 position = hit.transform.position;
                                GameObject hitted = hit.transform.gameObject;
                                int index = hitted.transform.GetSiblingIndex();

                                //Si la posicion es distinta a la anterior
                                if (position != boardPlayer.transform.position)
                                {
                                    Destroy(hitted);
                                    GameObject instance = Instantiate(selectedTile, position, Quaternion.identity);
                                    instance.transform.SetParent(board);
                                    instance.transform.SetSiblingIndex(index);

                                    //Borramos el player de la posicion anterior, solo puede haber 1 player activo en el tablero
                                    Vector3 playerBoardPosition = boardPlayer.transform.position;
                                    int indexPlayerBoard = boardPlayer.transform.GetSiblingIndex();
                                    Destroy(boardPlayer);
                                    GameObject floorInstance = Instantiate(floor, playerBoardPosition, Quaternion.identity);
                                    floorInstance.transform.SetParent(board);
                                    floorInstance.transform.SetSiblingIndex(indexPlayerBoard);

                                    boardPlayer = instance;
                                }
                            }
                            //Si no hay un player en escena invocamos directamente
                            else
                            {
                                Vector3 position = hit.transform.position;
                                GameObject hitted = hit.transform.gameObject;
                                int index = hitted.transform.GetSiblingIndex();
                                Destroy(hitted);
                                GameObject instance = Instantiate(selectedTile, position, Quaternion.identity);
                                instance.transform.SetParent(board);
                                instance.transform.SetSiblingIndex(index);
                                boardPlayer = instance;
                            }
                        }
                        else
                        {
                            Vector3 position = hit.transform.position;
                            GameObject hitted = hit.transform.gameObject;
                            int index = hitted.transform.GetSiblingIndex();
                            Destroy(hitted);                            
                            GameObject instance = Instantiate(selectedTile, position, Quaternion.identity);
                            instance.transform.SetParent(board);
                            instance.transform.SetSiblingIndex(index);
                        }
                    }

                    //Si se ha seleccionado un elemento y se ha pulsado en otro elemento para completar el mapa
                    else if (hit.transform.tag != "Untagged" && hit.transform.gameObject.layer == 5)
                    {
                        GameObject hitted = hit.transform.gameObject;
                        if (selectedTile == wall)
                        {
                            wallButton.GetComponent<Image>().sprite = normalWall;
                        }
                        else if (selectedTile == crate)
                        {
                            crateButton.GetComponent<Image>().sprite = normalCrate;
                        }
                        else if (selectedTile == goal)
                        {
                            goalButton.GetComponent<Image>().sprite = normalGoal;
                        }
                        else if (selectedTile == player)
                        {
                            playerButton.GetComponent<Image>().sprite = normalPlayer;
                        }
                        else if (selectedTile == floor)
                        {
                            floorButton.GetComponent<Image>().sprite = normalFloor;
                        }

                        switch (hit.transform.tag)
                        {
                            case ("Wall"):
                                selectedTile = wall;
                                hitted.GetComponent<Image>().sprite = selectedWall;
                                break;
                            case ("Crate"):
                                selectedTile = crate;
                                hitted.GetComponent<Image>().sprite = selectedCrate;
                                break;
                            case ("Player"):
                                selectedTile = player;
                                hitted.GetComponent<Image>().sprite = selectedPlayer;
                                break;
                            case ("Floor"):
                                selectedTile = floor;
                                hitted.GetComponent<Image>().sprite = selectedFloor;
                                break;
                            case ("Goal"):
                                selectedTile = goal;
                                hitted.GetComponent<Image>().sprite = selectedGoal;
                                break;
                        }
                    }
                }
            }
        }
    }
}
