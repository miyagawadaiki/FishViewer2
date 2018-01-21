using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphSettingContent : MyWindowContent {

	protected GraphContent gc;
	protected GraphContentType gcType;

	//[SerializeField]
	protected Button doneButton;
	protected SettingManager sm; 

	protected virtual void Awake () {
		doneButton = this.GetComponentInChildren<Button> ();

		sm = this.GetComponentInChildren<SettingManager> ();
		//sm.RegisterParameterText (gc.GetParameterText ());
	}

	// Use this for initialization
	protected virtual void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.SetSize (defaultSize);
		mwc.MoveTo (new Vector2 ());
		mwc.canExpand = false;

		doneButton.onClick.AddListener (() => mwc.Destroy ());
		doneButton.onClick.AddListener (() => gc.Set(sm.GetParameterText ()));

		sm.RegisterParameterText (gc.GetParameterText ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void RegisterGraphContent(GraphContent gc) {
		this.gc = gc;
	}
}
