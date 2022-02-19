using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class ActionManager : TileManagerScript
{
    
    private bool actionSelected;
    public string water = "water";
    public string weed = "weed";
    public string fert = "fertilize";
    public string pest = "herb/pest";
    public string mow = "mow";

    [SerializeField] public int envPoints;
    [SerializeField] public int moneyPoints;

    public string currentTile;
    public string currentAction;
    public bool canUseAct;

    public TextMeshProUGUI text;


    // Start is called before the first frame update
    void Start()
    {
        actionSelected = false;
        envPoints = 0;
        moneyPoints = 0;
    }

    // Update is called once per frame
    void Update()
    {
     
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = tileMap.WorldToCell(mousePos);

                //code to select action here

                //currentAction = "temp";

                //code to perform action here
                if (actionSelected)
                {
                    currentTile = GetTileType(gridPos);
                    //Debug.Log("tile for action picked");

                    if (CompatibleAction(currentTile, currentAction))
                    {
                        Debug.Log(currentAction + "ed " + currentTile);

                        if(currentAction == "water")
                        {
                            envPoints += 50;
                            moneyPoints -= 10;
                        }
                        else if (currentAction == "herb/pest")
                        {
                            envPoints -= 100;
                            moneyPoints -= 30;
                        }
                        else if (currentAction == "weed")
                        {
                            envPoints += 30;
                        }
                        else if (currentAction == "fertilize")
                        {
                            envPoints -= 50;
                            moneyPoints -= 20;
                        }
                        else if (currentAction == "mow")
                        {
                            envPoints += 20;
                            moneyPoints -= 10;
                        }
                    }
                }


            }
    }

    public string GetTileType(Vector3Int gridPos)
    {
        if (tileMap.HasTile(gridPos))
        {
            string temp = tileMap.GetTile(gridPos).name;
            Debug.Log(temp);
            return temp;
        }

        return null;
    }

    public bool CompatibleAction(string tileName, string action)
    {
        canUseAct = true;

        //nothing can be done with dirt
        if (tileName == "Dirt")
        {
            canUseAct = false;
        }
        else if (tileName == "Moss")
        {
            if(action == "fertilize" || action == "weed" || action == "herb/pest")
            {
                canUseAct = false;
            }
        }
        else if (tileName == "Grass")
        {
            /*
            if(action == "herb/pest")
            {
                canUseAct = false;
            }
            */
        }
        else if (tileName == "Flower")
        {
            if(action == "mow")
            {
                canUseAct = false;
            }
        }
        else if (tileName == "Garden")
        {
            if(action == "mow")
            {
                canUseAct = false;
            }
        }
        else if (tileName == "Tree")
        {
            if(action == "mow" || action == "weed")
            {
                canUseAct = false;
            }
        }
        else if (tileName == "Wild_grass")
        {
            if(action == "herb/pest" || action == "mow" || action == "weed")
            {
                canUseAct = false;
            }
        }
        else
        {
            canUseAct = true;
        }

        text.text = "Acting: " + canUseAct;
        //Debug.Log("Acting: " + canUseAct);
        return canUseAct;
    }

    public void ChangeAction(string newAction)
    {
        currentAction = newAction;
        actionSelected = true;
        text.text = "Action changed to " + currentAction;
        Debug.Log("action changed to " + currentAction);
    }
}
