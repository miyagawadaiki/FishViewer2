using System.Collections;
using System.Collections.Generic;
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

	private bool addListenerFlagO, addListenerFlagF;

	void Awake () {
		outputSelectButton.onClick.AddListener (() => this.GetComponentInParent<MyWindowManager> ().AddWindow ("FileSelect/Output"));
		outputSelectButton.onClick.AddListener (() => this.CallAddOutputListener ());
		formulaSelectButton.onClick.AddListener (() => this.GetComponentInParent<MyWindowManager> ().AddWindow ("FileSelect/Formula"));
		formulaSelectButton.onClick.AddListener (() => this.CallAddFormulaListener ());
	}

	// Use this for initialization
	void Start () {
		
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
	}
}
