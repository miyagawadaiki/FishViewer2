using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishMultiSelectSetting : Setting {


	[SerializeField]
	private GameObject fishNodeObj = null;
	[SerializeField]
	private Transform contentTra = null;

	private Toggle allToggle;

	private Toggle[] toggles;

	// Use this for initialization
	void Start () {
		allToggle = elements [0].GetComponent<Toggle> ();
		allToggle.isOn = true;
		allToggle.onValueChanged.AddListener (b => ExcuteAllToggle (b));

		toggles = new Toggle[DataBase.fish];

		for (int i = 0; i < DataBase.fish; i++) {
			GameObject obj = Instantiate (fishNodeObj, contentTra) as GameObject;
			toggles [i] = obj.GetComponent<Toggle> ();
			obj.GetComponentInChildren<Image> ().color = ProjectData.ColorList.GetColor (i);
			obj.GetComponentInChildren<Text> ().text += (i + 1) + "";
		}

		SetAllToggle ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetAllToggle() {
		foreach (Toggle t in toggles)
			t.isOn = true;
	}

	public void ResetAllToggle() {
		foreach (Toggle t in toggles)
			t.isOn = false;
	}

	public void ExcuteAllToggle(bool b) {
		if (b)
			SetAllToggle ();
		else
			ResetAllToggle ();
	}

	public override string GetData() {
		string s = "";

		for(int i = 0; i < toggles.Length; i++) {
			if (toggles[i].isOn)
				s += i + ",";
		}

		return s;
	}
}
