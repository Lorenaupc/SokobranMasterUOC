using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static float scale = 2;
    public ReadManager boardScript;
    public int level;
    string pathLevel;

    int goals;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);

        if (SceneManager.GetActiveScene().name == "OwnLevel")
        {
            pathLevel = PlayerPrefs.GetString("Level");
        }
        else pathLevel = "";

        boardScript = GetComponent<ReadManager>();

        if (pathLevel == "")
        {
            goals = boardScript.SetupBoard(level);
        }
        else
        {
            string[] ownLevel = File.ReadAllLines(pathLevel);
            goals = boardScript.SetupBoard(ownLevel);
        }
    }

    public void ResetLevel()
    {
        Transform board = GameObject.Find("Board").transform;
        Destroy(board.gameObject);
        boardScript = GetComponent<ReadManager>();
        //goals = boardScript.SetupBoard(level);
        if (pathLevel == "")
        {
            goals = boardScript.SetupBoard(level);
        }
        else
        {
            string[] ownLevel = File.ReadAllLines(pathLevel);
            goals = boardScript.SetupBoard(ownLevel);
        }
    }

    public void CheckWin()
    {
        int currentGoals = 0;
        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
        foreach (GameObject crate in crates)
        {
            if (crate.GetComponent<CrateController>().onGoal)
            {
                currentGoals += 1;
            }
        }

        if (currentGoals == goals)
        {
            if (pathLevel == "")
            {
                if (level != GetComponent<ReadManager>().levels.Length - 1)
                {
                    level++;
                }
                boardScript = GetComponent<ReadManager>();
                goals = boardScript.SetupBoard(level);
            }
            else
            {
                ResetLevel();
            }
        }
    }
}