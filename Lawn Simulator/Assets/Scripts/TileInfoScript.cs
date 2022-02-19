using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;


public class TileInfoScript : MonoBehaviour
{
    public TileManagerScript tileManager;
    public ActionManagerScript actionManager;
    public Camera cam;
    public Tilemap tileMap;

    public TextMeshProUGUI tileText;

    private bool startInfo;
    public bool dayChange;


    // water level goes down by 1 every day, don't let it reach 0?
    // bug level goes up by 1 randomly, don't let it reach 5?
    //weed level goes up exponentially, don't let it reach 5?
    //grass height goes up by 1 daily, lowers appeal?

    [SerializeField] public int grassWaterLevel;
    [SerializeField] public int grassBugLevel;
    [SerializeField] public float grassWeedLevel;
    [SerializeField] public int grassGrassLevel;

    [SerializeField] public int flowerWaterLevel;
    [SerializeField] public int flowerBugLevel;
    [SerializeField] public float flowerWeedLevel;
    //[SerializeField] public int flowerGrassLevel;

    [SerializeField] public int mossWaterLevel;
    [SerializeField] public int mossBugLevel;
    //[SerializeField] public int mossWeedLevel;
    //[SerializeField] public int mossGrassLevel;

    [SerializeField] public int wildgrassWaterLevel;
    [SerializeField] public int wildgrassBugLevel;
    [SerializeField] public float wildgrassWeedLevel;
    [SerializeField] public int wildgrassGrassLevel;


    //keeping track of the # of different kinds of tiles
    [SerializeField] public int dirtTiles;
    [SerializeField] public int grassTiles;
    [SerializeField] public int pathTiles;
    [SerializeField] public int mossTiles;
    [SerializeField] public int flowerTiles;
    [SerializeField] public int wildGrassTiles;


    // Start is called before the first frame update
    void Start()
    {
        //tileText.gameObject.SetActive(false);
        dayChange = false;
        startInfo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tileMap.WorldToCell(mousePos);

            GetTileInfo(gridPos);
        }

        if(startInfo == true)
        {
            SetUpTiles();
        }




