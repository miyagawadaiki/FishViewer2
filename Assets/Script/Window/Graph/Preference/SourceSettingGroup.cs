using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SourceSettingGroup : SettingGroup {

	private List<Dropdown> ddList;

	protected override void Awake () {
		base.Awake ();

		sgName = "Source";

		ddList = new List<Dropdown> ();
		foreach (Dropdown dd in this.GetComponentsInChildren<Dropdown>())
			ddList.Add (dd);
	}

	// Use this for initialization
	protected override void Start () {
		ddList [0].ClearOptions ();
		for(int i=1;i<=DataBase.fish;i++) {
			ddList [0].options.Add(new Dropdown.OptionData { text = "fish : " + i });
		}
		ddList [0].value = 1; ddList[0].value = 0;

		ddList [1].ClearOptions ();
		ddList [2].ClearOptions ();
		ddList [3].ClearOptions ();
		foreach (string tag in DataBase.GetTags()) {
			ddList [1].options.Add (new Dropdown.OptionData { text = tag });
			ddList [2].options.Add (new Dropdown.OptionData { text = tag });
			ddList [3].options.Add (new Dropdown.OptionData { text = tag });
		}
		ddList [1].value = 1;	ddList [1].value = 0;
		ddList [2].value = 1;	ddList [2].value = 0;
		ddList [3].value = 1;	ddList [3].value = 0;

		defParameter = "0,0,1,0,";

		base.Start ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void CoverXType(int graphType) {
		if (graphType == 1)
			CoverSettingElementImmediately (1);
		else
			DiscoverElement ();
	}

	public void CoverDType (int pointerType) {
		if (pointerType == 0)
			CoverSettingElementImmediately (3);
		else
			DiscoverElement ();
	}

	protected override void SetParameter () {
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		ddList[0].value = int.Parse (tmp [0]);
		ddList[1].value = int.Parse (tmp [1]);
		ddList[2].value = int.Parse (tmp [2]);
		ddList[3].value = int.Parse (tmp [3]);
	}

	public override string GetParameterText () {
		string ret = "";

		ret += ddList [0].value + ",";
		ret += ddList [1].value + ",";
		ret += ddList [2].value + ",";
		ret += ddList [3].value + ",";

		return ret;
	}
}
