using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InputFileSetting : MonoBehaviour {

	[SerializeField]
	private Text fileNameText = null;
	[SerializeField]
	private Button selectButton = null;
	[SerializeField]
	private InputField fishIF = null;
	[SerializeField]
	private InputField dtIF = null;
	[SerializeField]
	private GameObject nodeObj = null;
	[SerializeField]
	private Transform content = null;

	private bool addListenerFlag;
	private StreamReader sr;
	private Dropdown[] ddArray;
	private Text[] textArray;
	private int step, fish = 5;
	private float dt = 0.1f;

	void Awake () {
		selectButton.onClick.AddListener (() => this.GetComponentInParent<MyWindowManager> ().AddWindow ("FileSelect/Input"));
		selectButton.onClick.AddListener (() => this.CallAddListener ());

		Dropdown dd = nodeObj.GetComponentInChildren<Dropdown> ();
		dd.ClearOptions ();


	}

	// Use this for initialization
	void Start () {
		fishIF.text = "5";
		dtIF.text = "0.1";
	}
	
	// Update is called once per frame
	void Update () {
		if (addListenerFlag)
			AddListener ();
	}

	public void CallAddListener() {
		addListenerFlag = true;
	}

	public void AddListener() {
		GameObject.Find("FileSelectContent(Clone)").GetComponent<FileSelectContent>().doneButton.onClick.AddListener(() => UpdateContent());
	}

	public void UpdateContent () {
		fileNameText.text = ProjectData.FileName.GetName (ProjectData.FileKey.Input);

		sr = new StreamReader (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Input), System.Text.Encoding.GetEncoding("UTF-8"));
		string s;

		do {
			s = sr.ReadLine ();
		} while (s.Equals (""));

		char[] separator = { ',' };
		string[] tmp = s.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		if (tmp.Length == 3) {
			step = int.Parse (tmp [0]);
			fish = int.Parse (tmp [1]);
			dt = float.Parse (tmp [2]);

			do {
				s = sr.ReadLine ();
			} while (s.Equals (""));
			tmp = s.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		}

		ddArray = new Dropdown[tmp.Length / fish];
		textArray = new Text[tmp.Length / fish];
		for (int i = 0; i < tmp.Length / fish; i++) {
			GameObject obj = Instantiate (nodeObj, content) as GameObject;
			ddArray [i] = obj.GetComponentInChildren<Dropdown> ();
			textArray [i] = obj.GetComponentInChildren<Text> ();
		}

		float t;
		if (!float.TryParse (tmp [0], out t)) {


			do {
				s = sr.ReadLine ();
			} while (s.Equals (""));
			tmp = s.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		}

		for (int i = 0; i < tmp.Length / fish; i++) {
			textArray [i].text = tmp [i];
		}
	}
}
