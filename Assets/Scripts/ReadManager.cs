using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadManager : MonoBehaviour
{
    public GameObject crate;
    public GameObject floor;
    public GameObject goal;
    public GameObject player;
    public GameObject wall;
    public GameObject separation;

    Transform board;
    static string[] level1 = File.ReadAllLines("Assets/Levels/Level1.txt");
    static string[] level2 = File.ReadAllLines("Assets/Levels/Level2.txt");
    static string[] level3 = File.ReadAllLines("Assets/Levels/Level3.txt");
    static string[] level4 = File.ReadAllLines("Assets/Levels/Level4.txt");
    static string[] level5 = File.ReadAllLines("Assets/Levels/Level5.txt");
    static string[] level6 = File.ReadAllLines("Assets/Levels/Level6.txt");
    static string[] level7 = File.ReadAllLines("Assets/Levels/Level7.txt");
    static string[] level8 = File.ReadAllLines("Assets/Levels/Level8.txt");
    static string[] level9 = File.ReadAllLines("Assets/Levels/Level9.txt");
    static string[] level10 = File.ReadAllLines("Assets/Levels/Level10.txt");

    public string[][] levels = { level1, level2, level3, level4, level5, level6, level7, level8, level9, level10 };

    public int SetupBoard(int levelIndex)
    {
        float scale = GameManager.scale;
        int goals = 0;

        if (board != null)
        {
            Destroy(board.gameObject);
        }

        board = new GameObject("Board").transform; 

        string[] level = levels[levelIndex];
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

                if (tile != floor && tile != separation && tile != wall)
                {
                    instance = Instantiate(floor, new Vector3(x / scale, (level.Length - y) / scale, 0), Quaternion.identity);
                    instance.transform.SetParent(board);
                    instance.tag = "Untagged";

                    if (tile == goal)
                    {
                        instance.tag = "Untagged";
                        goals += 1;
                    }
                }
            }
        }

        float halfTile = 1 / (scale);
        board.position = new Vector3(-(maxX / 2 - halfTile) / scale, -(maxY / 2 + halfTile) / scale, 0);

        return goals;
    }

    public int SetupBoard(string[] level)
    {
        float scale = GameManager.scale;
        int goals = 0;

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

                if (tile != floor && tile != separation && tile != wall)
                {
                    instance = Instantiate(floor, new Vector3(x / scale, (level.Length - y) / scale, 0), Quaternion.identity);
                    instance.transform.SetParent(board);
                    instance.tag = "Untagged";

                    if (tile == goal)
                    {
                        instance.tag = "Untagged";
                        goals += 1;
                    }
                }
            }
        }

        float halfTile = 1 / (scale);
        board.position = new Vector3(-(maxX / 2 - halfTile) / scale, -(maxY / 2 + halfTile) / scale, 0);

        return goals;
    }
}
