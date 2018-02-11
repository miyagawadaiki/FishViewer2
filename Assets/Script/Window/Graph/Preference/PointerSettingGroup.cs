using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerSettingGroup : SettingGroup {

	private SliderWithText pointNumSlider;
	private Dropdown pointTypeDD, colorNumDD;
	private List<Toggle> toggleList;
	private InputField plotSizeIF;
	private SourceSettingGroup ssg;

	protected override void Awake () {
		base.Awake ();

		sgName = "Pointer";

		pointNumSlider = this.GetComponentInChildren<SliderWithText> ();

		Dropdown[] dds = this.GetComponentsInChildren<Dropdown> ();
		pointTypeDD = dds [0];
		colorNumDD = dds[1];

		pointTypeDD.ClearOptions ();
		string[] tmp = System.Enum.GetNames (typeof(PointType));
		for (int i = 0; i < tmp.Length; i++)
			pointTypeDD.options.Add (new Dropdown.OptionData { text = tmp [i] });
		
		ssg = this.transform.parent.GetComponentInChildren<SourceSettingGroup> ();
		pointTypeDD.onValueChanged.AddListener (value => ssg.CoverDType (value));

		toggleList = new List<Toggle> ();
		foreach (Toggle t in this.GetComponentsInChildren<Toggle>())
			toggleList.Add (t);

		plotSizeIF = this.GetComponentInChildren<InputField> ();
	}

	// Use this for initialization
	protected override void Start () {

		defParameter = "0,50,0,1,1,10,0,";

		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected override void SetParameter () {
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		pointTypeDD.value = 1; pointTypeDD.value = int.Parse (tmp [0]);
		pointNumSlider.SetValue (float.Parse (tmp [1]));
		colorNumDD.value = int.Parse (tmp [2]);
		toggleList [0].isOn = int.Parse (tmp [3]) > 0 ? true : false;
		toggleList [1].isOn = int.Parse (tmp [4]) > 0 ? true : false;
		toggleList [2].isOn = int.Parse (tmp [6]) > 0 ? true : false;
		plotSizeIF.text = tmp [5];
	}

	public override string GetParameterText () {
		string ret = "";

		ret += pointTypeDD.value + ",";
		ret += pointNumSlider.slider.value + ",";
		ret += colorNumDD.value + ",";
		ret += (toggleList [0].isOn ? "1" : "0") + ",";
		ret += (toggleList [1].isOn ? "1" : "0") + ",";
		ret += plotSizeIF.text + ",";
		ret += (toggleList [2].isOn ? "1" : "0") + ",";

		return ret;
	}
}
