using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphSettingContent : MyWindowContent {

	protected GraphContent gc;
	protected GraphContentType gcType;

	[SerializeField]
	private Button doneButton = null;

	// Use this for initialization
	protected virtual void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.SetSize (defaultSize);
		mwc.MoveTo (new Vector2 ());
		mwc.canExpand = false;

		doneButton.onClick.AddListener (() => mwc.Destroy ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void RegisterGraphContent(GraphContent gc) {
		this.gc = gc;

		foreach (Setting s in this.GetComponentsInChildren<Setting>()) {
			s.type = gcType;
			s.gc = gc;
		}
	}

	public virtual void UpdateGraphContent() {

	}

	public virtual string GetParameterText() {
		return "";
	}
}
