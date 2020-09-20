using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDotManager : MonoBehaviour
{
    public static ControlDotManager instance;
    [HideInInspector]public float leftSceneX;
    [HideInInspector]public float upSceneY;
    [HideInInspector]public float dotGapUp;
    [HideInInspector]public float dotGapSide;
    [HideInInspector]public float yUpperGap;
    [HideInInspector]public bool isChanged;
    public int dimN;
    public int dimM;
    public float upperGapAmount;
    public float waitBetweenTurns;
    public ControlDot[,] controlDots;
    public ControlDot baseDot;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    //"LoadSceneVariables" calculates the distances between dots and tiles based on the screen lengths and N and M variables;
    public void LoadSceneVariables() 
    {
        leftSceneX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        upSceneY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        yUpperGap = Mathf.Abs(Camera.main.ViewportToWorldPoint(new Vector3(0, 1-upperGapAmount, 0)).y - upSceneY);

        int gapAmount;
        if (dimN % 2 == 0)
        {
            gapAmount = dimN * 3 / 2;
        }
        else 
        {
            gapAmount = (dimN - 1) * 3 / 2 + 2;
        }
        gapAmount = gapAmount +1;
        dotGapSide = Mathf.Abs(leftSceneX - Camera.main.ViewportToWorldPoint(new Vector3(1f / gapAmount, 0, 0)).x);
        dotGapUp = dotGapSide / 2 * Mathf.Sqrt(3);
    }

    /*"LoadControlDots" Instantiates the control points of tiles based on the calculated values from "LoadSceneVariables".
     Loads the control dots and assigns the tiles that will be controled by the dot. It is known that which dot will control which tiles
      at the beginning of the game.*/
    public void LoadControlDots() 
    {
        controlDots = new ControlDot[dimN-1,(dimM-1)*2];
        for (int j = 0; j < (dimM - 1) * 2 ; j++)
        {
            if (j % 2 == 0)
            {
                float lastGap = 1.5f * dotGapSide;
                for (int i = 0; i < dimN-1; i++)
                {
                    controlDots[i, j] = Instantiate(baseDot, new Vector2(leftSceneX + lastGap, upSceneY - yUpperGap - (j + 2) * dotGapUp), Quaternion.identity);
                    if (i % 2 == 0)
                    {
                        lastGap = lastGap + 2 * dotGapSide;
                        controlDots[i, j].controlledTiles = new Tile[3];
                        controlDots[i, j].controlledTiles[0] = TileManager.instance.tiles[i, j/2];
                        controlDots[i, j].controlledTiles[1] = TileManager.instance.tiles[i+1, j/2];
                        controlDots[i, j].controlledTiles[2] = TileManager.instance.tiles[i, j/2 + 1];
                    }
                    else
                    {
                        lastGap = lastGap + dotGapSide;
                        controlDots[i, j].controlledTiles = new Tile[3];
                        controlDots[i, j].controlledTiles[0] = TileManager.instance.tiles[i, j/2];
                        controlDots[i, j].controlledTiles[1] = TileManager.instance.tiles[i + 1, j/2];
                        controlDots[i, j].controlledTiles[2] = TileManager.instance.tiles[i+1, j/2 + 1];
                    }
                    controlDots[i, j].gameObject.SetActive(false);
                }
            }
            else
            {
                float lastGap = 2 * dotGapSide;
                for (int i = 0; i < dimN-1; i++)
                {
                    controlDots[i, j] = Instantiate(baseDot, new Vector2(leftSceneX + lastGap, upSceneY - yUpperGap - (j + 2) * dotGapUp), Quaternion.identity);
                    if (i % 2 == 0)
                    {
                        lastGap = lastGap + dotGapSide;
                        controlDots[i, j].controlledTiles = new Tile[3];
                        controlDots[i, j].controlledTiles[0] = TileManager.instance.tiles[i+1, (j-1)/2];
                        controlDots[i, j].controlledTiles[2] = TileManager.instance.tiles[i , (j - 1) / 2 +1];
                        controlDots[i, j].controlledTiles[1] = TileManager.instance.tiles[i +1 , (j - 1) / 2 +1];
                    }
                    else
                    {
                        lastGap = lastGap + 2 * dotGapSide;
                        controlDots[i, j].controlledTiles = new Tile[3];
                        controlDots[i, j].controlledTiles[0] = TileManager.instance.tiles[i, (j - 1) / 2];
                        controlDots[i, j].controlledTiles[2] = TileManager.instance.tiles[i , (j - 1) / 2 + 1];
                        controlDots[i, j].controlledTiles[1] = TileManager.instance.tiles[i+1, (j - 1) / 2 + 1];
                    }
                    controlDots[i, j].gameObject.SetActive(false);
                }
            }
        }
    }
    public void CheckControlDots() 
    {
        isChanged = true;

        while (isChanged) 
        {
            isChanged = false;
            for (int j = 0; j < (dimM - 1) * 2; j++)
            {
                for (int i = 0; i < dimN - 1; i++)
                {
                    controlDots[i, j].CheckTilesBegin();
                }
            }

        }
    }
    //Clears all the control dots
    public void ClearAllDots()
    {
        for (int j = 0; j < (dimM - 1) * 2; j++)
        {
            for (int i = 0; i < dimN - 1; i++)
            {
                Destroy(controlDots[i, j].gameObject);
            }
        }

    }
    //Checks all possible moves for all control dots
    public void CheckAllPossibleMove()
    {
        for (int j = 0; j < (dimM - 1) * 2; j++)
        {
            for (int i = 0; i < dimN - 1; i++)
            {
                controlDots[i, j].CheckPossibleMove();
            }
        }

    }
}
