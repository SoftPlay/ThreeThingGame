using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullets : MonoBehaviour 
{
	public GameObject Bullet;
	public Vector3 BulletPos;
	private GameObject go;
	// Use this for initialization
	void Start () 
	{
		go = GameObject.Instantiate(Bullet);

		go.transform.parent = this.gameObject.transform;
		go.transform.localPosition = BulletPos;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(go == null)
		{
			go = GameObject.Instantiate(Bullet);

			go.transform.parent = this.gameObject.transform;
			go.transform.localPosition = BulletPos;
		}
	}
}
