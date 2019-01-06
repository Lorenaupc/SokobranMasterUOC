using SimpleFileBrowser;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void start10Levels()
    {
        SceneManager.LoadScene("Level");
    }

    public void startOwnLevel()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    public void createLevel()
    {
        SceneManager.LoadScene("Editor");
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        PlayerPrefs.SetString("Level",FileBrowser.Result);
        SceneManager.LoadScene("OwnLevel");
    }
}
