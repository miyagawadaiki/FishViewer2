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

	public override void Awake () {
		base.Awake ();
		defaultPosition = new Vector2 ();
		mwc.canExpand = false;
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
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
