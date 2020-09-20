using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDot : MonoBehaviour
{
    private bool isSelected;
    public int index;
    public Tile[] controlledTiles;

    public void ActivateTiles() 
    {
        gameObject.SetActive(true);
        controlledTiles[0].gameObject.SetActive(true);
        controlledTiles[1].gameObject.SetActive(true);
        controlledTiles[2].gameObject.SetActive(true);
    }
    public void DeactivateTiles()
    {
        gameObject.SetActive(false);
        controlledTiles[0].gameObject.SetActive(false);
        controlledTiles[1].gameObject.SetActive(false);
        controlledTiles[2].gameObject.SetActive(false);
    }
    //Check tiles at the beginning of the game to start a game with no tiles that will be destroyed.
    public void CheckTilesBegin()
    {
        if (controlledTiles[0].tileType == controlledTiles[1].tileType && controlledTiles[0].tileType == controlledTiles[2].tileType) 
        {
            ControlDotManager.instance.isChanged = true;
            controlledTiles[0].ClearItself();
            controlledTiles[0].CreateHexElement();
            controlledTiles[1].ClearItself();
            controlledTiles[1].CreateHexElement();
            controlledTiles[2].ClearItself();
            controlledTiles[2].CreateHexElement();
        }
    }
    //Check if 3 controlled tiles have the same type.
    public void CheckTiles() 
    {
        if (controlledTiles[0].tileType == controlledTiles[1].tileType && controlledTiles[0].tileType == controlledTiles[2].tileType)
        {
            TileManager.instance.pointGained = true;
            TileManager.instance.destroyTiles.Add(controlledTiles[0]);
            TileManager.instance.destroyTiles.Add(controlledTiles[1]);
            TileManager.instance.destroyTiles.Add(controlledTiles[2]);
        }

    }
    //Check tiles for the ending check
    public void CheckTilesEnd()
    {
        if (controlledTiles[0].tileType == controlledTiles[1].tileType && controlledTiles[0].tileType == controlledTiles[2].tileType)
        {
            TileManager.instance.pointGained = true;
        }

    }
    //Check if any possible move exists for this control dot
    public void CheckPossibleMove() 
    {
        for (int i = 0; i < 3; i++)
        {
            int type0 = controlledTiles[0].tileType;
            bool type0bomb = controlledTiles[0].isBomb;
            int type0Countdown = controlledTiles[0].bombCountdown;
            controlledTiles[0].ChangeHexElement(controlledTiles[1].tileType, controlledTiles[1].isBomb, controlledTiles[1].bombCountdown);
            controlledTiles[1].ChangeHexElement(controlledTiles[2].tileType, controlledTiles[2].isBomb, controlledTiles[2].bombCountdown);
            controlledTiles[2].ChangeHexElement(type0, type0bomb, type0Countdown);
            TileManager.instance.CheckControlTilesEqualEnding();
        }

    }
    //Turns the hexagons in the tiles counter clockwise.
    public IEnumerator TurnCounterClockWiseCoroutine() 
    {
        bool tilesDestroyed;
        tilesDestroyed = false;
        for (int i = 0; i < 3; i++) 
        {
            int type0 = controlledTiles[0].tileType;
            bool type0bomb = controlledTiles[0].isBomb;
            int type0Countdown = controlledTiles[0].bombCountdown;
            controlledTiles[0].ChangeHexElement(controlledTiles[1].tileType, controlledTiles[1].isBomb, controlledTiles[1].bombCountdown);
            controlledTiles[1].ChangeHexElement(controlledTiles[2].tileType, controlledTiles[2].isBomb, controlledTiles[2].bombCountdown);
            controlledTiles[2].ChangeHexElement(type0, type0bomb, type0Countdown);
            TileManager.instance.CheckControlTilesEqual();
            if (TileManager.instance.pointGained == true)
            {
                tilesDestroyed = true;
                TileManager.instance.DestroyEqualTiles();
                TileManager.instance.pointGained = false;
                break;
            }
            yield return new WaitForSeconds(ControlDotManager.instance.waitBetweenTurns);
        }
        if (!tilesDestroyed) 
        {
            GameManager.instance.gameState = GameState.CanAct;
        }
    }
    //Turns the hexagons in the tiles clockwise.
    public IEnumerator TurnClockWiseCoroutine()
    {
        bool tilesDestroyed;
        tilesDestroyed = false;
        for (int i = 0; i < 3; i++)
        {
            int type0 = controlledTiles[0].tileType;
            bool type0bomb = controlledTiles[0].isBomb;
            int type0Countdown = controlledTiles[0].bombCountdown;
            controlledTiles[0].ChangeHexElement(controlledTiles[2].tileType, controlledTiles[2].isBomb, controlledTiles[2].bombCountdown);
            controlledTiles[2].ChangeHexElement(controlledTiles[1].tileType, controlledTiles[1].isBomb, controlledTiles[1].bombCountdown);
            controlledTiles[1].ChangeHexElement(type0, type0bomb, type0Countdown);
            TileManager.instance.CheckControlTilesEqual();
            if (TileManager.instance.pointGained == true)
            {
                tilesDestroyed = true;
                TileManager.instance.DestroyEqualTiles();
                TileManager.instance.pointGained = false;
                break;
            }
            yield return new WaitForSeconds(ControlDotManager.instance.waitBetweenTurns);
        }
        if (!tilesDestroyed)
        {
            GameManager.instance.gameState = GameState.CanAct;
        }
    }
    public void TurnCounterClockWise() 
    {
        StartCoroutine("TurnCounterClockWiseCoroutine");
    }
    public void TurnClockWise()
    {
        StartCoroutine("TurnClockWiseCoroutine");
    }

}
