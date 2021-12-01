using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> menus = new List<GameObject>();

    public Image fadeOutImage;
    public float transitionTimer = 0f;

    private bool loadMenu = false;
    private bool loadLevel = false;
    private int playersSelectedOption = 0;
    private string fadeState = "FadingIn";

    public TextMeshProUGUI turnCountDisplay;

    public SaveLoadMaps mapManager;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        //turnCountDisplay.text = "Turn Count: " + gameManager.turnCount.ToString();
        fadeOutImage.canvasRenderer.SetAlpha(1.0f);
        fadeOutImage.CrossFadeAlpha(0, 2, false);
        foreach (Transform child in transform)
        {
            if (child.gameObject != fadeOutImage.gameObject)
            {
                menus.Add(child.gameObject);
            }
            else
            {
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transitionTimer < 1f)
        {
            transitionTimer += Time.deltaTime;
        }
        else if (transitionTimer > 1f && fadeState == "FadingOut")
        {
            TransitionIn();
            if (loadMenu)
            {
                gameManager.playTest = false;
                LoadDifferentMenu();
                loadMenu = false;
            }
            if (loadLevel)
            {
                gameManager.playTest = true;
                mapManager.selectedMap = 1;
                mapManager.mapReadyToLoad = true;
                //if (mapManager.LoadLevelData(playersSelectedOption))
                //{
                //    playersSelectedOption = 3;
                //    LoadDifferentMenu();
                //    //turnCountDisplay.text = "Turn Count: " + gameManager.turnCount.ToString();
                //    loadLevel = false;
                //}
                loadLevel = false;
                for (int i = 0; i < menus.Count; i++)
                {
                    menus[i].SetActive(false);
                }
            }
        }
        else if (transitionTimer > 1f && fadeState == "FadingIn")
        {
            fadeOutImage.gameObject.SetActive(false);
            fadeState = "FadeState";
        }
    }

    public void LoadDifferentMenu()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (i == playersSelectedOption)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
    public void TransitionIn()
    {
        fadeState = "FadingIn";
        fadeOutImage.gameObject.SetActive(true);
        transitionTimer = 0f;
        fadeOutImage.canvasRenderer.SetAlpha(1.0f);
        fadeOutImage.CrossFadeAlpha(0, 1, false);
    }
    public void TransitionOutTolevel(int selectedOption)
    {
        TransitionOut(selectedOption);
        loadLevel = true;
    }
    public void TransitionOutToMenu(int selectedOption)
    {
        TransitionOut(selectedOption);
        loadMenu = true;
    }
    private void TransitionOut(int selectedOption)
    {
        fadeState = "FadingOut";
        fadeOutImage.gameObject.SetActive(true);
        playersSelectedOption = selectedOption;
        transitionTimer = 0f;
        fadeOutImage.canvasRenderer.SetAlpha(0.0f);
        fadeOutImage.CrossFadeAlpha(1, 1, false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
