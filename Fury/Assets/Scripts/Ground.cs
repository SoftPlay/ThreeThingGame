using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour 
{
	public int NumberOfPixels = 1;

	private int PixelWidth;
	private int ScreenWidth;

	// Use this for initialization
	void Start () 
	{
		ScreenWidth = Screen.width;

		if(NumberOfPixels == 0)
			NumberOfPixels = 1;
			
		PixelWidth = ScreenWidth / NumberOfPixels;
		Debug.Log(PixelWidth);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
