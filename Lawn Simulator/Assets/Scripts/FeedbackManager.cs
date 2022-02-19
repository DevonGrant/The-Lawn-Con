using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    //public ActionManagerScript actionManager;
    public RawImage water;
    public RawImage weed;
    public RawImage pollution;
    public RawImage gas;
    public RawImage pest;
    public RawImage bug;
    public RawImage appeal;
    public RawImage unappeal;

    public Texture waterAction;
    public Texture weedAction;
    public Texture fertilizeAction;
    public Texture mowAction;
    public Texture pestAction;

    public RawImage[] dayActions = new RawImage[14];

    private Vector3 waterOrigin;
    private Vector3 weedOrigin;
    private Vector3 pollutionOrigin;
    private Vector3 gasOrigin;
    private Vector3 pestOrigin;
    private Vector3 bugOrigin;
    private Vector3 appealOrigin;
    private Vector3 unappealOrigin;

    private bool usedOnce = false;

    private string action;
    public float speed = .75f;
    public float maxHeight = 800;

    public RawImage[] icons;

    // Start is called before the first frame update
    void Start()
    {
        icons = new RawImage[] { water, weed, pollution, gas, pest, bug, appeal, unappeal};

        foreach (var item in dayActions)
        {
            item.gameObject.SetActive(false);
        }

        waterOrigin = water.gameObject.transform.position;
        weedOrigin = weed.gameObject.transform.position;
        pollutionOrigin = pollution.gameObject.transform.position;
        gasOrigin = gas.gameObject.transform.position;
        pestOrigin = pest.gameObject.transform.position;
        bugOrigin = bug.gameObject.transform.position;
        appealOrigin = appeal.gameObject.transform.position;
        unappealOrigin = unappeal.gameObject.transform.position;

        ResetFeedback();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in icons)
        {
            if (item.gameObject.active)
            {
                item.gameObject.transform.position = new Vector3(item.gameObject.transform.position.x, item.gameObject.transform.position.y + speed, item.transform.position.z);

                if (item.gameObject.transform.position.y >= maxHeight)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ResetFeedback()
    {

        foreach (var item in icons)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void Feedback(string currentAction, int actionNum, string tileType)
    {
        ResetFeedback();

        Texture temp = null;

        if (currentAction == "water")
        {
            water.gameObject.transform.position = waterOrigin;
            water.gameObject.SetActive(true);
            temp = waterAction;
        }
        else if (currentAction == "herb/pest")
        {
            pest.gameObject.transform.position = pestOrigin;
            pest.gameObject.SetActive(true);
            bug.gameObject.transform.position = bugOrigin;
            bug.gameObject.SetActive(true);
            temp = pestAction;

            if (tileType == "Wild_grass")
            {
                unappeal.gameObject.transform.position = unappealOrigin;
                unappeal.gameObject.SetActive(true);
            }
        }
        else if (currentAction == "weed")
        {
            weed.gameObject.transform.position = weedOrigin;
            weed.gameObject.SetActive(true);
            temp = weedAction;

            //Handle Appeal Feedback
            if (tileType == "Wild_grass")
            {
                unappeal.gameObject.transform.position = unappealOrigin;
                unappeal.gameObject.SetActive(true);
            }
            else
            {
                appeal.gameObject.transform.position = appealOrigin;
                appeal.gameObject.SetActive(true);
            }
        }
        else if (currentAction == "fertilize")
        {

            appeal.gameObject.transform.position = appealOrigin;
            appeal.gameObject.SetActive(true);

            temp = fertilizeAction;

            //Polution if used more than once

            if (usedOnce)
            {
                pollution.gameObject.transform.position = pollutionOrigin;
                pollution.gameObject.SetActive(true);

            }

            usedOnce = true;
        }
        else if (currentAction == "mow")
        {
            gas.gameObject.transform.position = gasOrigin;
            gas.gameObject.SetActive(true);
            pollution.gameObject.transform.position = pollutionOrigin;
            pollution.gameObject.SetActive(true);
            temp = mowAction;

            //Handle Appeal Feedback
            if (tileType == "Moss")
            {
                unappeal.gameObject.transform.position = unappealOrigin;
                unappeal.gameObject.SetActive(true);
            }
            else if (tileType == "Grass")
            {
                appeal.gameObject.transform.position = appealOrigin;
                appeal.gameObject.SetActive(true);
            }
        }

        if (temp != null)
        {
            dayActions[actionNum - 1].gameObject.SetActive(true);
            dayActions[actionNum - 1].texture = temp;
            Debug.Log(dayActions[actionNum - 1].texture.name);
        }

    }
}
