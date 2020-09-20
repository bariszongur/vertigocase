using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    public Tile[,] tiles;
    public Tile baseTile;
    public SpriteRenderer rendererSprite;
    public HexElement[] baseHexElements;
    public float waitBetweenFall;
    public int bombCountdowns;
    [HideInInspector]public Vector2 scaleAmount;
    public float tileHexScale;
    [HideInInspector]public List<Tile> destroyTiles;
    public bool pointGained;
    public Canvas bombCanvas;


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
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    /*Loads the tiles of the grid. After Instantiating the each tile of the grid, a random colored hexagon will be Instantiated
     aswell with the function CreateHexElement inside the Tile.*/
    public void LoadTiles()
    {
        float initX = ControlDotManager.instance.leftSceneX + ControlDotManager.instance.dotGapSide;
        float initY = ControlDotManager.instance.upSceneY - ControlDotManager.instance.yUpperGap - ControlDotManager.instance.dotGapUp;
        tiles = new Tile[ControlDotManager.instance.dimN, ControlDotManager.instance.dimM];
        scaleAmount = new Vector2(ControlDotManager.instance.dotGapSide / rendererSprite.bounds.extents.y, ControlDotManager.instance.dotGapSide / rendererSprite.bounds.extents.y);
        for (int j = 0; j < ControlDotManager.instance.dimM; j++)
        {
            
            for (int i = 0; i < ControlDotManager.instance.dimN; i++)
            {
                if (i % 2 == 0)
                {
                    tiles[i, j] = Instantiate(baseTile, new Vector2(initX + i * ControlDotManager.instance.dotGapSide*1.5f, initY - j * ControlDotManager.instance.dotGapUp * 2), baseTile.transform.rotation);
                    tiles[i, j].transform.localScale = scaleAmount;
                    tiles[i, j].gameObject.SetActive(false);

                }
                else 
                {
                    tiles[i, j] = Instantiate(baseTile, new Vector2(initX + i * ControlDotManager.instance.dotGapSide*1.5f, initY - (j) * ControlDotManager.instance.dotGapUp * 2 - ControlDotManager.instance.dotGapUp), baseTile.transform.rotation);
                    tiles[i, j].transform.localScale = scaleAmount;
                    tiles[i, j].gameObject.SetActive(false);
                }
                tiles[i, j].CreateHexElement();
            }
        }
    }
    /*Checks if any control dot's Tiles has the same type with CheckTiles in the ControlDot objects. Each ControlDot controls 3 Tiles. If these tiles' types
     are equal, colored hexagons will be destroyed*/
    public void CheckControlTilesEqual()
    {
        pointGained = false;
        destroyTiles = new List<Tile>();
        for (int j = 0; j < (ControlDotManager.instance.dimM - 1) * 2; j++)
        {
            for (int i = 0; i < ControlDotManager.instance.dimN - 1; i++)
            {
                ControlDotManager.instance.controlDots[i, j].CheckTiles();
            }
        }
    }
   
    public void CheckControlTilesEqualEnding()
    {
        for (int j = 0; j < (ControlDotManager.instance.dimM - 1) * 2; j++)
        {
            for (int i = 0; i < ControlDotManager.instance.dimN - 1; i++)
            {
                ControlDotManager.instance.controlDots[i, j].CheckTilesEnd();
            }
        }
    }
    /*Destroys the tiles that should be destroyed*/
    public void DestroyEqualTiles()
    {
        for (int i = 0; i < destroyTiles.Count; i++)
        {
            if (destroyTiles[i].tileType != -1)
            {
                destroyTiles[i].ClearItself();
                GameManager.instance.AddPoints();
            }
        }
        StartCoroutine("FallDownTiles");
    }
    /*After Destroying the colored hexogons in the tiles of the grid, upper hexagons falls down to blanked spaces. This method recursively falls down the tiles
     and checks if any more tile is destroyed to make combos available*/
    public IEnumerator FallDownTiles() 
    {
        yield return new WaitForSeconds(0f);
        for (int j = ControlDotManager.instance.dimM-1; j >= 0; j--)
        {
            bool isFell;
            isFell = false;
            for (int i = 0; i < ControlDotManager.instance.dimN; i++)
            {
                if (tiles[i, j].tileType == -1) 
                {
                    isFell = true;
                    bool isSwapped;
                    isSwapped = false;
                    int dim2;
                    dim2 = j-1;
                    while (!isSwapped && dim2 >= 0) 
                    {
                        if (tiles[i, dim2].tileType != -1) 
                        {
                            tiles[i, j].ChangeHexElement(tiles[i, dim2].tileType, tiles[i,dim2].isBomb, tiles[i,dim2].bombCountdown);
                            tiles[i, dim2].ClearItself();
                            isSwapped = true;
                        }
                        dim2--;
                    }
                    if (!isSwapped)
                    {
                        tiles[i, j].CreateHexElement();
                        if ((GameManager.instance.spawnedBombs+1) * 1000 < GameManager.instance.points)
                        {
                            GameManager.instance.spawnedBombs++;
                            tiles[i, j].ChangeHexElement(tiles[i, j].tileType, true, bombCountdowns);
                            
                            Debug.Log("BombSpawned");
                        }
                    }
                }
            }
            if (isFell) {
                yield return new WaitForSeconds(waitBetweenFall);
            }
        }
        CheckControlTilesEqual();
        if (pointGained)
        {
            pointGained = false;
            DestroyEqualTiles();
        }
        else 
        {
            GameManager.instance.CheckBombs();
        }
    }
    public void ClearAllTiles()
    {
        for (int j = 0; j < ControlDotManager.instance.dimM; j++)
        {

            for (int i = 0; i < ControlDotManager.instance.dimN; i++)
            {
                tiles[i, j].ClearItself();
                Destroy(tiles[i, j].gameObject);
            }
        }

    }

}
