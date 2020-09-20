using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public ControlDot selectedDot;
    private bool isSelectedClicked;
    public float swipeThreshold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Swipe();
    }
    //Called in every update. Simple click and Swipe detection for the game.
    public void Swipe()
    {
        float minDist;
        int minX;
        int minY;
        minX = -1;
        minY = -1;
        minDist = 99999;
        /*At every click, nearest control point to the clicked point is selected. If the selected control point is already selected, then swipe functions to rotate
        the tiles can be performed.*/
        if (Input.GetMouseButtonDown(0) && GameManager.instance.gameState == GameState.CanAct)
        {
            Vector3 clickedPosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //save began touch 2d point
            Vector2 clickedPos = new Vector2(clickedPosRaw.x, clickedPosRaw.y);
            for (int j = 0; j < (ControlDotManager.instance.dimM - 1) * 2 ; j++)
            {
                for (int i = 0; i < ControlDotManager.instance.dimN - 1; i++) 
                {
                    Vector2 dotPos = new Vector2(ControlDotManager.instance.controlDots[i, j].transform.position.x, ControlDotManager.instance.controlDots[i, j].transform.position.y);
                    if (Vector2.Distance(clickedPos, dotPos ) < minDist)
                    {
                        minDist = Vector3.Distance(clickedPos, dotPos);
                        minX = i;
                        minY = j;
                    }
                }
            }
            if (selectedDot != null)
            {
                if (selectedDot.transform.position.x == ControlDotManager.instance.controlDots[minX, minY].transform.position.x && selectedDot.transform.position.y == ControlDotManager.instance.controlDots[minX, minY].transform.position.y)
                {
                    isSelectedClicked = true;
                }
                else
                {
                    selectedDot.DeactivateTiles();
                    selectedDot = ControlDotManager.instance.controlDots[minX, minY];
                    selectedDot.ActivateTiles();
                }
            }
            else 
            {
                selectedDot = ControlDotManager.instance.controlDots[minX, minY];
                selectedDot.ActivateTiles();
            }
            

        }
        /*Swiping up will turn the tiles counterclockwise, swiping down will turn clockwise.*/
        if (Input.GetMouseButtonUp(0) && GameManager.instance.gameState == GameState.CanAct)
        {
            if (isSelectedClicked)
            {
                Vector3 clickedPosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (clickedPosRaw.y - selectedDot.transform.position.y > swipeThreshold)
                {
                    GameManager.instance.gameState = GameState.InAction;
                    selectedDot.TurnCounterClockWise();
                    
                }
                else if (clickedPosRaw.y - selectedDot.transform.position.y < swipeThreshold*-1) 
                {
                    GameManager.instance.gameState = GameState.InAction;
                    selectedDot.TurnClockWise();
                }
                isSelectedClicked = false;
            }
        }
    }
}
