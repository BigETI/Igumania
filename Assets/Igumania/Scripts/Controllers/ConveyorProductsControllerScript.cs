using UnityEngine;

// TODO: Fix poor API design without breaking references

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
