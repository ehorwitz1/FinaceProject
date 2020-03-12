using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChartSlice : MonoBehaviour
{
    public float radius;

    public List<GameObject> slices;

    public List<RotatedAngleSlice> sliceScripts;
    public List<float> rotationList;
    public List<Color> colors;
    

    //The number of slices should be fixed over time, there should 
    //Need to be able to calculate the max angle for each slice.
    //Should just be the proportion of the finance cost * 360
    //Then need to work on getting the stacks to wrap around everything 

    // Start is called before the first frame update
    void Start()
    {
        //Get reference to children
        foreach (Transform child in transform)
        {
            slices.Add(child.gameObject);
        }

        //Get a list of the children scripts
        foreach (GameObject child in slices)
        {
            sliceScripts.Add(child.GetComponent<RotatedAngleSlice>());
        }
        //Hold total rotation
        float totalRotation = 0;
        
        //Calculate the rotation for each slice and its color
        for (int i = 0; i < sliceScripts.Count; i++)
        {
            rotationList.Add(totalRotation);
            totalRotation += sliceScripts[i].maxDegree;
            sliceScripts[i].color = colors[i];
        }


        for (int i = 0; i < sliceScripts.Count; i++)
        {
            sliceScripts[i].GenerateSlice(rotationList[i], radius);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
