using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleGraphSettingContent : MyWindowContent {

	private SingleGraphContent sgc;

	[SerializeField]
	private Button doneButton = null;
	[SerializeField]
	private GraphTypeSetting gts = null;
	[SerializeField]
	private SourceSetting ss = null;
	[SerializeField]
	private PointerSetting ps = null;

	//private Setting[] settings;

	void Awake() {
		//settings = this.GetComponentsInChildren<Setting> ();

		foreach (Setting s in this.GetComponentsInChildren<Setting> ()) {
			s.type = GraphContentType.Single;
			s.gc = sgc;
		}
	}

	// Use this for initialization
	void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.SetSize (defaultSize);
		mwc.MoveTo (new Vector2 ());

		doneButton.onClick.AddListener (() => mwc.Destroy ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RegisterGraphContent (SingleGraphContent sgc) {
		this.sgc = sgc;
	}

	public void UpdateGraphContent() {
		string s = ss.GetData () + ps.GetData ();

		sgc.Set (gts.GetGraphType (), s);
	}
}
