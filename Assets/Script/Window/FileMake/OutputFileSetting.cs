using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OutputFileSetting : MonoBehaviour {

	[SerializeField]
	private Text outputFileNameText = null;
	[SerializeField]
	private Button outputSelectButton = null;
	[SerializeField]
	private Text formulaFileNameText = null;
	[SerializeField]
	private Button formulaSelectButton = null;
	[SerializeField]
	private Toggle allToggle = null;
	[SerializeField]
	private GameObject nodeObj = null;
	[SerializeField]
	private Transform content = null;

	public List<DataType> dataTypes;
	public List<Toggle> toggles;

	private bool addListenerFlagO = false, addListenerFlagF = false;
	private StreamReader sr;


	void Awake () {
		outputSelectButton.onClick.AddListener (() => this.GetComponentInParent<MyWindowManager> ().AddWindow ("FileSelect/Output"));
		outputSelectButton.onClick.AddListener (() => this.CallAddOutputListener ());
		formulaSelectButton.onClick.AddListener (() => this.GetComponentInParent<MyWindowManager> ().AddWindow ("FileSelect/Formula"));
		formulaSelectButton.onClick.AddListener (() => this.CallAddFormulaListener ());

		allToggle.onValueChanged.AddListener (b => AllExecute (b));
	}

	// Use this for initialization
	void Start () {
		UpdateOutputContent ();
		UpdateFormulaContent ();
	}
	
	// Update is called once per frame
	void Update () {
		if (addListenerFlagO) {
			AddOutputListener ();
			addListenerFlagO = false;
		}

		if (addListenerFlagF) {
			AddFormulaListener ();
			addListenerFlagF = false;
		}
	}

	public void CallAddOutputListener () {
		addListenerFlagO = true;
	}

	public void AddOutputListener () {
		GameObject.Find("FileSelectContent(Clone)").GetComponent<FileSelectContent>().doneButton.onClick.AddListener(() => UpdateOutputContent());
	}

	public void CallAddFormulaListener () {
		addListenerFlagF = true;
	}

	public void AddFormulaListener () {
		GameObject.Find("FileSelectContent(Clone)").GetComponent<FileSelectContent>().doneButton.onClick.AddListener(() => UpdateFormulaContent());
	}

	public void UpdateOutputContent () {
		// ファイル名を表示
		outputFileNameText.text = ProjectData.FileName.GetName (ProjectData.FileKey.Output);
	}

	public void UpdateFormulaContent () {

		formulaFileNameText.text = ProjectData.FileName.GetName (ProjectData.FileKey.Formula);

		if (!File.Exists (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Formula)))
			return;

		sr = new StreamReader (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Formula), System.Text.Encoding.GetEncoding("UTF-8"));

		foreach (Transform tra in content)
			Destroy (tra.gameObject);

		dataTypes = new List<DataType> ();
		toggles = new List<Toggle> ();

		while (true) {
			string name;
			do {
				name = sr.ReadLine ();
			} while (name != null && (name.Equals ("") || name.IndexOf("//") >= 0));

			if (name == null)
				break;

			char[] sep = { '\t', ' ' };
			name = name.Split (sep, System.StringSplitOptions.RemoveEmptyEntries) [0];

			string formula;
			do {
				formula = sr.ReadLine ();
			} while (formula.Equals ("") || name.IndexOf("//") >= 0);

			DataType dt = new DataType (name, formula);
			dataTypes.Add (dt);

			GameObject obj = Instantiate (nodeObj, content) as GameObject;
			toggles.Add (obj.GetComponentInChildren<Toggle> ());
			Text[] texts = obj.GetComponentsInChildren<Text> ();
			texts [0].text = name;
			texts [1].text = formula;
			texts [2].text = dt.GetParametersText ();

			if (dt.HasParameter ()) {
				Button b = obj.GetComponentInChildren<Button> ();
				b.onClick.AddListener (() => OpenParameterWindow (texts [2]));
			}
		}

		sr.Close ();
	}

	public void OpenParameterWindow (Text text) {
		this.GetComponentInParent<MyWindowManager> ().AddWindow ("Parameter");
		this.GetComponentInParent<MyWindowManager> ().GetLastWindowController ().gameObject.GetComponentInChildren<ParameterContent> ().RegisterTextArea (text);
	}

	public void AllExecute (bool b) {
		for (int i = 0; i < toggles.Count; i++) {
			if (b)
				toggles [i].isOn = true;
			else
				toggles [i].isOn = false;
		}
	}
}
