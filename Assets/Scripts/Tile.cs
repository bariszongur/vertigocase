using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tile : MonoBehaviour
{
    public int tileType ;
    public HexElement hexElement;
    public bool isBomb;
    public int bombCountdown;
    public Canvas bombCountdownCanvas;
    public Text bombCoundownText;
    //CreateHexElement Creates a random Hexagon into the Tile
    public void CreateHexElement()
    {
        tileType = Random.Range(0, TileManager.instance.baseHexElements.Length);
        hexElement = Instantiate(TileManager.instance.baseHexElements[tileType], transform.position, transform.rotation);
        hexElement.transform.localScale = TileManager.instance.scaleAmount * TileManager.instance.tileHexScale;
    }
    //ChangeHexElement Changes the current Hexagon into another Hexagon. This is generally used in swaps of tiles.
    public void ChangeHexElement(int type, bool isThisBomb, int bombCountdown) 
    {

        if (tileType != -1) 
        {
            ClearItself();
        }
        if (isThisBomb) 
        {
            isBomb = true;
            this.bombCountdown = bombCountdown;
            bombCountdownCanvas = Instantiate(TileManager.instance.bombCanvas, Camera.main.WorldToScreenPoint(transform.position), transform.rotation);
            bombCoundownText = bombCountdownCanvas.GetComponentInChildren<Text>();
            bombCoundownText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            bombCoundownText.text = bombCountdown.ToString();
        }
        tileType = type;
        hexElement = Instantiate(TileManager.instance.baseHexElements[tileType], transform.position, transform.rotation);
        hexElement.transform.localScale = TileManager.instance.scaleAmount * TileManager.instance.tileHexScale;
    }
    //Clears itself. Used when Hexagons are destroyed.
    public void ClearItself() 
    {
        if (isBomb) 
        {
            isBomb = false;
            Destroy(bombCoundownText.gameObject);
            Destroy(bombCountdownCanvas.gameObject);
            bombCountdown = 0;
        }

        tileType = -1;
        Destroy(hexElement.gameObject);
    }
}
