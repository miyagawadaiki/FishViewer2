using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FileMakeContent : MyWindowContent {

	[SerializeField]
	private Button doneButtn = null;
	[SerializeField]
	private Slider slider = null;
	//[SerializeField]
	//private Image image = null;

	private InputFileSetting ifs;
	private OutputFileSetting ofs;
	private StreamWriter streamWriter;
	private StreamWriter debug;
	private int tagNum;
	private DataType dataT;
	private List<string> tagNames;
	//private bool flag = false;

	private int i, j, k;

	public override void Awake () {
		base.Awake ();

		defaultPosition = new Vector2 ();
		mwc.canMove = false;
		mwc.canExpand = false;

		doneButtn.onClick.AddListener (() => Make ());
		//doneButtn.onClick.AddListener (() => mwc.Destroy ());

		ifs = this.GetComponentInChildren<InputFileSetting> ();
		ofs = this.GetComponentInChildren<OutputFileSetting> ();

		slider.gameObject.SetActive (false);
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Make () {

		// ReadファイルにOutputで使ったファイルを登録
		ProjectData.FileName.Set (ProjectData.FileKey.Read, ProjectData.FileName.GetPath (ProjectData.FileKey.Output), ProjectData.FileName.GetName (ProjectData.FileKey.Output));

		tagNames = new List<string> ();


		DB.Init (ifs.step, ifs.fish, ifs.dt);

		// 一時的な処置
		DB.start = 2;

		// Input File から読み込む系列名をハッシュに登録
		for (i = 0; i < ifs.ddArray.Length; i++) {
			if (ifs.ddArray [i].value == 0)
				continue;

			int n = DB.tags.Count;
			DB.tags.Add (ifs.constTags [ifs.ddArray [i].value], n);

			tagNames.Add (ifs.constTags [ifs.ddArray [i].value]);
		}

		int fillNum = DB.tags.Count;

		for (i = 0; i < ofs.toggles.Count; i++) {
			if (ofs.toggles [i].isOn) {
				if (!DB.tags.ContainsKey (ofs.dataTypes [i].dataName)) {
					int n = DB.tags.Count;
					DB.tags.Add (ofs.dataTypes [i].dataName, n);
				}

				// DBのList<DataType>にも登録しあとで使う
				DB.dataTypes.Add (ofs.dataTypes [i]);

				tagNames.Add (ofs.dataTypes [i].dataName);
			}
		}

		DB.data = new float[ifs.step, ifs.fish, DB.tags.Count];

		// Input File からすでにある情報を読み取り
		for (i = 0; i < ifs.step; i++) {
			for (j = 0; j < ifs.fish; j++) {
				int l = 0;
				for (k = 0; k < fillNum; k++) {
					while (ifs.ddArray [l].value == 0) {
						l++;
					}

					DB.data [i, j, k] = ifs.constData [i, j, l];
					l++;
				}
			}
		}

		i = 0; j = 0; k = 0;

		StartCoroutine("CreateData");

		return;
	}

	private void Write () {
		streamWriter = new StreamWriter (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Output), false, Encoding.GetEncoding ("UTF-8"));
		streamWriter.WriteLine ((DB.step - DB.start) + "," + DB.fish + "," + DB.constNums ["dt"]);

		string t = "";
		for (i = 0; i < DB.fish; i++) {
			for (j = 0; j < tagNames.Count; j++) {
				t += tagNames [j] + ",";
			}
			t += ",";
		}
		streamWriter.WriteLine (t);

		for (i = DB.start; i < DB.step; i++) {
			string s = "";
			for (j = 0; j < DB.fish; j++) {
				for (k = 0; k < DB.tags.Count; k++) {
					s += DB.data [i, j, k] + ",";
				}
				s += ",";
			}

			streamWriter.WriteLine (s);
		}

		foreach (string sc in ofs.shortcuts) {
			char[] sep = { '@' };
			string[] tmp = sc.Split (sep, StringSplitOptions.RemoveEmptyEntries);
			string bar = tmp [1];
			for (int i = tagNames.Count - 1; i >= 0; i--) {
				//Debug.Log ("tagName = " + tagName);
				bar = bar.Replace (tagNames [i], DB.tags [tagNames [i]] + "");
			}
			char[] exSep = { '!' };
			string[] hoge = bar.Split (exSep, StringSplitOptions.RemoveEmptyEntries);
			string text = "";
			foreach (string ele in hoge)
				text += ele;
			streamWriter.WriteLine (tmp [0] + "@" + text);
		}

		streamWriter.Close ();

	}

	private IEnumerator CreateData () {

		slider.gameObject.SetActive (true);
		slider.minValue = 0f;
		slider.maxValue = DB.dataTypes.Count;
		slider.value = 0f;

		yield return null;

		for (i = 0; i < DB.dataTypes.Count; i++) {
			DataType dataT = DB.dataTypes [i];
			int tagNum = DB.tags [dataT.dataName];
			Debug.Log ("tag = " + dataT.dataName);
			for (j = 0; j < DB.step; j++) {
				for (k = 0; k < DB.fish; k++) {
					//debug = new StreamWriter ("C:/Users/Daiki/Desktop/Debug.txt", true, Encoding.GetEncoding ("UTF-8"));
					//debug.WriteLine ("\ti, j, k = " + i + ", " + j + ", " + k);
					//debug.Close ();
					//Debug.Log ("\ti, j, k = " + i + ", " + j + ", " + k);
					DB.data [j, k, tagNum] = dataT.Eval (j, k);
					//image.fillAmount += 1f / slider.maxValue;
					
				}
			}
			slider.value++;
			yield return null;
		}

		Write ();
		mwc.Destroy ();
		mwc.mwm.AddWindow (MyWindowManager.ContentType.FileRead, "");
		yield break;
	}
}

// ファイル作成用の簡易データベース
public class DB {
	public static Dictionary<string, int> tags;
	public static Dictionary<string, float> constNums;
	public static List<DataType> dataTypes;
	public static float[,,] data;
	public static int step, fish, start;

	public DB () {
		
	}

	public static void Init (int st, int fi, float dt) {
		tags = new Dictionary<string, int> ();
		constNums = new Dictionary<string, float> ();
		dataTypes = new List<DataType> ();

		step = st;
		fish = fi;
		start = 0;

		constNums.Add ("allStep", step);
		constNums.Add ("allFish", fish);
		constNums.Add ("dt", dt);
		constNums.Add ("pi", 3.141592f);
	}
}