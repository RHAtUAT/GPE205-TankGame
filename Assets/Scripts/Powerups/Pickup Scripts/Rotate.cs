using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float degree;
    public float step = 0.025f;
    public float amplitude = 0.5f;
    private Transform tf;
    private Vector3 origin;
    private float total = 0;

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        origin = tf.position;
    }

    // Update is called once per frame
    void Update()
    {
        tf.Rotate(0, degree * Time.deltaTime, 0);
        tf.position = origin + Vector3.up * Mathf.Sin(total += step) * amplitude * Time.deltaTime;
    }
}
