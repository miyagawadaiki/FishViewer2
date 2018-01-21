using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerSettingGroup : SettingGroup {

	private SliderWithText pointNumSlider;
	private Dropdown colorNumDD;
	private List<Toggle> toggleList;
	private InputField plotSizeIF;

	protected override void Awake () {
		base.Awake ();

		sgName = "Pointer";

		pointNumSlider = this.GetComponentInChildren<SliderWithText> ();

		colorNumDD = this.GetComponentInChildren<Dropdown> ();

		toggleList = new List<Toggle> ();
		foreach (Toggle t in this.GetComponentsInChildren<Toggle>())
			toggleList.Add (t);

		plotSizeIF = this.GetComponentInChildren<InputField> ();
	}

	// Use this for initialization
	protected override void Start () {

		defParameter = "50,0,1,1,10,0,";

		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected override void SetParameter () {
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		pointNumSlider.SetValue (float.Parse (tmp [0]));
		colorNumDD.value = int.Parse (tmp [1]);
		toggleList [0].isOn = int.Parse (tmp [2]) > 0 ? true : false;
		toggleList [1].isOn = int.Parse (tmp [3]) > 0 ? true : false;
		toggleList [2].isOn = int.Parse (tmp [5]) > 0 ? true : false;
		plotSizeIF.text = tmp [4];
	}

	public override string GetParameterText () {
		string ret = "";

		ret += pointNumSlider.slider.value + ",";
		ret += colorNumDD.value + ",";
		ret += (toggleList [0].isOn ? "1" : "0") + ",";
		ret += (toggleList [1].isOn ? "1" : "0") + ",";
		ret += plotSizeIF.text + ",";
		ret += (toggleList [2].isOn ? "1" : "0") + ",";

		return ret;
	}
}
