using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;

    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
    }

    public void ResetLevel()
    {
        
        GameObject gm = GameObject.Find("GameManager(Clone)");
        gm.GetComponent<GameManager>().ResetLevel();
        
    }

    public void Back()
    {
        GameObject gm = GameObject.Find("GameManager(Clone)");
        Destroy(gm);

        SceneManager.LoadScene("Menu");
    }
}