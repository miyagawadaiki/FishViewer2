using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiParameterSettingGroup : SettingGroup {

	[SerializeField]
	private GameObject parameterNodeObj = null;
	[SerializeField]
	private RectTransform contentRecTra = null;
	[SerializeField]
	private Button addButton = null;
	[SerializeField]
	private Text text = null;
	[SerializeField]
	private Button changeButton = null;

	private int num = 0;

	protected override void Awake () {
		base.Awake ();

		sgName = "Multi Parameter";

		changeButton.onClick.AddListener (() => OpenSettingWindow());
	}

	// Use this for initialization
	protected override void Start () {


		defParameter = "";

		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Translate (true);
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			Translate (false);
		}
	}

	protected override void SetParameter () {
		if (parameter.Equals (""))
			return;

		char[] graphSeparator = { '\t' };
		string[] tmp = parameter.Split (graphSeparator, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < tmp.Length; i++) {
			Add (tmp [i]);
		}
	}

	public override string GetParameterText () {
		string ret = "";

		foreach (MultiVariousParameterNode mvpn in contentRecTra.gameObject.GetComponentsInChildren<MultiVariousParameterNode>()) {
			ret += mvpn.GetParameterText () + "\t";
		}

		return ret;
	}

	public void Add(string p) {
		GameObject obj = Instantiate (parameterNodeObj, contentRecTra) as GameObject;
		obj.transform.SetAsLastSibling ();
		addButton.transform.SetAsLastSibling ();
		MultiVariousParameterNode megn = obj.GetComponent<MultiVariousParameterNode> ();
		//megn.RegisterMultiParameterSettingGroup (this);
		megn.RegisterParameter (p);
		megn.number = ++num;
	}

	public void Select (MultiVariousParameterNode mvpn) {
		foreach (Transform t in contentRecTra) {
			if (t.gameObject.CompareTag ("ParameterNode")) {
				MultiVariousParameterNode tmp = t.gameObject.GetComponent<MultiVariousParameterNode> ();
				if (tmp.Equals (mvpn)) {
					tmp.Select ();
					text.text = tmp.GetViewText ();
				} else {
					tmp.Unselect ();
				}
			}
		}
				
	}

	public void OpenSettingWindow() {
		foreach (Transform t in contentRecTra) {
			if (t.gameObject.CompareTag ("ParameterNode")) {
				MultiVariousParameterNode tmp = t.gameObject.GetComponent<MultiVariousParameterNode> ();
				if (tmp.isSelected) {
					tmp.OpenSettingWindow ();
					return;
				}
			}
		}
	}

	public void Remove() {
		foreach (Transform t in contentRecTra) {
			if (t.gameObject.CompareTag ("ParameterNode") && t.gameObject.GetComponent<MultiVariousParameterNode> ().isSelected) {
				Destroy (t.gameObject);
				//--num;
				return;
			}
		}
	}

	public void Clear() {
		num = 0;
		foreach (Transform t in contentRecTra)
			if(t.gameObject.CompareTag("ParameterNode"))
				Destroy (t.gameObject);
	}

	public void Translate (bool up) {
		MultiVariousParameterNode[] mvpns = contentRecTra.gameObject.GetComponentsInChildren<MultiVariousParameterNode> ();
		for (int i = 0; i < mvpns.Length; i++) {
			if (mvpns [i].isSelected) {
				if (up) {
					mvpns [i].transform.SetSiblingIndex (i - 1);
				} else {
					mvpns [i].transform.SetSiblingIndex (i + 1);
				}
			}
		}
	}
}
