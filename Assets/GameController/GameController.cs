using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private UIController ui;

    private int turn = 0;

    [SerializeField]
    private int numberOfTurns = 10;

    [SerializeField]
    private GameObject boardPrefab;

    private GameObject boardInstance;

    // Start is called before the first frame update

    public void newGame()
    {
        if (boardInstance != null)
        {
            GameObject.Destroy(boardInstance);
        }
        boardInstance = GameObject.Instantiate(boardPrefab);
        
        turn = -1;

        nextTurn();

        ui.showGameEnd(false);
        ui.showMenu(false);
        ui.showGameUI(true);
    }

    public void nextTurn()
    {
        turn++;

        if (turn >= 0 && turn < numberOfTurns)
        {
            ui.setTurnNumberText(turn, numberOfTurns);
        }

        if (turn >= numberOfTurns)
        {
            ui.showGameEnd(true);
            ui.showMenu(false);
            ui.showGameUI(false);
        }
        else
        {
            var weather = boardInstance.GetComponentInChildren<WeatherControl>(true);
            if (!weather.initialized)
            {
                weather.initialize();
            }
            weather.AdvanceTurn();
        }
    }

    public void returnToMenu()
    {
        ui.showGameEnd(false);
        ui.showMenu(true);
        ui.showGameUI(false);

        if (boardInstance != null)
        {
            GameObject.Destroy(boardInstance);
        }
    }
}
