using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

	// Use this for initialization
	void OnCollisionEnter(Collision col)
	{
		SceneManager.LoadScene("End Credit");
	}
}
