using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileMakeContent : MyWindowContent {

	[SerializeField]
	private Button doneButtn = null;

	void Awake () {
		doneButtn.onClick.AddListener (() => Make ());
	}

	// Use this for initialization
	void Start () {
		mwc = this.GetComponentInParent<MyWindowController> ();
		mwc.SetSize (defaultSize);
		mwc.canMove = false;
		mwc.canExpand = false;
		mwc.MoveTo (new Vector2 (0f, 0f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Make () {

	}
}

public class DB {
	public DataType[] dataTypes;
	public static float[,,] data;


}