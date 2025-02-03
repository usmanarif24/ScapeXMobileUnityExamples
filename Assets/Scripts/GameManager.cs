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
        string[] tempString;


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

                tempString = Phone.gameObject.GetComponent<ScapeXMobileTransform>().Symbol.Data.ToString().Split(':');

                //Debug.Log(tempString[0]);
                //Debug.Log(tempString[1]);
                //Debug.Log(tempString[2]);

                if (tempString[0] == "20")
                {
                    Debug.Log("x: " + Phone.gameObject.GetComponent<RectTransform>().anchoredPosition.x + "y: " + Phone.gameObject.GetComponent<RectTransform>().anchoredPosition.y);
                    //Send OSC
                    //SendOSC();
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

        void SendOSC()
        {
            OscMessage msg = new OscMessage();
            msg.address = "/VENTUZ";
            //Do string spliting to get the email & the username
            msg.values.Add(tempString[1]);
            msg.values.Add(tempString[2]);
            OscManager.Send(msg);
            //Debug.Log("Sent: " + tempString[1] + " & " + tempString[2]);
        }

    }
}
