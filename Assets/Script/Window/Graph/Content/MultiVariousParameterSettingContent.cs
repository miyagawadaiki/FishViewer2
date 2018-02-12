using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiVariousParameterSettingContent : MyWindowContent {

	private MultiVariousParameterNode mvpn;
	private Button doneButton;
	private SettingManager sm; 

	public override void Awake () {
		base.Awake ();

		defaultPosition = new Vector2 ();
		mwc.canExpand = false;

		doneButton = this.GetComponentInChildren<Button> ();

		sm = this.GetComponentInChildren<SettingManager> ();
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();

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
