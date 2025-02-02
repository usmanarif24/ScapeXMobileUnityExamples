using System.Collections;
using System.Collections.Generic;
using TuioUnity.Tuio20.Sxm;
using UnityEngine;

namespace SxmExamples
{
    public class GameManager : MonoBehaviour
    {

        GameObject Phone;

        public OSC OscManager;

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("LookForPhone", 2f, 0.2f);
        }

        void LookForPhone()
        {
            Phone = GameObject.FindGameObjectWithTag("Phone");

            if (Phone != null)
            {
                if (Phone.gameObject.GetComponent<ScapeXMobileTransform>().Symbol.Data == "20")
                {
                    //Send OSC String to Ventuz, with name and email. 
                    Debug.Log(Phone.gameObject.GetComponent<RectTransform>().anchoredPosition);
                    //if it is inside the rabeeh marker (-1500,-500)
                }
                else
                {
                    //Send osc to display text to connect to the web app
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
}
