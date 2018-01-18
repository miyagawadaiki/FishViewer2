using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiGraphSetting : MonoBehaviour {//Setting {

	/*
	[SerializeField]
	private GameObject graphNodeObj = null;
	[SerializeField]
	private RectTransform contentRecTra = null;
	[SerializeField]
	private Button addButton = null;
	[SerializeField]
	private Text text = null;
	[SerializeField]
	private Button changeButton = null;

	private MultiVariousGraphSettingContent mvgsc;
	*/

	void Awake() {
		//mvgsc = this.GetComponentInParent<MultiVariousGraphSettingContent> ();
	}

	// Use this for initialization
	void Start () {
		//addButton.onClick.AddListener (() => this.Add ());
		//changeButton.onClick.AddListener(() => this.OpenSettingWindow());
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Translate (true);
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			Translate (false);
		}
		*/
	}

	/*
	public void Add () {
		GameObject obj = Instantiate (graphNodeObj, contentRecTra) as GameObject;
		obj.transform.SetAsLastSibling ();
		addButton.transform.SetAsLastSibling ();
		MultiEachGraphNode megn = obj.GetComponent<MultiEachGraphNode> ();
		megn.RegisterMultiGraphSetting (this);
	}

	public void OpenSettingWindow() {
		mvgsc.mwc.mwm.AddWindow ("MultiGraphSetting");
		foreach (Transform t in contentRecTra) {
			if (!t.gameObject.CompareTag ("GraphNode"))
				continue;
			MultiEachGraphNode tmp = t.gameObject.GetComponent<MultiEachGraphNode> ();
			if(tmp.isSelected)
				mvgsc.mwc.mwm.GetLastWindowController ().gameObject.GetComponent<MultiGraphSettingContent> ().RegisterMultiEachGraphNode (tmp);
		}
		
	}

	public void Select(MultiEachGraphNode megn) {
		foreach (Transform t in contentRecTra) {
			if (!t.gameObject.CompareTag ("GraphNode"))
				continue;
			MultiEachGraphNode tmp = t.gameObject.GetComponent<MultiEachGraphNode> ();
			if (tmp.Equals (megn)) {
				tmp.Select ();
				text.text = megn.GetViewText ();
			}
			else
				tmp.Unselect ();
		}
	}

	public void Remove() {
		foreach (Transform t in contentRecTra) {
			if (!t.gameObject.CompareTag ("GraphNode"))
				continue;
			MultiEachGraphNode tmp = t.gameObject.GetComponent<MultiEachGraphNode> ();
			if (tmp.isSelected) {
				Destroy (t.gameObject);
				return;
			}
		}
	}

	public void Translate(bool up) {
		MultiEachGraphNode[] megns = contentRecTra.gameObject.GetComponentsInChildren<MultiEachGraphNode> ();
		for (int i = 0; i < megns.Length; i++) {
			if (megns [i].isSelected) {
				if (up) {
					megns [i].transform.SetSiblingIndex (i - 1);
				} else {
					megns [i].transform.SetSiblingIndex (i + 1);
				}
			}
		}
	}

	public override string GetData () {
		string ret = "";

		foreach (Transform t in contentRecTra) {
			if (t.gameObject.CompareTag ("GraphNode")) {
				ret += t.GetComponent<MultiEachGraphNode> ().GetParameterText () + "\t";
			}
		}

		return ret;
	}
	*/
}
