using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorProductsControllerScript : MonoBehaviour
{
    public GameObject startObject;
    public GameObject endObject;

    public void ShowStartObject()
    {
        endObject.SetActive(false);
        startObject.SetActive(true);
    }

    public void ShowEndObject()
    {
        startObject.SetActive(false);
        endObject.SetActive(true);
    }
}
