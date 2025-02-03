using System;
using TuioUnity.Tuio20.Sxm;
using UnityEngine;

namespace SxmExamples
{
    public class DetectionImage : MonoBehaviour
    {
        public OSC OscManager;
        private string[] tempString;

        private void OnTriggerEnter(Collider other)
        {
            HandleDetection(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            HandleDetection(other, false);
        }

        private void HandleDetection(Collider other, bool isEntering)
        {
            if (other.gameObject.CompareTag("Phone"))
            {
                ScapeXMobileTransform mobileTransform = other.gameObject.GetComponent<ScapeXMobileTransform>();
                if (mobileTransform == null)
                {
                    Debug.LogError("ScapeXMobileTransform component not found on the Phone object.");
                    return;
                }

                tempString = mobileTransform.Symbol.Data.ToString().Split(':');

                if (tempString.Length >= 3)
                {
                    Debug.Log(tempString[0] + " " + tempString[1] + " " + tempString[2]);

                    if (tempString[0] == "20")
                    {
                        if (isEntering)
                            OnDetection();
                        else
                            OffDetection();

                        SendName();
                        SendEmail();
                    }
                    else
                    {
                        Debug.Log("Phone is not connected to webpage");
                    }
                }
                else
                {
                    Debug.LogWarning("Not connected");
                }
            }
        }

        private void OnDetection()
        {
            OscMessage msg = new OscMessage
            {
                address = "/DETECTION"
            };
            msg.values.Add(1);
            OscManager.Send(msg);
            Debug.Log("ON Detection");
        }

        private void OffDetection()
        {
            OscMessage msg = new OscMessage
            {
                address = "/DETECTION"
            };
            msg.values.Add(0);
            OscManager.Send(msg);
            Debug.Log("OFF Detection");

        }

        private void SendName()
        {
            OscMessage msg = new OscMessage
            {
                address = "/NAME"
            };
            msg.values.Add(tempString[1]);
            OscManager.Send(msg);
        }

        private void SendEmail()
        {
            OscMessage msg = new OscMessage
            {
                address = "/EMAIL"
            };
            msg.values.Add(tempString[2]);
            OscManager.Send(msg);
        }
    }
}
