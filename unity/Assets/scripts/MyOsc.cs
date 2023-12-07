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
    private int rochePosition = 0;
    private bool roche1Combinaison;
    private bool roche2Combinaison;
    private bool roche3Combinaison;
    private bool roche4Combinaison;

    public GameObject[] ligne1;
    public GameObject[] ligne2;
    public GameObject[] ligne3;
    public GameObject[] ligne4;
    private bool btnLigheEtat;
    private int lignePosition = 0;

    public GameObject[] rochesArr;


    void Start()
    {
        oscReceiver.Bind("/encoder", RotationMessageReceived);
        oscReceiver.Bind("/key1", ButtonMessageReceived);
        oscReceiver.Bind("/key2", BtnChangementPierre);
        oscReceiver.Bind("/key3", BtnChangementLigne);
        oscReceiver.Bind("/light", lightMessageReceived);
        oscReceiver.Bind("/pot", PotMessageReceived);
        oscReceiver.Bind("/tof", ControleMusique);
    }
    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }



    /*******************Btn Changement de ligne*************************/

    void BtnChangementLigne(OSCMessage oscMessage)
    {
        int value = oscMessage.Values[0].IntValue;

        if (value == 1 && btnLigheEtat == false)
        {
            lignePosition++;
            if (lignePosition == 4)
            {
                lignePosition = 0;
            }
            btnLigheEtat = true;
            print(btnLigheEtat);
        }
        else if (value == 0 && btnLigheEtat == true)
        {
            btnLigheEtat = false;
        }
    }

    /*******************Controle de la musique*************************/
    void ControleMusique(OSCMessage oscMessage)
    {
        float value = MathF.Round(ScaleValue(oscMessage.Values[0].IntValue, 0,500, 0, 3));
        Debug.Log(value);

    }


    /*******************CHANGEMENT DE LA ROCHE SELECTIONNER*************************/
    void BtnChangementPierre(OSCMessage oscMessage)
    {

        int value = oscMessage.Values[0].IntValue;

        if (value == 1 && btnRocheEtat == false)
        {
            rochePosition++;
            if(rochePosition == 4)
            {
                rochePosition = 0;
            }
            btnRocheEtat = true;
            print(rochePosition);
        }
        else if (value == 0 && btnRocheEtat == true)
        {
            btnRocheEtat = false;
        }

    }
    /*******************ROTATION DES ROCHES*************************/
    void PotMessageReceived(OSCMessage oscMessage)
    {

        float value = ScaleValue(oscMessage.Values[0].IntValue, 0, 4095, 0, 360);
       

        rochesArr[rochePosition].transform.eulerAngles = new Vector3(value, 0, 0);

        if (value >= 0 && value <= 10 && rochePosition == 0)
        {
            roche1Combinaison = true;
        }
        else if(value !>= 0 && value !<= 10 && rochePosition == 0)
        { roche1Combinaison = false; }
        if (value >= 85 && value <= 95 && rochePosition == 1)
        {
            roche2Combinaison = true;
        }
        else if (value !>= 85 && value !<= 95 && rochePosition == 0)
        { roche1Combinaison = false; }
        if (value >= 175 && value <= 185 && rochePosition == 2)
        {
            roche3Combinaison = true;
        }
        else if (value !>= 175 && value !<= 185 && rochePosition == 0)
        { roche1Combinaison = false; }
        if (value >= 265 && value <= 275 && rochePosition == 3)
        {
            roche4Combinaison = true;
        }
        else if (value !>= 265 && value !<= 275 && rochePosition == 0)
        { roche1Combinaison = false; }
        if (roche1Combinaison == true && roche2Combinaison == true && roche3Combinaison == true && roche4Combinaison == true)
        {
            Debug.Log("vous avez réussis :D");
        }
        else { Debug.Log(value); }

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
