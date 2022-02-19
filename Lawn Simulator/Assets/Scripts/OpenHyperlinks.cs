using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;
 
//[RequireComponent(typeof(TMP_Text))]
public class OpenHyperlinks : MonoBehaviour
{

    public void OpenLink(int num)
    {
        switch (num)
        {
            case 1:
                Application.OpenURL("https://www.lushlawn.com/blog/lawn-care/why-overwatering-is-a-bad-idea/");
                break;
            case 2:
                Application.OpenURL("https://www.worldatlas.com/articles/what-is-the-environmental-impact-of-pesticides.html");
                break;
            case 3:
                Application.OpenURL("https://www.newstalkzb.co.nz/news/national/is-mowing-your-lawn-bad-for-the-environment/#:~:text=Studies%20say%20mowing%20our%20lawns%20is%20hurting%20the,five%20and%2010%20percent%20of%20urban%20engine%20pollution.");
                break;
            case 4:
                Application.OpenURL("https://www.environment.co.za/environmental-issues/how-do-fertilizers-affect-the-environment.html#:~:text=Fertilizers%20consists%20of%20substances%20and%20chemicals%20like%20methane%2C,is%20leading%20to%20global%20warming%20and%20weather%20changes.");
                break;
            case 5:
                Application.OpenURL("http://www.sodco.net/tips-and-resources/overwatering");
                break;
            case 6:
                Application.OpenURL("https://www.ag.ndsu.edu/publications/environment-natural-resources/environmental-implications-of-excess-fertilizer-and-manure-on-water-quality");
                break;
            case 7:
                Application.OpenURL("https://www.outdoorideas.net/environmental-impact-lawn-mowers/#:~:text=%20The%20Environmental%20Impact%20Of%20Lawn%20Mowers%20,pollution%20caused%20by%20gasoline%20engines%2C%20they...%20More%20");
                break;
            case 8:
                Application.OpenURL("https://www.thespruce.com/green-weed-killers-2152938");
                break;
            case 9:
                Application.OpenURL("https://www.bigblogofgardening.com/tips-for-weeding-your-garden-and-lawn/");
                break;
            default:
                break;
        }
            }

}

