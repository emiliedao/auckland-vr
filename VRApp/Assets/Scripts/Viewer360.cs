using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer360 : MonoBehaviour {

    public void Start()
    {
        
    }

    public void PickImageFromGallery()
    {
        if (NativeGallery.IsMediaPickerBusy())
        {
            Debug.Log("busy");
        }
    }

}
