using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerSetting : Setting {

	private Dropdown color;
	private Toggle colorGrad, sizeGrad, autoSize;
	private InputField sizeValue, pointNum;
	private SliderWithText markerPos;

	// Use this for initialization
	void Start () {
		pointNum = elements [0].gameObject.GetComponent<InputField> ();

		markerPos = elements [1].gameObject.GetComponent<SliderWithText> ();

		color = elements [2].gameObject.GetComponent<Dropdown> ();
		color.value = 1; color.value = 0;

		colorGrad = elements [3].gameObject.GetComponent<Toggle> ();

		sizeGrad = elements [4].gameObject.GetComponent<Toggle> ();

		sizeValue = elements [5].gameObject.GetComponent<InputField> ();

		autoSize = elements [6].gameObject.GetComponent<Toggle> ();

		if (type == GraphContentType.MultiEven)
			CoverElement (1);

		/*
		for (int i = 0; i < ProjectData.ColorList.Count(); i++) {
			color.options.Add (new Dropdown.OptionData { text = "" });
		}

		Image[] images = content.GetComponentsInChildren<Image> ();

		for (int i = 0; i < ProjectData.ColorList.Count(); i++) {
			images [i].color = ProjectData.ColorList.colors [i];
		}
		*/
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
		string s = pointNum.text + "," + markerPos.slider.value + "," + color.value + "," + (colorGrad.isOn ? "1" : "0") + "," + (sizeGrad.isOn ? "1" : "0") + "," + sizeValue.text + "," + (autoSize.isOn ? "1" : "0") + ",";
		return s;
	}
}
