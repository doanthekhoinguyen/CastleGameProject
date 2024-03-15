using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Castle.CustomUtil;

public class ScenceLoader : MonoBehaviour
{
    [SerializeField] public GameObject optionMenu;
    [SerializeField] public GameObject BtnExit_OtionMenu;

    //slider
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;





    public void LoadScence()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void QuitGame()
    { 
        Application.Quit();
    }
    public void ShowOptionMenu()
    {
        optionMenu.SetActive(true);
    }
    public void HideOptionMenu()
    {
        optionMenu.SetActive(false);
    }

}
