using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour {

	// Use this for initializationv
	void OnMouseDown()
    {
        SceneManager.LoadScene("End Credit");
    }
}
