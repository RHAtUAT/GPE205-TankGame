using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;
    public GameObject target;
    private Transform targetTf;


    private void Awake()
    {
        targetTf = target.GetComponent<Transform>();
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredCameraPosition = target.transform.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        Debug.DrawRay(target.transform.localPosition, desiredCameraPosition, Color.green);
        Debug.Log("Im alive");
        if (Physics.Linecast(targetTf.position, desiredCameraPosition, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}
