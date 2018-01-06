using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithText : MonoBehaviour {

	public Slider slider;
	public Text text;

	[SerializeField]
	private float minValue = 0f;
	[SerializeField]
	private float maxValue = 1f;
	[SerializeField]
	private int digit = 0;
	[SerializeField]
	private int fontSize = 14;

	void Awake() {
		slider.minValue = minValue;
		slider.maxValue = maxValue;

		text.fontSize = fontSize;
		text.text = slider.value + "";

		slider.onValueChanged.AddListener (f => UpdateText (f));
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateText (float value) {
		float a = Mathf.Pow (10f, digit);
		slider.value = (int)((value / a) + 0.5f) * a;
		text.text = slider.value + "";
	}

	public void SetValue(float value) {
		slider.value = value;
		UpdateText (value);
	}
}
