using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleGraphSettingContent : GraphSettingContent {

	//private SingleGraphContent sgc;

	//[SerializeField]
	//private Button doneButton = null;
	[SerializeField]
	private GraphTypeSetting gts = null;
	[SerializeField]
	private SourceSetting ss = null;
	[SerializeField]
	private PointerSetting ps = null;

	//private Setting[] settings;

	void Awake() {
		//settings = this.GetComponentsInChildren<Setting> ();

		/*
		foreach (Setting s in this.GetComponentsInChildren<Setting> ()) {
			s.type = GraphContentType.Single;
			s.gc = sgc;
		}
		*/

		gcType = GraphContentType.Single;
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void UpdateGraphContent() {
		string s = GetParameterText();

		gc.Set (s);
	}

	public override string GetParameterText () {
		return gts.GetData () + ss.GetData () + ps.GetData ();
	}
}
