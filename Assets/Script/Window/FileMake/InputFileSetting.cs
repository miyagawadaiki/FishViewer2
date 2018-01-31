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

	public int step = 0, fish = 5;
	public float dt = 0.1f;
	public List<string> constTags;
	public float[,,] constData;

	private bool addListenerFlag;
	private StreamReader sr;
	private Dropdown[] ddArray;
	private Text[] textArray;
	//private List<string> defaultTags;

	void Awake () {
		selectButton.onClick.AddListener (() => this.GetComponentInParent<MyWindowManager> ().AddWindow ("FileSelect/Input"));
		selectButton.onClick.AddListener (() => this.CallAddListener ());

		fishIF.onEndEdit.AddListener (s => UpdateContent ());

		Dropdown dd = nodeObj.GetComponentInChildren<Dropdown> ();
		dd.ClearOptions ();

		constTags = new List<string>();
		constTags.Add ("Nonuse");
		DataType[] dta = ProjectData.DefaultData.dataTypes;
		for (int i = 0; i < dta.Length; i++) {
			constTags.Add (dta [i].dataName);
		}
	}

	// Use this for initialization
	void Start () {
		fishIF.text = "5";
		dtIF.text = "0.1";

		UpdateContent ();
	}
	
	// Update is called once per frame
	void Update () {
		if (addListenerFlag) {
			AddListener ();
			addListenerFlag = false;
		}
	}

	public void CallAddListener() {
		addListenerFlag = true;
	}

	public void AddListener() {
		GameObject.Find("FileSelectContent(Clone)").GetComponent<FileSelectContent>().doneButton.onClick.AddListener(() => UpdateContent());
	}

	public void UpdateContent () {
		// ファイル名を表示
		fileNameText.text = ProjectData.FileName.GetName (ProjectData.FileKey.Input);

		// ファイルをオープン
		sr = new StreamReader (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Input), System.Text.Encoding.GetEncoding("UTF-8"));
		string s;

		// 空行をスルーする
		do {
			s = sr.ReadLine ();
		} while (s.Equals (""));

		char[] separator = { ',' };
		string[] tmp = s.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		// 列数が3であれば step, fish, deltaTime とみなす
		if (tmp.Length == 3) {
			step = int.Parse (tmp [0]);
			fish = int.Parse (tmp [1]);
			dt = float.Parse (tmp [2]);

			do {
				s = sr.ReadLine ();
			} while (s.Equals (""));
			tmp = s.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		}
		// そうでなければ step を数え上げる
		else {
			float f;
			string hoge;
			if (!float.TryParse (tmp [0], out f)) {
				hoge = sr.ReadLine ();
			}

			do {
				step++;
				hoge = sr.ReadLine ();
			} while (hoge != null);


			// オープンし直す
			sr.Close ();
			sr = new StreamReader (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Input), System.Text.Encoding.GetEncoding("UTF-8"));

			// 空行をスルーする
			do {
				s = sr.ReadLine ();
			} while (s.Equals (""));
			do {
				s = sr.ReadLine ();
			} while (s.Equals (""));

			tmp = s.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		}


		constData = new float[step, fish, constTags.Count];



		foreach (Transform t in content)
			Destroy (t.gameObject);
		ddArray = new Dropdown[tmp.Length / fish];
		textArray = new Text[tmp.Length / fish];
		for (int i = 0; i < tmp.Length / fish; i++) {
			GameObject obj = Instantiate (nodeObj, content) as GameObject;
			ddArray [i] = obj.GetComponentInChildren<Dropdown> ();
			ddArray [i].ClearOptions ();
			for (int j = 0; j < constTags.Count; j++)
				ddArray [i].options.Add (new Dropdown.OptionData { text = constTags [j] });
			ddArray [i].value = 1;	ddArray [i].value = 0;
			textArray [i] = obj.GetComponentInChildren<Text> ();
			textArray [i].text = "";
		}

		// タグが付いていたとき
		float fuga;
		if (!float.TryParse (tmp [0], out fuga)) {
			for (int i = 0; i < tmp.Length / fish; i++) {
				for (int j = 0; j < constTags.Count; j++) {
					if (tmp [i].Equals (constTags [j]))
						ddArray[i].value = j;
				}

				constTags.Add (tmp [i]);
				for (int j = 0; j < ddArray.Length; j++)
					ddArray [j].options.Add (new Dropdown.OptionData { text = tmp [i] });
				ddArray [i].value = constTags.Count - 1;
			}

			do {
				s = sr.ReadLine ();
			} while (s.Equals (""));
			tmp = s.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		} else {
			for (int i = 0; i < tmp.Length / fish; i++)
				ddArray [i].value = i + 1;
		}


		for (int i = 0; i < 4; i++) {
			for(int j=0;j<tmp.Length / fish; j++)
				textArray [j].text += tmp [j] + "\n";
			tmp = sr.ReadLine ().Split (separator, System.StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
