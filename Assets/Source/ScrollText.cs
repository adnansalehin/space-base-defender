using UnityEngine;
using System.Collections;

public class ScrollText : MonoBehaviour
{
    TextMesh tm = new TextMesh();
    public float speed;
    private Transform tr;
    // Use this for initialization
    void Start()
    {
        tm = GetComponent<TextMesh>();
        this.tr = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            tr.Translate(Vector3.up * this.speed);
        }
    }
}
