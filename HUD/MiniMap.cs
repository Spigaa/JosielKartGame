using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private LineRenderer lineRendere;
    private GameObject TrackPath;

    // Start is called before the first frame update
    void Start()
    {
        lineRendere = GetComponent<LineRenderer>();
        TrackPath = this.gameObject;

        int num_of_path = TrackPath.transform.childCount;
        lineRendere.positionCount = num_of_path + 1;

        for (int x = 0; x< num_of_path; x++)
        {
            lineRendere.SetPosition(x, new Vector3(TrackPath.transform.GetChild(x).transform.position.x, 0, TrackPath.transform.GetChild(x).transform.position.z));
        }

        lineRendere.SetPosition(num_of_path, lineRendere.GetPosition(0));

        lineRendere.startWidth = 45f;
        lineRendere.endWidth = 45;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
