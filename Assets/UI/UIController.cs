using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject menu;
    public GameObject gameUI;
    public GameObject gameEnd;

    public TMPro.TextMeshProUGUI turnNumberText;

    public GameObject beginButton;

    public GameObject nextTurnButton;

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

    public void setTurnNumberText(int turn, int totalTurns)
    {
        turnNumberText.text = (turn + 1).ToString() + "/" + (totalTurns).ToString();
    }

    public void showBeginButton(bool show)
    {
        beginButton.SetActive(show);
    }

    public void showNextTurnButton(bool show)
    {
        nextTurnButton.SetActive(show);
    }
}
