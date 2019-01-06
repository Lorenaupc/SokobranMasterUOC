using UnityEngine;
using SimpleFileBrowser;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSaveLevel : MonoBehaviour
{
    public GameObject crate;
    public GameObject floor;
    public GameObject goal;
    public GameObject player;
    public GameObject wall;
    public GameObject separation;

    public Text messageText;

    internal Transform board;

    string[] level;
    string path;

    public void Load()
    {
        GetComponent<CreateTiles>().disableSelectedTile();
        StartCoroutine(ShowLoadDialogCoroutine());
        messageText.text = "";
    }

    private bool comprovarGoals()
    {
        GameObject[] allCrates = GameObject.FindGameObjectsWithTag("Crate");
        GameObject[] allGoals = GameObject.FindGameObjectsWithTag("Goal");

        if (allCrates.Length == allGoals.Length || (allCrates.Length == 0 || allGoals.Length == 0))
        {
            return true;
        }

        else return false;
    }

    private bool comprovarPlayer()
    {
        int i = GameObject.FindGameObjectsWithTag("Player").Length - 1;

        if (i == 1)
        {
            return true;
        }

        else return false;
    }

    public void Save()
    {
        GetComponent<CreateTiles>().disableSelectedTile();
        if (!comprovarGoals())
        {
            messageText.text = ("Deben haber el mismo número de Metas que de Cajas");
        }
        else if (!comprovarPlayer())
        {
            messageText.text = ("Debe existir un jugador en el nivel");
        }
        else
        {
            StartCoroutine(ShowSaveDialogCoroutine());
            messageText.text = "";
        }
    }

    IEnumerator ShowSaveDialogCoroutine()
    {
        yield return FileBrowser.WaitForSaveDialog(false, null, "Save File", "Save");
        path = FileBrowser.Result;
        SaveBoard();
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        level = File.ReadAllLines(FileBrowser.Result);
        LoadBoard();
    }

    private void SaveBoard()
    {
        board = GameObject.Find("Board").transform;
        Transform[] boardChilds = board.GetComponentsInChildren<Transform>();

        StreamWriter writer = new StreamWriter(path, false);

        foreach (Transform child in boardChilds)
        {
            string sign = null;
            switch (child.tag)
            {
                case ("Wall"):
                    sign = "#";
                    break;
                case ("Crate"):
                    sign = "o";
                    break;
                case ("Player"):
                    sign = "@";
                    break;
                case ("Empty"):
                    sign = ".";
                    break;
                case ("Goal"):
                    sign = "*";
                    break;
                case ("Separation"):
                    sign = ",";
                    break;
            }
            if (sign != null)
            {
                if (sign == ",")
                {
                    writer.Write(sign);
                    writer.WriteLine();
                }
                else writer.Write(sign);
            }
        }
        writer.Close();
    }

    private void LoadBoard()
    {
        float scale = GameManager.scale;

        if (board != null)
        {
            Destroy(board.gameObject);
        }

        board = new GameObject("Board").transform;
        float maxY = level.Length;
        float maxX = 0;

        for (int y = 0; y < level.Length; y++)
        {
            string row = level[y];
            maxX = Mathf.Max(row.Length, maxX);
            for (int x = 0; x < row.Length; x++)
            {
                GameObject tile = null;
                switch (row[x])
                {
                    case '#':
                        tile = wall;
                        break;
                    case '@':
                        tile = player;
                        break;
                    case 'o':
                        tile = crate;
                        break;
                    case '*':
                        tile = goal;
                        break;
                    case '.':
                        tile = floor;
                        break;
                    case ',':
                        tile = separation;
                        break;
                }

                GameObject instance = Instantiate(tile, new Vector3(x / scale, (level.Length - y) / scale, 0), Quaternion.identity);
                instance.transform.SetParent(board);

                if (tile == player)
                {
                    GetComponent<CreateTiles>().boardPlayer = instance;
                }
            }
        }

        float halfTile = 1 / (scale);
        board.position = new Vector3(-(maxX / 2 - halfTile) / scale, -(maxY / 2 + halfTile) / scale, 0);
        GetComponent<CreateTiles>().board = board;
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
