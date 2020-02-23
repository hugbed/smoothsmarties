using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public UIController ui;

    private int turn = 0;
    public int numberOfTurns = 10;

    private int prefabIndex = 0;
    public List<GameObject> boardPrefabs = new List<GameObject>();

    private GameObject boardInstance = null;

    // Start is called before the first frame update

    public void newGame()
    {
        if (boardInstance != null)
        {
            Destroy(boardInstance);
        }

        WeatheredTile.ClearWeatherEffect();

        boardInstance = Instantiate(boardPrefabs[prefabIndex]);
        prefabIndex = (prefabIndex + 1) % boardPrefabs.Count;

        turn = -1;

        ui.setTurnNumberText(0, numberOfTurns);

        ui.showGameEnd(false);
        ui.showMenu(false);
        ui.showGameUI(true);

        ui.showNextTurnButton(false);
        ui.showBeginButton(true);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void nextTurn()
    {
        WeatheredTile.ClearWeatherEffect();
        turn++;

        var weather = boardInstance.GetComponentInChildren<WeatherControl>(true);
        if (turn == 0)
        {
            weather.StartForecasting();

            // Hide numbers
            {
                var go = GameObject.Find("NumberTilemap");
                go.SetActive(false);
            }
            //// Show Burn
            {
                var go = GameObject.Find("BuildingsTilemap");
                go.GetComponent<Tilemap>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

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
            weather.Step();
        }
    }

    public void returnToMenu()
    {
        ui.showGameEnd(false);
        ui.showMenu(true);
        ui.showGameUI(false);

        if (boardInstance != null)
        {
            Destroy(boardInstance);
        }
    }

    public void beginGamePhase()
    {
        nextTurn();
        ui.showNextTurnButton(true);
        ui.showBeginButton(false);
    }
}
