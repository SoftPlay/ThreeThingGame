using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollided : MonoBehaviour 
{
	public bool HasCollided;

	// Use this for initialization
	void Start () 
	{
		HasCollided = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.collider.name == "Terrain")
		{
			HasCollided = true;
		}
	}
}
