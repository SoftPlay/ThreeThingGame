using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

    private Vector3 OriginPosition;
    private Quaternion OriginRotation;

	// Use this for initialization
	void Start () 
    {
        OriginPosition = transform.position;
        OriginRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if( Input.GetKey( KeyCode.R) )
        {
            resetMe();
        }
	}

    public void resetMe()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;

        GetComponent<Rigidbody2D>().transform.position = OriginPosition;
        GetComponent<Rigidbody2D>().transform.rotation = OriginRotation;

        transform.position = OriginPosition;
        transform.rotation = OriginRotation;
    }
}
