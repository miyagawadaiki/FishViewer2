using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiGraphSettingContent : MyWindowContent {

	/*
	[SerializeField]
	private Button doneButton = null;
	[SerializeField]
	private GraphTypeSetting gts = null;
	[SerializeField]
	private SourceSetting ss = null;
	[SerializeField]
	private PointerSetting ps = null;

	private MultiEachGraphNode megn;
	*/

	// Use this for initialization
	void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.SetSize (defaultSize);
		mwc.MoveTo (new Vector2 ());
		mwc.canExpand = false;

		//doneButton.onClick.AddListener (() => mwc.Destroy ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	public void RegisterMultiEachGraphNode(MultiEachGraphNode megn) {
		this.megn = megn;
		doneButton.onClick.AddListener (() => megn.RegisterParameter (GetParameterText()));
	}

	public string GetParameterText() {
		return gts.GetData () + ss.GetData () + ps.GetData ();
	}
	*/
}
