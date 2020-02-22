using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject gameUI;
    [SerializeField]
    private GameObject gameEnd;

    public void showMenu(bool show)
    {
        // Can replace for anything else (like animation or something)
        menu.SetActive(show);
    }
    public void showGameUI(bool show)
    {
        gameUI.SetActive(show);
    }

    public void showGameEnd(bool show)
    {
        gameEnd.SetActive(show);
    }
}
