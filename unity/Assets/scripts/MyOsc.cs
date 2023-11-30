using extOSC;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MyOsc : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public extOSC.OSCTransmitter oscTransmitter;

    public GameObject sphere;

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

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

        //Debug.Log(value);

        float Rotation = sphere.transform.eulerAngles.x; ;

        if (value < 0)
        {
            sphere.transform.eulerAngles = new Vector3( Rotation - 10, sphere.transform.rotation.y ,sphere.transform.rotation.z);
            Debug.Log("tourne à gauche");
        }
        else if (value > 0)
        {
            sphere.transform.eulerAngles = new Vector3(Rotation + 10, sphere.transform.rotation.y, sphere.transform.rotation.z);
            Debug.Log("tourne à Droite");
        }

    }



    /**
    void ButtonMessageReceived(OSCMessage oscMessage)
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
        if (value != autreValeur && value == 0)
        {
            body.AddForce(new Vector2(0, saut), ForceMode2D.Impulse);
            son.SetActive(true);
            portesOuvertes.SetActive(true);
            portesFermer.SetActive(false);
            lightOn = 0;
        } else if (value != autreValeur && value == 1)
        {
             lightOn = 255;
            son.SetActive(false);
            portesOuvertes.SetActive(false);
            portesFermer.SetActive(true);
        }
        autreValeur = value;
  
       

    } 
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

            // Changer l'échelle de la valeur pour l'appliquer à la rotation :
            float opacite = ScaleValue(valeurLumiere, 0, 1800, 0 , 1);
        // Appliquer la rotation au GameObject ciblé :       
        opacite = 1.3f - opacite;
        vision.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, opacite);

    }

    void PotMessageReceived(OSCMessage oscMessage)
    {
       
        float valeur = oscMessage.Values[0].IntValue; // ScaleValue(oscMessage.Values[0].IntValue, 0, 4095, 40, 320);
        float rotation = ScaleValue(valeur, 0, 4095, 0, 362);
        boite.transform.eulerAngles = new Vector3(0, 0, rotation);
        Debug.Log(valeur);
    }

    **/

    // Start is called before the first frame update
    void Start()
    {
 
        oscReceiver.Bind("/encoder", RotationMessageReceived);
        //oscReceiver.Bind("/key", ButtonMessageReceived);
       //scReceiver.Bind("/light", lightMessageReceived);
       //scReceiver.Bind("/pot", PotMessageReceived);
    }

    // Update is called once per frame
    void Update()
    {
        /**
         * 
         * 
         * FUNCTION WEIRD QUE J'AI UTILISER LA DERNIERE FOIS POUR LA ROTATION
         * 
         * 
        float Rotation;
        if (sphere.transform.eulerAngles.x <= 180f)
        {
            Rotation = sphere.transform.eulerAngles.x;
        }
        else
        {
            Rotation = sphere.transform.eulerAngles.x - 360f;
        }
        Debug.Log("Rotation avec function "+Rotation);
        Debug.Log("blblblblblb " + sphere.transform.eulerAngles.x);
        **/
    }

    float startChrono;

    void LateUpdate()
    {
     // float position = boite.transform.position.x;
     // position = ScaleValue(position, -7, 7, 0, 255);

        

        if(Time.realtimeSinceStartup - startChrono > 0.050f){
            startChrono = Time.realtimeSinceStartup;
            extOSC.OSCMessage message = new OSCMessage("/pixel");
     //     message.AddValue(extOSC.OSCValue.Int((int)lightOn));
            oscTransmitter.Send(message);
        }
  

    }
}
