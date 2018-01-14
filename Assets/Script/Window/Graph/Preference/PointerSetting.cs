using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerSetting : Setting {

	private Dropdown color;
	private Toggle colorGrad, sizeGrad, autoSize;
	private InputField sizeValue;
	private SliderWithText pointNum;

	// Use this for initialization
	void Start () {
		pointNum = elements [0].gameObject.GetComponent<SliderWithText> ();

		color = elements [1].gameObject.GetComponent<Dropdown> ();
		color.value = 1;

		colorGrad = elements [2].gameObject.GetComponent<Toggle> ();

		sizeGrad = elements [3].gameObject.GetComponent<Toggle> ();

		sizeValue = elements [4].gameObject.GetComponent<InputField> ();

		autoSize = elements [5].gameObject.GetComponent<Toggle> ();

		if (type == GraphContentType.MultiEven)
			CoverElement (1);

		if (gc.GetParameterText().Equals("")) {
			pointNum.SetValue (50f);
			color.value = 0;
			colorGrad.isOn = true;
			sizeGrad.isOn = false;
			sizeValue.text = "10";
			autoSize.isOn = false;
			return;
		}

		char[] separator = { '\t', ' ', ',' };
		string[] tmp = gc.GetParameterText ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		pointNum.SetValue(float.Parse(tmp[4]));
		color.value = int.Parse(tmp[5]);
		colorGrad.isOn = tmp[6].Equals("1") ? true : false;
		sizeGrad.isOn = tmp [7].Equals ("1") ? true : false;
		sizeValue.text = tmp [8];
		autoSize.isOn = tmp [9].Equals ("1") ? true : false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	public override void SetMemoData() {
		pointNum.text = gc.mPointNum + "";
		markerPos.SetValue (gc.mMarkerRate);
		color.value = gc.mPointColorNum;
		colorGrad.isOn = gc.mUseColorGrad;
		sizeGrad.isOn = gc.mUseSizeGrad;
		sizeValue.text = gc.mPlotSize + "";
		autoSize.isOn = gc.mUseAutoSize;
	}
	*/

	public override string GetData() {
		string s = (int)pointNum.slider.value + "," + color.value + "," + (colorGrad.isOn ? "1" : "0") + "," + (sizeGrad.isOn ? "1" : "0") + "," + sizeValue.text + "," + (autoSize.isOn ? "1" : "0") + ",";
		return s;
	}
}
