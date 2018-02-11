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
	public List<string> shortcuts;

	private bool addListenerFlagO = false, addListenerFlagF = false;
	private StreamReader sr;

	private Text settingText;
	private DataType settingDT;


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
		shortcuts = new List<string> ();

		while (true) {
			// 空白行とコメント行を飛ばす
			string name;
			do {
				name = sr.ReadLine ();
			} while (name != null && (name.Equals ("") || name.IndexOf("//") >= 0));

			// nullが出たらファイルは読み終えたことになる
			if (name == null)
				break;

			// UseDefaultとあればDefaultFormulaを使う
			if (name.IndexOf ("UseDefault") >= 0) {
				DataType[] dts = ProjectData.DefaultData.dataTypes;
				for (int i = 2; i < dts.Length; i++) {

					// DataTypeに登録
					dataTypes.Add (dts [i]);

					// 確認用Toggleを生成
					GameObject ob = Instantiate (nodeObj, content) as GameObject;
					toggles.Add (ob.GetComponentInChildren<Toggle> ());
					Text[] textArray = ob.GetComponentsInChildren<Text> ();
					textArray [0].text = dts [i].dataName;
					textArray [1].text = dts [i].formula;
					textArray [2].text = dts [i].GetParametersText ();

					// パラメータ付きの式ならParameterWindowを開けるようにする
					if (dts [i].HasParameter ()) {
						Button b = ob.GetComponentInChildren<Button> ();
						b.interactable = true;
						b.onClick.AddListener (() => OpenParameterWindow (textArray [2], dts [i]));
					}
				}
				continue;
			}

			// Shortcutとあればショートカット用記述と判断
			if (name.IndexOf ("Shortcut") >= 0) {
				char[] sepa = { '_' };
				string[] hoge = name.Split (sepa, System.StringSplitOptions.RemoveEmptyEntries);
				shortcuts.Add (hoge[hoge.Length-1]);
				continue;
			}

			// タグ名と判断してタブやスペースなどのいらないものをそぎ落とす
			char[] sep = { '\t', ' ' };
			name = name.Split (sep, System.StringSplitOptions.RemoveEmptyEntries) [0];

			// 式の記述を見つけるまで空白行とコメントを飛ばす
			string formula;
			do {
				formula = sr.ReadLine ();
			} while (formula.Equals ("") || formula.IndexOf("//") >= 0);

			// DataTypeに登録
			DataType dt = new DataType (name, formula);
			dataTypes.Add (dt);

			// 確認用Toggleを生成
			GameObject obj = Instantiate (nodeObj, content) as GameObject;
			toggles.Add (obj.GetComponentInChildren<Toggle> ());
			Text[] texts = obj.GetComponentsInChildren<Text> ();
			texts [0].text = name;
			texts [1].text = formula;
			texts [2].text = dt.GetParametersText ();

			// パラメータ付きの式ならParameterWindowを開けるようにする
			if (dt.HasParameter ()) {
				Button b = obj.GetComponentInChildren<Button> ();
				b.interactable = true;
				b.onClick.AddListener (() => OpenParameterWindow (texts [2], dt));
			}
		}

		sr.Close ();
	}

	public void OpenParameterWindow (Text text, DataType dt) {
		this.GetComponentInParent<MyWindowManager> ().AddWindow ("Parameter");
		this.GetComponentInParent<MyWindowManager> ().GetLastWindowController ().gameObject.GetComponentInChildren<ParameterContent> ().Register (this, dt.GetParametersText ());
		settingText = text;
		settingDT = dt;
		//this.GetComponentInParent<MyWindowManager> ().GetLastWindowController ().gameObject.GetComponentInChildren<ParameterContent> ().RegisterTextArea (text);
		//this.GetComponentInParent<MyWindowManager> ().GetLastWindowController ().gameObject.GetComponentInChildren<ParameterContent> ().doneButton.onClick.AddListener (() => UpdateDataType ());
	}

	public void UpdateDataType (string parameterText) {
		settingText.text = parameterText;
		settingDT.SetParametersText (parameterText);
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
