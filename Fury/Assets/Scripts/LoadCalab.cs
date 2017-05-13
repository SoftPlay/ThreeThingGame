using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCalab : MonoBehaviour {

	void OnMouseDown()
    {
        SceneManager.LoadScene("StartCalibrate");
    }
}
