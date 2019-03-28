using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWheel : MonoBehaviour
{
    private TankData tankData;
    private MeshRenderer mRenderer;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
        tankData = GetComponentInParent<TankData>();
        tankData.wheelRenderers.Add(mRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
