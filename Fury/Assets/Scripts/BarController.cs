using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour 
{
	private float maxGood = 0.7f;
	private float minGood = 0.1f;

	public static bool moveBar = true;

	// Use this for initialization
	void Start () 
	{
		this.gameObject.transform.localPosition = new Vector3(0, -1, 0);
		moveBar = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(moveBar)
		{
			float pressure = Fizzyo.FizzyoDevice.Instance().Pressure();

			this.gameObject.transform.localPosition = new Vector3(0, pressure, 0);
		}
	}
}
