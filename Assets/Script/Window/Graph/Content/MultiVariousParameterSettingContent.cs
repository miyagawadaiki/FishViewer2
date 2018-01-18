using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiVariousParameterSettingContent : MyWindowContent {

	private MultiVariousParameterNode mvpn;
	private Button doneButton;
	private SettingManager sm; 

	void Awake () {
		doneButton = this.GetComponentInChildren<Button> ();

		sm = this.GetComponentInChildren<SettingManager> ();
	}

	// Use this for initialization
	void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.SetSize (defaultSize);
		mwc.MoveTo (new Vector2 ());
		mwc.canExpand = false;

		doneButton.onClick.AddListener (() => mwc.Destroy ());
		doneButton.onClick.AddListener (() => mvpn.RegisterParameter (sm.GetParameterText ()));

		sm.RegisterParameterText (mvpn.GetParameterText ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RegisterMultiVariousParameterNode (MultiVariousParameterNode mvpn) {
		this.mvpn = mvpn;
	}
}
