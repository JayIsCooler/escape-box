using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class joueur : MonoBehaviour
{
    public GameObject victoire;

    public void OnTriggerEnter(Collider other)
    {
     if(other.tag == "victoire")
        {
           victoire.SetActive(true);
        }   
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "victoire")
        {
           victoire.SetActive(false);   
        }
    }

}

