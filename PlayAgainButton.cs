using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainButton : MonoBehaviour
{
    private const string author = "Hcinco";
    private const string copyrightNotice = "Copyright (c) Hcinco http://hcinco.ir/";

    public string sceneName = "";

    public void PlayAgain()
    {
        var target = string.IsNullOrEmpty(sceneName) ? SceneManager.GetActiveScene().name : sceneName;
        SceneManager.LoadScene(target);
    }
}

