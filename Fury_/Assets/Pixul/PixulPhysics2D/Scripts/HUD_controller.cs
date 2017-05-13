using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD_controller : MonoBehaviour {

	public Slider slider_gravityX;
	public Text currentXValue;

	public Slider slider_gravityY;
	public Text currentYValue;

	public Slider slider_power;
	public Text currentPowerValue;

	public Slider slider_damping;
	public Text currentDampingValue;

	public GameObject circle;
	private PixulPhysics PixulPhysicsScript;

	void Start(){
		PixulPhysicsScript = circle.GetComponent<PixulPhysics>();
	}
	
	// Update is called once per frame
	void Update () {

		currentXValue.text = slider_gravityX.value.ToString();
		currentYValue.text = slider_gravityY.value.ToString();
		PixulPhysicsScript.m_gravity = new Vector2(slider_gravityX.value, slider_gravityY.value);

		currentPowerValue.text = slider_power.value.ToString();
		PixulPhysicsScript.m_power = slider_power.value;

		currentDampingValue.text = slider_damping.value.ToString();
		PixulPhysicsScript.m_damping = slider_damping.value;
	}
}
