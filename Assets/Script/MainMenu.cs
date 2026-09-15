using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Player");
        SceneManager.LoadSceneAsync("Prolog");
    }

    public void Option()
    {
        // ¾ÀÀ» ´ÝÁö¾Ê°í À§¿¡ ¿­±â
        SceneManager.LoadSceneAsync("OptionScene", LoadSceneMode.Additive);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
