using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishMultiSettingGroup : SettingGroup {

	[SerializeField]
	private GameObject fishNodeObj = null;
	[SerializeField]
	private Transform contentTra = null;

	private Toggle allToggle;

	private Toggle[] toggles;

	protected override void Awake () {
		base.Awake ();

		sgName = "Fish Select";

		allToggle = this.GetComponentInChildren<Toggle> ();

		toggles = new Toggle[DataBase.fish];
		for (int i = 0; i < toggles.Length; i++) {
			GameObject obj = Instantiate (fishNodeObj, contentTra) as GameObject;
			toggles [i] = obj.GetComponent<Toggle> ();
			obj.GetComponentInChildren<Image> ().color = ProjectData.ColorList.GetColor (i);
			obj.GetComponentInChildren<Text> ().text += (i + 1) + "";
		}

		elementNum = DataBase.fish;
	}

	// Use this for initialization
	protected override void Start () {
		allToggle.isOn = true;
		allToggle.onValueChanged.AddListener (b => ExcuteAllToggle (b));

		defParameter = "";

		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	protected override void SetParameter () {
		ResetAllToggle ();

		if (parameter.Equals ("")) {
			allToggle.isOn = true;
			SetAllToggle ();
			return;
		}

		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < tmp.Length; i++) {
			if (int.Parse (tmp [i]) > 0)
				toggles [i].isOn = true;
			else
				toggles [i].isOn = false;
		}
	}

	public override string GetParameterText () {
		string ret = "";

		for(int i = 0; i < toggles.Length; i++) {
			if (toggles [i].isOn)
				ret += "1,";
			else
				ret += "0,"; 
		}

		return ret;
	}

	public void ExcuteAllToggle (bool b) {
		if (b)
			SetAllToggle ();
		else
			ResetAllToggle ();
	}

	private void SetAllToggle() {
		foreach (Toggle t in toggles)
			t.isOn = true;
	}

	private void ResetAllToggle() {
		foreach (Toggle t in toggles)
			t.isOn = false;
	}
}
