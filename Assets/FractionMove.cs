using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractionMove : MonoBehaviour
{
    public Transform sync;
    private Transform self;

    private float prevX;
    private float prevY;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Transform>();
        self.position = sync.position;
        prevX = sync.position.x;
        prevY = sync.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        /*float tempx = (sync.position.x - prevX) * 0.4f;
        float tempy = (sync.position.y - prevY) * 0.4f;
        prevX = sync.position.x;
        prevY = sync.position.y;*/
        float tempx = sync.position.x;
        float tempy = sync.position.y;
        Vector3 vec = new Vector3(tempx + 100, tempy +30, self.position.z) * -0.1f;
        //self.Translate(vec * speed * Time.deltaTime);
        self.position = vec;
    }
}
