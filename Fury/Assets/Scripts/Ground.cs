using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour 
{
	public int NumberOfPixels = 1;
	public GameObject Pixel;

	private float PixelWidth;
	private float ScreenWidth;
	private float ScreenHeight;

	private List<GameObject> Pixels = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		ScreenWidth = Camera.main.pixelWidth * 2;
		ScreenHeight = Camera.main.pixelHeight;

		if(NumberOfPixels == 0)
			NumberOfPixels = 1;

		PixelWidth = ScreenWidth / NumberOfPixels;


		for(int i = 0; i < NumberOfPixels / 2; i++)
		{
			int num = 0;
			GameObject go = Instantiate(Pixel);
			go.name = "Pixel";

			go.transform.localPosition = new Vector3(go.transform.localPosition.x + (PixelWidth / 100) * i, 
													 go.transform.localPosition.y - ((ScreenHeight + num) / 200), 0);
			go.transform.localScale = new Vector3(PixelWidth + 1, ScreenHeight - num, 0);
			go.transform.parent = this.gameObject.transform;
			Pixels.Add(go);

			num = 0;
			go = Instantiate(Pixel);
			go.name = "Pixel";

			go.transform.localPosition = new Vector3(go.transform.localPosition.x - (PixelWidth / 100) * i, 
													 go.transform.localPosition.y - ((ScreenHeight + num) / 200), 0);
			go.transform.localScale = new Vector3(PixelWidth + 1, ScreenHeight - num, 0);
			go.transform.parent = this.gameObject.transform;
			Pixels.Add(go);
		}

		Debug.Log(PixelWidth);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//foreach(GameObject obj in Pixels)
		//{
		//	int num = Random.Range(10, 200);
		//	obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, 
		//											  -((ScreenHeight + num) / 200), 0);
		//	obj.transform.localScale = new Vector3(obj.transform.localScale.x, ScreenHeight - num, 0);
		//}
	}
}
