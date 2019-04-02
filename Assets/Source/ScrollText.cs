using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollText : MonoBehaviour
{
    [SerializeField]
    TextMesh tm;

    [SerializeField]
    Text text;

    [SerializeField]
    public float speed;
    private Transform tr;
    // Use this for initialization
    void Start()
    {
        //tm = GetComponent<TextMesh>();
        //text = GetComponent<Text>();
        //this.tr = tm.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            //tr.Translate(Vector3.up * speed);
            text.rectTransform.Translate(Vector3.up * speed);

        }
    }
}
