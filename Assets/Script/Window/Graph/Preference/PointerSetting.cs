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

		if (gc.memo != null) {
			pointNum.slider.value = (float)gc.memo.pointNum;
			pointNum.text.text = gc.memo.pointNum + "";
			color.value = gc.memo.pointColorNum;
			colorGrad.isOn = gc.memo.useColorGrad;
			sizeGrad.isOn = gc.memo.useSizeGrad;
			sizeValue.text = gc.memo.plotSize + "";
			autoSize.isOn = gc.memo.useAutoSize;
		} else {
			pointNum.SetValue (50f);
			color.value = 0;
			colorGrad.isOn = true;
			sizeGrad.isOn = false;
			sizeValue.text = "10";
			autoSize.isOn = false;
		}

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
		string s = (int)pointNum.slider.value + "," + color.value + "," + (colorGrad.isOn ? "1" : "0") + "," + (sizeGrad.isOn ? "1" : "0") + "," + sizeValue.text + "," + (autoSize.isOn ? "1" : "0") + ",";
		return s;
	}
}
