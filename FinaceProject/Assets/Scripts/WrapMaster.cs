using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapMaster : MonoBehaviour
{
    public GameObject topChart;
    public GameObject bottomChart;

    public List<GameObject> topChartSlices;
    public List<GameObject> bottomChartSlices;

    public List<WrapTesting> wraps;

    // Start is called before the first frame update
    void Start()
    {
        topChartSlices = topChart.GetComponent<PieChartSlice>().slices;
        bottomChartSlices = bottomChart.GetComponent<PieChartSlice>().slices;

        foreach(Transform child in transform)
        {
            wraps.Add(child.GetComponent<WrapTesting>());
        }

        Invoke("SetAllWraps", .5f);

    }

    public void SetAllWraps()
    {
        for (int i = 0; i < wraps.Count; i++)
        {
            wraps[i].bottom = bottomChartSlices[i];
            wraps[i].top = topChartSlices[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