        if (dayChange)
        {
            DayUpdate();
            dayChange = false;
            Debug.Log("Day Changes");
        }




    }

    private void SetUpTiles()
    {
        //info for grass tiles
        grassWaterLevel = 5;
        grassBugLevel = 0;
        grassWeedLevel = 0;
        grassGrassLevel = 1;

        //info for moss tiles
        mossWaterLevel = 4;
        mossBugLevel = 0;

        //info for flower tiles
        flowerWaterLevel = 3;
        flowerBugLevel = 0;
        flowerWeedLevel = 0;

        //info for wild grass tiles
        wildgrassWaterLevel = 6;
        wildgrassBugLevel = 1;
        wildgrassWeedLevel = 1;
        wildgrassGrassLevel = 2;


        startInfo = false;
    }

    public void GetTileInfo(Vector3Int gridPos)
    {

        //Debug.Log("action selected is false");
        //tileText.gameObject.SetActive(false);

        if (tileMap.HasTile(gridPos))
        {
            string temp = tileMap.GetTile(gridPos).name;
            Debug.Log("found tile for info");

            if (temp == "Dirt")
            {
                tileText.text = "DIRT: \n Dirt has no stats, and counts as a 'dead' tile.";
            }
            else if (temp == "Path")
            {
                tileText.text = "PATH: \n Path has no stats, but it does raise appeal!";
            }
            else if (temp == "Grass")
            {
                tileText.text = "GRASS: \n Water Level: " + grassWaterLevel + "   Bug Level: " + grassBugLevel + "   Weed Level: " + grassWeedLevel + "   Grass Height: " + grassGrassLevel;
            }
            else if (temp == "Moss")
            {
                tileText.text = "MOSS: \n Water Level: " + mossWaterLevel + "   Bug Level: " + mossBugLevel;
            }
            else if (temp == "Flower")
            {
                tileText.text = "FLOWER: \n Water Level: " + flowerWaterLevel + "   Bug Level: " + flowerBugLevel + "   Weed Level: " + flowerWeedLevel;
            }
            else if (temp == "Wild_Grass")
            {
                tileText.text = "WILD GRASS: \n Water Level: " + wildgrassWaterLevel + "   Bug Level: " + wildgrassBugLevel + "   Weed Level: " + wildgrassWeedLevel + "   Grass Height: " + wildgrassGrassLevel;
            }

            tileText.gameObject.SetActive(true);
        }


    }

    private void DayUpdate()
    {
        

        dayChange = false;

        // water level goes down by 1 every day, don't let it reach 0?
        // bug level goes up by 1 randomly, don't let it reach 5?
        //weed level goes up exponentially, don't let it reach 5?
        //grass height goes up by 1 daily, lowers appeal?

        //changing water level
        grassWaterLevel -= 1;
        mossWaterLevel -= 1;
        flowerWaterLevel -= 1;
        wildgrassWaterLevel -= 1;

        //changing bug level

        if (UnityEngine.Random.Range(0, 100) <= 25)
        {
            grassBugLevel += 1;
        }

        if (UnityEngine.Random.Range(0, 100) <= 25)
        {
            mossBugLevel += 1;
        }

        if (UnityEngine.Random.Range(0, 100) <= 25)
        {
            flowerBugLevel += 1;
        }

        if (UnityEngine.Random.Range(0, 100) <= 25)
        {
            wildgrassBugLevel += 1;
        }

        //changing weed level

        if (UnityEngine.Random.Range(0, 100) <= 50)
        {
            grassWeedLevel += 1;
        }

        if (UnityEngine.Random.Range(0, 100) <= 50)
        {
            flowerWeedLevel += 1;
        }

        if (UnityEngine.Random.Range(0, 100) <= 50)
        {
            wildgrassWeedLevel += 1;
        }

        //changing grass height
        grassGrassLevel += 1;
        wildgrassGrassLevel += 1;

    }

    public void ActionEffects(string currentTile, string action)
    {

        Debug.Log("effect triggered on "+ currentTile +action);

        if(currentTile == "Grass")
        {
            if(action == "mow")
            {
                grassGrassLevel = 0;
            }
            else if(action == "water")
            {
                grassWaterLevel = 5;
                Debug.Log("Water effect on grass");
            }
            else if(action == "weed")
            {
                grassWeedLevel = 0;
            }
            else if(action == "herb/pest")
            {
                grassWeedLevel = 0;
                grassBugLevel = 0;
            }
        }
        else if(currentTile == "Moss")
        {
            if(action == "water")
            {
                mossWaterLevel = 4;
            }
            else if(action == "herb/pest")
            {
                mossBugLevel = 0;
            }
        }
        else if(currentTile == "Flower")
        {
            if (action == "water")
            {
                flowerWaterLevel = 3;
            }
            else if (action == "weed")
            {
                flowerWeedLevel = 0;
            }
            else if (action == "herb/pest")
            {
                flowerWeedLevel = 0;
                flowerBugLevel = 0;
            }
        }
        else if(currentTile == "Wild_Grass")
        {
            if (action == "mow")
            {
                wildgrassGrassLevel = 0;
            }
            else if (action == "water")
            {
                wildgrassWaterLevel = 6;
            }
            else if (action == "weed")
            {
                wildgrassWeedLevel = 0;
            }
            else if (action == "herb/pest")
            {
                wildgrassWeedLevel = 0;
                wildgrassBugLevel = 0;
            }
        }
    }

    public void GetTileTypes()
    {
        
      TileBase[] tileArray = tileMap.GetTilesBlock(tileMap.cellBounds);
      for (int i = 0; i < tileArray.Length; i++)
      {/*
           for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
           {
               for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
               {
                   Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                   Vector3 place = tileMap.CellToWorld(localPlace);
                   if (tileMap.HasTile(localPlace))
                   {
                       //Tile at "place"
                       availablePlaces.Add(place);
                   }
                   else
                   {
                       //No tile at "place"
                   }
               }
           }
            */
           if (tileArray[i] != null)
           {
               string thisTile = tileArray[i].name;
               //Debug.Log(thisTile);

               if(thisTile == "Dirt")
               {
                    Debug.Log(thisTile);
                    Debug.Log(i);
                    dirtTiles++;

                }
               else if(thisTile == "Grass")
               {
                   Debug.Log(thisTile);
                   Debug.Log(i);
                    grassTiles++;
               }
               else if(thisTile == "Moss")
               {
                   Debug.Log(thisTile);
                   Debug.Log(i);
                    mossTiles++;
               }
               else if(thisTile == "Path")
               {
                   Debug.Log(thisTile);
                   Debug.Log(i);
                    pathTiles++;

               }
               else if(thisTile == "Flower")
               {
                   Debug.Log(thisTile);
                   Debug.Log(i);
                    flowerTiles++;
               }
               else if(thisTile == "Wild_Grass")
               {
                   Debug.Log(thisTile);
                   Debug.Log(i);
                    wildGrassTiles++;
               }
               
           }
            //string thisTile = tileArray[i].name;

            //Debug.Log(thisTile);

            //Debug.Log("Hello~");
            actionManager.finalCheck = false;

      } 
    }

}
