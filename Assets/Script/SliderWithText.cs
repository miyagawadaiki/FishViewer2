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
	private bool wholeNumbers = false;
	[SerializeField]
	private int fontSize = 14;

	// Use this for initialization
	void Start () {
		slider.minValue = minValue;
		slider.maxValue = maxValue;
		slider.wholeNumbers = wholeNumbers;

		text.fontSize = fontSize;
		text.text = slider.value + "";

		slider.onValueChanged.AddListener (f => UpdateText (f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateText (float value) {
		text.text = value + "";
	}
}
