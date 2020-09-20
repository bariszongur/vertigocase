
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    public int points;
    public int spawnedBombs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    //Initial load of the game.
    public void Init()
    {
        gameState = GameState.InAction;
        UIManager.instance.OnStartUI();
        ControlDotManager.instance.LoadSceneVariables();
        TileManager.instance.LoadTiles();
        ControlDotManager.instance.LoadControlDots();
        ControlDotManager.instance.CheckControlDots();
        gameState = GameState.CanAct;
    }
    //Add points to the player
    public void AddPoints() 
    {
        points += 5;
        UIManager.instance.scoreText.text = "Score: " + points.ToString();
    }
    //Checks if any bombs are detonated, lowers the countdown of the bombs.
    public void CheckBombs() 
    {
        for (int j = 0; j < ControlDotManager.instance.dimM; j++)
        {
            for (int i = 0; i < ControlDotManager.instance.dimN; i++)
            {
                if (TileManager.instance.tiles[i, j].isBomb) 
                {
                    if (TileManager.instance.tiles[i, j].bombCountdown <= 1)
                    {
                        gameState = GameState.IsEnded;
                        UIManager.instance.ActivateEndGameUI();
                    }
                    else 
                    {
                        TileManager.instance.tiles[i, j].bombCountdown--;
                        TileManager.instance.tiles[i, j].bombCoundownText.text = TileManager.instance.tiles[i, j].bombCountdown.ToString();
                    }
                }
            }
        }
        if (gameState != GameState.IsEnded) 
        {
            gameState = GameState.CanAct;
        }
        CheckIfEnded();
    }
    //Restarts the game
    public void RestartGame() 
    {
        points = 0;
        spawnedBombs = 0;
        ControlDotManager.instance.ClearAllDots();
        TileManager.instance.ClearAllTiles();
        Init();
    }
    //Check if any possible moves left
    public void CheckIfEnded() 
    {
        ControlDotManager.instance.CheckAllPossibleMove();
        if (!TileManager.instance.pointGained)
        {
            gameState = GameState.IsEnded;
            UIManager.instance.ActivateEndGameUI();
        }
        TileManager.instance.pointGained = false;
    }
}
public enum GameState {InAction, CanAct, IsEnded };
