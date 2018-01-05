using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SourceSetting : Setting {

	//[SerializeField]
	private Dropdown fish;
	//[SerializeField]
	private Dropdown xType;
	//[SerializeField]
	private Dropdown yType;

	// Use this for initialization
	void Start () {
		fish = elements [0].gameObject.GetComponent<Dropdown> ();
		xType = elements [1].gameObject.GetComponent<Dropdown> ();
		yType = elements [2].gameObject.GetComponent<Dropdown> ();

		fish.ClearOptions ();
		for(int i=1;i<=DataBase.fish;i++) {
			fish.options.Add(new Dropdown.OptionData { text = "fish : " + i });
		}
		fish.value = 1; fish.value = 0;

		if (type == GraphContentType.MultiEven)
			CoverElement (0);

		xType.ClearOptions ();
		yType.ClearOptions ();
		foreach (string tag in DataBase.GetTags()) {
			xType.options.Add (new Dropdown.OptionData { text = tag });
			yType.options.Add (new Dropdown.OptionData { text = tag });
		}
		xType.value = 1;	xType.value = 0;
		yType.value = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	public override void SetMemoData() {
		fish.value = gc.mFish;
		xType.value = gc.mXType;
		yType.value = gc.mYType;
	}
	*/

	public override string GetData() {
		string[] tags = DataBase.GetTags ();
		string s = fish.value + "," + xType.value + "," + yType.value + ",";
		return s;
	}
}
