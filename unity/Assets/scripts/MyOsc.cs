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

    public GameObject blackLight;
    private bool blackLightEtat;

    public GameObject objetSecretsContainer;

    public GameObject lumierePlafond;

    private bool btnRocheEtat;
    private int rochePosition = 0;
    private bool roche1Combinaison;
    private bool roche2Combinaison;
    private bool roche3Combinaison;
    private bool roche4Combinaison;
   

    public GameObject[] ligneNotes;

    private bool btnLigheEtat;
    public List<int> melodie;
    public List<int> melodieJoueur;
    private int verifMelodi = 0;
    public GameObject[] rochesArr;


    public bool reussisDefis1 = false;
    public bool reussisDefis2 = false;

    public GameObject crystal1;
    public GameObject crystal2;

    public GameObject melodiSon;
    public GameObject mauvaiseMelodi;
    private bool fin;
    public GameObject imageFin;
    void Start()
    {
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
            if(fin == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            btnLigheEtat = true;
            for (int i = 0; i < ligneNotes.Length; i++)
            {

                if (ligneNotes[i].activeInHierarchy == true)
                {
                    ligneNotes[i].GetComponent<AudioSource>().Play();

                }
            }
        }
        else if (value == 0 && btnLigheEtat == true)
        {
            
           
            for(int i = 0; i < ligneNotes.Length; i++)
            {
                
                    if(ligneNotes[i].activeInHierarchy == true)
                {
                    melodieJoueur.Add(int.Parse(ligneNotes[i].name));
                    
                    
                }
            }
            btnLigheEtat = false;
        }
        else if (melodieJoueur.Count == 4)
        {
            if (melodieJoueur[0] == melodie[0])
            {
                verifMelodi++;
            }
            if (melodieJoueur[1] == melodie[1])
            {
                verifMelodi++;
            }
            if (melodieJoueur[2] == melodie[2])
            {
                verifMelodi++;
            }
            if (melodieJoueur[3] == melodie[3])
            {
                verifMelodi++;
            }
            if (verifMelodi == 4)
            {
                Debug.Log("réussis!");
                reussisDefis2 = true;
                crystal1.SetActive(true);
            }
            else
            {
                melodieJoueur.Clear();
                mauvaiseMelodi.GetComponent<AudioSource>().Play();
            }



        }
    }

    /*******************Controle de la musique*************************/
    void ControleMusique(OSCMessage oscMessage)
    {
        int value = (int) MathF.Round(ScaleValue(oscMessage.Values[0].IntValue, 0, 500, 0, 3));
        
        if(reussisDefis2 == false)
        {

        
        if (value == 0)
        {
            ligneNotes[0].SetActive(true);
            ligneNotes[1].SetActive(false);
            ligneNotes[2].SetActive(false);
            ligneNotes[3].SetActive(false);
        }
        if (value == 1)
        {
            ligneNotes[0].SetActive(false);
            ligneNotes[1].SetActive(true);
            ligneNotes[2].SetActive(false);
            ligneNotes[3].SetActive(false);
        }
        if (value == 2)
        {
            ligneNotes[0].SetActive(false);
            ligneNotes[1].SetActive(false);
            ligneNotes[2].SetActive(true);
            ligneNotes[3].SetActive(false);
        }
        if (value == 3)
        {
            ligneNotes[0].SetActive(false);
            ligneNotes[1].SetActive(false);
            ligneNotes[2].SetActive(false);
            ligneNotes[3].SetActive(true);
        }
        }
        else if (reussisDefis2 == true)
        {
            ligneNotes[0].SetActive(false);
            ligneNotes[1].SetActive(false);
            ligneNotes[2].SetActive(false);
            ligneNotes[3].SetActive(false);
        }
    }


    /*******************CHANGEMENT DE LA ROCHE SELECTIONNER*************************/
    void BtnChangementPierre(OSCMessage oscMessage)
    {

        int value = oscMessage.Values[0].IntValue;

        if (value == 1 && btnRocheEtat == false)
        {
            if (fin == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            rochePosition++;
            if(rochePosition == 4)
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

        float value = ScaleValue(oscMessage.Values[0].IntValue, 0, 4095, 0, 360);



      if (reussisDefis1 == false) {
            rochesArr[rochePosition].transform.eulerAngles = new Vector3(value, 0, 0);
            if (value >= 175 && value <= 185 && rochePosition == 0)
        {
            roche1Combinaison = true;

        }
        else if(value !>= 175 && value !<= 185 && rochePosition == 0)
        { roche1Combinaison = false;
                }
        if (value >= 265 && value <= 275 && rochePosition == 1)
        {
            roche2Combinaison = true;
        }
        else if (value !>= 265 && value !<= 275 && rochePosition == 1)
        { roche1Combinaison = false;
        }
        if (value >= 340 && value <= 365 &&  rochePosition == 2)
        {
            roche3Combinaison = true;
        }
        else if (value ! >= 340 && value !<= 365 && rochePosition == 2)
        { roche1Combinaison = false;
        }
        if (value >= 85 && value <= 95 && rochePosition == 3)
        {
            roche4Combinaison = true;
        }
        else if (value !>= 85 && value !<= 95 && rochePosition == 3)
        { roche1Combinaison = false;
        }
        if (roche1Combinaison == true && roche2Combinaison == true && roche3Combinaison == true && roche4Combinaison == true)
        {
            reussisDefis1 = true;
                crystal2.SetActive(true);
            }
      }
    }

    /********************************CODE POUR LA BLACKLIGHT ET LE KEY UNIT***************************************/

 
 
    void ButtonMessageReceived(OSCMessage oscMessage)
    {
       int value = oscMessage.Values[0].IntValue;

        if (value == 1 && blackLightEtat == false)
        {
            if (fin == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            blackLight.SetActive(true);
 
            blackLightEtat = true;
        }
        else if (value == 0 && blackLightEtat == true)
        {
            blackLightEtat = false;
            blackLight.SetActive(false);
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
        if (blackLight.activeInHierarchy == true && lumierePlafond.GetComponent<Light>().intensity <= 0.8)
        {
            objetSecretsContainer.SetActive(true);
        }
        else if (lumierePlafond.GetComponent<Light>().intensity <= 1)
        {
            melodiSon.SetActive(false);
        }
        else if (lumierePlafond.GetComponent<Light>().intensity >= 1)
        {
            melodiSon.SetActive(true);
        }
        if (lumierePlafond.GetComponent<Light>().intensity >= 0.8)
        {
            objetSecretsContainer.SetActive(false);
        }

        if(reussisDefis1 == true && reussisDefis2 == true)
        {
            imageFin.SetActive(true);
            fin = true;
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
