using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class ActionManagerScript : TileManagerScript
{
    public TileInfoScript tileInfo;
    
    private string water = "water";
    private string weed = "weed";
    private string fert = "fertilize";
    private string pest = "herb/pest";
    private string mow = "mow";

    [SerializeField] public int envPoints;
    [SerializeField] public int appealPoints;
    [SerializeField] public int pollutionPoints;
    [SerializeField] public int waterPoints;

    public string currentTile;
    public string currentAction;
    public string lastAction;
    public bool canUseAct;

    public TextMeshProUGUI text;
    public FeedbackManager feedback;
    public TextMeshProUGUI instructions;
    

    private bool firstClick = true;
    private bool end = false;
    public TextMeshProUGUI endingText;
    public GameObject EndUI;


    public GameObject pause;
    public bool actionSelected;
    public int actionNum = 0;

    private float highest;
    private float second;

   private bool bad = false;
   private bool neutral = false;

    //for anything at the end that should only run once
    public bool finalCheck = true;

    public GameObject scoreIcons;

    //private string highestAction;
    //private string secondAction;

    // Start is called before the first frame update
    void Start()
    {
        actionSelected = false;

        envPoints = 0;
        appealPoints = 0;
        pollutionPoints = 0;
        waterPoints = 0;

        EndUI.SetActive(false);

        pause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (actionNum >= 14)
        {
            dayUI.SetActive(false);
            feedbackUI.SetActive(false);

            if (!end)
            {
                EndUI.SetActive(true);
            }

            //Deciding Type of Ending
            Ending();



            if(finalCheck == true)
            {
                //get tile numbers from tileInfo
                tileInfo.GetTileTypes();

                //add/subtract appeal based on results
                appealPoints += tileInfo.pathTiles * 5;
                appealPoints += tileInfo.flowerTiles * 15;
                appealPoints -= tileInfo.dirtTiles * 10;

                //grass appeal mess
                if(tileInfo.grassGrassLevel == 0 && tileInfo.grassBugLevel ==0 && tileInfo.grassWaterLevel ==5 && tileInfo.grassBugLevel == 0)
                {
                    appealPoints += tileInfo.grassTiles * 15;
                }

                //add/subtract env based on results
                envPoints += tileInfo.wildGrassTiles * 10;
                envPoints += tileInfo.mossTiles * 5;
                envPoints -= tileInfo.grassTiles * 5;

                //subtract based on water levels
                if(tileInfo.grassWaterLevel <= 0)
                {
                    appealPoints -= tileInfo.grassWaterLevel * 5;
                }
                if (tileInfo.flowerWaterLevel <= 0)
                {
                    appealPoints -= tileInfo.flowerWaterLevel * 5;
                }
                if (tileInfo.mossWaterLevel <= 0)
                {
                    appealPoints -= tileInfo.mossWaterLevel * 5;
                }
                if (tileInfo.wildgrassWaterLevel <= 0)
                {
                    appealPoints -= tileInfo.wildgrassWaterLevel * 5;
                }

                //subtract based on weed levels
                if (tileInfo.grassWeedLevel >= 3)
                {
                    envPoints += 5;
                    
                    if(tileInfo.grassWeedLevel > 5)
                    {
                        appealPoints -= 5;
                    }
                }
                if (tileInfo.flowerWeedLevel >= 3)
                {
                    envPoints += 5;

                    if (tileInfo.flowerWeedLevel > 5)
                    {
                        appealPoints -= 10;
                    }
                }
                if (tileInfo.wildgrassWeedLevel >= 3)
                {
                    envPoints += 5;
                }

                //subtract based on bug level
                if (tileInfo.grassBugLevel >= 3)
                {
                    envPoints += 5;

                    if (tileInfo.grassBugLevel > 5)
                    {
                        appealPoints -= 5;
                    }
                }
                if (tileInfo.flowerBugLevel >= 3)
                {
                    envPoints += 5;

                    if (tileInfo.flowerBugLevel > 5)
                    {
                        appealPoints -= 5;
                    }
                }
                if (tileInfo.mossBugLevel >= 3)
                {
                    envPoints += 5;

                    if (tileInfo.mossBugLevel > 5)
                    {
                        appealPoints -= 5;
                    }
                }
                if (tileInfo.wildgrassBugLevel >= 3)
                {
                    envPoints += 5;

                    if (tileInfo.wildgrassBugLevel > 5)
                    {
                        appealPoints -= 5;
                    }
                }
            }
            

        }

        if (Input.GetMouseButtonDown(0))
        {

            if (firstClick && instructions.gameObject.active)
            {
                firstClick = false;
                instructions.gameObject.SetActive(false);
                actionSelected = false;
            }

            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tileMap.WorldToCell(mousePos);

            //code to select action here
            if(GetTileType(gridPos) == "Null")
            {
                actionSelected = false;
                currentAction = "null";
            }


            //currentAction = "temp";

            //code to perform action here
            if (actionSelected)
            {
                currentTile = GetTileType(gridPos);
                Debug.Log("tile for action picked");

                if (CompatibleAction(currentTile, currentAction))
                {
                    Debug.Log(currentAction + "ed " + currentTile);
                    tileInfo.tileText.gameObject.SetActive(false);

                    if (currentAction == "water")
                    {
                        if (lastAction == "water")
                        {
                            envPoints += 10;
                            waterPoints -= 10;
                        }
                        else if (lastAction == "herb/pest")
                        {
                            envPoints -= 20;
                            pollutionPoints -= 20;
                        }

                        envPoints += 5;
                        waterPoints -= 5;
                    }
                    else if (currentAction == "herb/pest")
                    {
                        envPoints -= 70;
                        pollutionPoints -= 50;
                        appealPoints += 25;

                        if (lastAction == "water")
                        {
                            envPoints -= 20;
                            pollutionPoints -= 20;
                        }

                    }
                    else if (currentAction == "weed")
                    {
                        envPoints += 10;
                        appealPoints += 10;
                    }
                    else if (currentAction == "fertilize")
                    {
                        envPoints -= 10;
                        pollutionPoints -= 15;
                        appealPoints += 15;
                    }
                    else if (currentAction == "mow")
                    {
                        appealPoints += 10;
                        pollutionPoints -= 15;
                    }

                    lastAction = currentAction;

                    actionNum++;

                    feedback.Feedback(currentAction, actionNum, currentTile);

                    //actions have effects on tile info
                    tileInfo.ActionEffects(currentTile, currentAction);

                    //updating tileText gui
                    tileInfo.GetTileInfo(gridPos);

                    //can change the day to update tiles in TileInfoScript
                    if (actionNum % 2 == 1)
                    {
                        tileInfo.dayChange = true;
                    }
                        
                }
            }


        }

        //Quit the game when ESC is pressed
        if (Input.GetKeyDown("escape"))
        {
            if (end)
            {
                EndUI.SetActive(false);
            }

            pause.SetActive(true);
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

        //nothing can be done with dirt or paths
        if (tileName == "Dirt" || tileName == "Path")
        {
            canUseAct = false;
        }
        else if (tileName == "Moss")
        {
            if(action == "fertilize" || action == "weed")
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
        else if (tileName == "Wild_Grass")
        {
            if(action == "herb/pest" || action == "mow" || action == "weed")
            {
                canUseAct = false;
            }
        }
        else
        {
            canUseAct = false;
        }

        //text.text = "Acting: " + canUseAct;
        //Debug.Log("Acting: " + canUseAct);
        return canUseAct;
    }

    public void ChangeAction(string newAction)
    {
        currentAction = newAction;
        actionSelected = true;
        //text.text = "Action changed to " + currentAction;
        Debug.Log("action changed to " + currentAction);
        pop.Play();
    }

    public void CancelAction()
    {
        currentAction = "None";
        actionSelected = false;
        Debug.Log("action turned off");
        pop.Play();
    }

    //Quit Game Things
    public void Quit()
    {
        Application.Quit();
    }

    public void Continue()
    {
        pause.SetActive(false);

        if (end)
        {
            EndUI.SetActive(true);
        }
    }

    private void TwoHighest(float[] actionNums)
    {
        highest = Mathf.NegativeInfinity;
        second = Mathf.NegativeInfinity;

        for (int i = 0; i < actionNums.Length; i++)
        {
            if (actionNums[i] >= highest)
            {
                second = highest;
                highest = actionNums[i];
            }
            else if (actionNums[i] > second)
            {
                second = actionNums[i];
            }
        }
    }

    private void Ending()
    {

        float Water = 0;
        float Weed = 0;
        float Fertilize = 0;
        float Pesticides = 0;
        float Mow = 0;

        //Figure Out the Two Most Used Actions:
        foreach (var item in feedback.dayActions)
        {
            end = true;

            switch (item.texture.name)
            {
                case "wateringCan":
                    Water++;
                    break;
                case "weeding":
                    Weed++;
                    break;
                case "fertilizer":
                    Fertilize++;
                    break;
                case "pestHerb":
                    Pesticides++;
                    break;
                case "lawnMower":
                    Mow++;
                    break;
                default:
                    break;
            }

        }

        float[] actionsNum = new float[] { Water, Weed, Fertilize, Pesticides, Mow };

        TwoHighest(actionsNum);

        string temp = "";
        string temp2 = "";

        string temp3 = "";
        string temp4 = "";

        //Highest
        if (highest == Water)
        {
            temp = "Water";
            temp3 = "While watering your lawn everyday might seem like a good idea initially, doing so can actually cause harm to your lawn, the overall environment, and your water bill!";
        }
        else if (highest == Weed)
        {
            temp = "Weed";
            temp3 = "The environmental impact of weeding all depends on how you choose to weed your lawn. By choosing to weed by hand or use organic materials instead of chemical can make a huge difference to the environment.";
        }
        else if (highest == Fertilize)
        {
            temp = "Fertilize";
            temp3 = "While fertilizers promote the growth of healthy plants, the long term effects are very harmful to the environment. Fertilizer has been proven to the deplete the quality of the soil, add toxins into nearby water sources, and contribute to climate change.";
        }
        else if (highest == Pesticides)
        {
            temp = "Pesticides/Herbicides";
            temp3 = "Using pesticides and herbicides to get rid of pesky bugs and weeds might seem tempting, but doing so causes biodiversity loss in soil, contaminate water sources, and contributes to air pollution.";
        }
        else if (highest == Mow)
        {
            temp = "Mow";
            temp3 = "Gas powered mowers, while convenient, have been proven to contribute to air pollution and ground contamination. By switching to either an electric powered mower or a manual one, you can mow your lawn and remain environmentally friendly.";
        }

        //Second Highest
        if (second == Water)
        {
            temp2 = "Water";
            temp4 = "While watering your lawn everyday might seem like a good idea initially, doing so can actually cause harm to your lawn, the overall environment, and your water bill!";

        }
        else if (second == Weed)
        {
            temp2 = "Weed";
            temp4 = "The environmental impact of weeding all depends on how you choose to weed your lawn. By choosing to weed by hand or use organic materials instead of chemical can make a huge difference to the environment.";

        }
        else if (second == Fertilize)
        {
            temp2 = "Fertilize";
            temp4 = "While fertilizers promote the growth of healthy plants, the long term effects are very harmful to the environment. Fertilizer has been proven to the deplete the quality of the soil, add toxins into nearby water sources, and contribute to climate change.";

        }
        else if (second == Pesticides)
        {
            temp2 = "Pesticides/Herbicides";
            temp4 = "Using pesticides and herbicides to get rid of pesky bugs and weeds might seem tempting, but doing so causes biodiversity loss in soil, contaminate water sources, and contributes to air pollution.";

        }
        else if (second == Mow)
        {
            temp2 = "Mow";
            temp4 = "Gas powered mowers, while convenient, have been proven to contribute to air pollution and ground contamination. By switching to either an electric powered mower or a manual one, you can mow your lawn and remain environmentally friendly.";

        }



        switch (temp)
        {
            case "Water":
                switch (temp2)
                {
                    case "Pesticides/Herbicides":
                    case "Fertilize":
                    case "Mow":
                        bad = true;
                        break;
                    case "Weed":
                        neutral = true;
                        break;
                    default:
                        break;
                }
                break;
            case "Weed":
                switch (temp2)
                {
                    case "Water":
                    case "Fertilize":
                    case "Pesticides/Herbicides":
                        neutral = true;
                        break;
                    default:
                        break;
                }
                break;
            case "Fertilize":
                switch (temp2)
                {
                    case "Pesticides/Herbicides":
                    case "Water":
                        bad = true;
                        break;
                    case "Weed":
                    case "Mow":
                        neutral = true;
                        break;
                    default:
                        break;
                }
                break;
            case "Pesticides/Herbicides":
                switch (temp2)
                {
                    case "Water":
                    case "Fertilize":
                    case "Mow":
                        bad = true;
                        break;
                    case "Weed":
                        neutral = true;
                        break;
                    default:
                        break;
                }
                break;
            case "Mow":
                switch (temp2)
                {
                    case "Water":
                    case "Pesticides/Herbicides":
                        bad = true;
                        break;
                    case "Fertilize":
                        neutral = true;
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;
        }

        if (bad)
        {
            scoreIcons.transform.position = new Vector3(125, 410, 0);
            endingText.text = "Your lawn looks tidy and clean, but at a consequence? Throughout the week, you used " + temp + " and " + temp2 + " the most to keep your lawn looking the way you want, but at a grave cost to " +
                "the planet. You put your lawn appeal ahead of the environment, together we can change that!\n" +
                "\nYour Score: \nEnviornment Points: " + envPoints + "\nPollution Points: " + pollutionPoints + "\nWater Points: " + waterPoints + "\nAppeal Points: " + appealPoints +
                "\n\nYour Most Used Actions: \n\n" + temp + ": " + temp3 + "\n\n" + temp2 + ": " + temp4;
        }
        else if (neutral)
        {
            scoreIcons.transform.position = new Vector3(125, 383, 0);
            endingText.text = "You managed to maintain a great lawn while avoiding a negative environmental impact, way to go! Your two most used actions were " + temp + " and " + temp2 + ", which were able " +
                "to maintain your lawn while not contributing to grave levels of pollution. You're on the right track, let's see if you can do better!\n" +
                "\nYour Score: \nEnviornment Points: " + envPoints + "\nPollution Points: " + pollutionPoints + "\nWater Points: " + waterPoints + "\nAppeal Points: " + appealPoints +
                "\n\nYour Most Used Actions: \n\n" + temp + ": " + temp3 + "\n\n" + temp2 + ": " + temp4;
        }
        else
        {
            scoreIcons.transform.position = new Vector3(125, 410, 0);
            endingText.text = "You managed to maintain a beautiful lawn while helping the planet grow stronger and healthier, that’s awesome! Your two most used actions were " + temp + " and " + temp2 + "," +
                " the perfect combination for environmental happiness and health. Keep up the great work, and together we can save our Earth!\n" +
                "\nYour Score: \nEnviornment Points: " + envPoints + "\nPollution Points: " + pollutionPoints + "\nWater Points: " + waterPoints + "\nAppeal Points: " + appealPoints +
                 "\n\nYour Most Used Actions: \n\n" + temp + ": " + temp3 + "\n\n" + temp2 + ": " + temp4;
        }
    }

}
