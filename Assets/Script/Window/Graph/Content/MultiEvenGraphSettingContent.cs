using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiEvenGraphSettingContent : GraphSettingContent {

	//private MultiEvenGraphContent megc;

	//[SerializeField]
	//private Button doneButton = null;
	[SerializeField]
	private GraphTypeSetting gts = null;
	[SerializeField]
	private FishMultiSelectSetting fmss = null;
	[SerializeField]
	private SourceSetting ss = null;
	[SerializeField]
	private PointerSetting ps = null;

	void Awake() {
		gcType = GraphContentType.MultiEven;
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		ss.CoverElement (0);
		ps.CoverElement (1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	public void RegisterGraphContent(MultiEvenGraphContent megc) {
		this.megc = megc;

		foreach (Setting s in this.GetComponentsInChildren<Setting>()) {
			s.type = GraphContentType.MultiEven;
			s.gc = megc;
		}
	}
	*/

	public override void UpdateGraphContent() {
		gc.Set (GetParameterText ());
	}

	public override string GetParameterText () {
		if(fmss.GetData().Equals("")) {
			return "";
		}

		char[] separator = { ',' };
		string[] hoge = fmss.GetData ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		int num = hoge.Length;
		string ret = "";

		string[] ssElement = ss.GetData ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		string[] psElement = ps.GetData ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < num; i++) {
			ret += 
				gts.GetData () +
				hoge [i] + "," +
				ssElement [1] + "," +
				ssElement [2] + "," +
				psElement [0] + "," +
				int.Parse (hoge [i]) + "," +
				psElement [2] + "," +
				psElement [3] + "," +
				psElement [4] + "," +
				psElement [5] + "," +
				"\t";

		}

		return ret;
	}
}
