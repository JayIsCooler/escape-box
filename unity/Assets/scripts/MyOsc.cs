using extOSC;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MyOsc : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public extOSC.OSCTransmitter oscTransmitter;

    public GameObject sphere;


    public GameObject blackLight;
    private bool blackLightEtat;
    private bool blackLightBtnPrete = false;

    public GameObject objetSecretsContainer;

    public GameObject lumierePlafond;

    private bool btnRocheEtat;
    private bool btnRochePrete;
    private int rochePosition = 1;

    public GameObject[] rochesArr;


    void Start()
    {   
        oscReceiver.Bind("/encoder", RotationMessageReceived);
        oscReceiver.Bind("/key1", ButtonMessageReceived);
        oscReceiver.Bind("/key2", BtnChangementPierre);
        oscReceiver.Bind("/light", lightMessageReceived);
        oscReceiver.Bind("/pot", PotMessageReceived);
    }
    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    /*******************CHANGEMENT DE LA ROCHE SELECTIONNER*************************/
    void BtnChangementPierre(OSCMessage oscMessage)
    {

        int value = oscMessage.Values[0].IntValue;

        if (value == 1 && btnRocheEtat == false)
        {
            rochePosition++;
            if(rochePosition == 5)
            {
                rochePosition = 0;
            }
            btnRocheEtat = true;
        }
        else if (value == 0 && btnRocheEtat == true)
        {
            btnRocheEtat = false;
        }

    }
    /*******************ROTATION DES ROCHES*************************/
    void PotMessageReceived(OSCMessage oscMessage)
    {

        float value = ScaleValue(oscMessage.Values[0].FloatValue, 0, 4095, 40, 320);

        if(rochePosition == 1)
        {
            rochesArr[0].transform.eulerAngles = new Vector3(value, 0, 0);
        }
        if (rochePosition == 2)
        {
            rochesArr[1].transform.eulerAngles = new Vector3(value, 0, 0);
        }
        if (rochePosition == 3)
        {
            rochesArr[2].transform.eulerAngles = new Vector3(value, 0, 0);
        }
        if (rochePosition == 4)
        {
            rochesArr[3].transform.eulerAngles = new Vector3(value, 0, 0);
        }



    }



    /********************************CODE POUR LA COMBINAISON ET LE ENCODER***************************************/
    void RotationMessageReceived(OSCMessage oscMessage)
    {
        float value;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            return;
        }

        if (value < 0)
        {
            sphere.transform.Rotate(sphere.transform.rotation.x + 10, sphere.transform.rotation.y ,sphere.transform.rotation.z);
        }
        else if (value > 0)
        {
            sphere.transform.Rotate(sphere.transform.rotation.x - 10, sphere.transform.rotation.y, sphere.transform.rotation.z);
        }
  
    }


    /********************************CODE POUR LA BLACKLIGHT ET LE KEY UNIT***************************************/

 
 
    void ButtonMessageReceived(OSCMessage oscMessage)
    {
       int value = oscMessage.Values[0].IntValue;

        if (value == 1 && blackLightEtat == false && blackLightBtnPrete == false)
        {
            blackLight.SetActive(true);
 
            blackLightEtat = true;
        }
        else if (value == 0 && blackLightEtat == true && blackLightBtnPrete == false)
        {
            blackLightBtnPrete = true;
        }
        else if (value == 1 && blackLightEtat == true && blackLightBtnPrete == true)
        {
            blackLight.SetActive(false);
            blackLightEtat = false;
        }
        else if (value == 0 && blackLightEtat == false && blackLightBtnPrete == true)
        {
            blackLightBtnPrete = false;
        }
       


    }



    /********************************CODE POUR LA LUMIERE QUI SORS DU PLAFOND ET LE LIGHT UNIT***************************************/



    void lightMessageReceived(OSCMessage oscMessage)
    {

        float valeurLumiere;
        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            valeurLumiere = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            valeurLumiere = oscMessage.Values[0].FloatValue;
        }
        else
        {

            return;
        }


        float scaledValeurLumiere = ScaleValue(valeurLumiere, 4096,0, 0, 2);
        lumierePlafond.GetComponent<Light>().intensity = scaledValeurLumiere;

           

    }

    private void Update()
    {
        if (blackLight.activeInHierarchy == true && lumierePlafond.GetComponent<Light>().intensity <= 0.5)
        {
            objetSecretsContainer.SetActive(true);
        }
        else
        {
            objetSecretsContainer.SetActive(false);
        }
    }









    // Update is called once per frame

    float startChrono;

    void LateUpdate()
    {
        // float position = boite.transform.position.x;
        // position = ScaleValue(position, -7, 7, 0, 255);


        /**
        if(Time.realtimeSinceStartup - startChrono > 0.050f){
            startChrono = Time.realtimeSinceStartup;
            extOSC.OSCMessage message = new OSCMessage("/pixel");
      message.AddValue(extOSC.OSCValue.Int((int)lightOn));
            oscTransmitter.Send(message);
        }
  
    **/
    }

}
