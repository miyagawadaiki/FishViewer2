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

	private InputFileSetting ifs;
	private OutputFileSetting ofs;

	void Awake () {
		doneButtn.onClick.AddListener (() => Make ());
		doneButtn.onClick.AddListener (() => mwc.Destroy ());

		ifs = this.GetComponentInChildren<InputFileSetting> ();
		ofs = this.GetComponentInChildren<OutputFileSetting> ();
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
		List<string> tagNames = new List<string> ();

		DB.Init (ifs.step, ifs.fish, ifs.dt);

		// Input File から読み込む系列名をハッシュに登録
		for (int i = 0; i < ifs.ddArray.Length; i++) {
			if (ifs.ddArray [i].value == 0)
				continue;

			int n = DB.tags.Count;
			DB.tags.Add (ifs.constTags [ifs.ddArray [i].value], n);

			tagNames.Add (ifs.constTags [ifs.ddArray [i].value]);
		}

		int fillNum = DB.tags.Count;

		// Output File Setting で定めたこれから用意する系列名をハッシュに登録
		for (int i = 0; i < ofs.toggles.Count; i++) {
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
		for (int i = 0; i < ifs.step; i++) {
			for (int j = 0; j < ifs.fish; j++) {
				int l = 0;
				for (int k = 0; k < fillNum; k++) {
					while (ifs.ddArray [l].value == 0) {
						l++;
					}

					DB.data [i, j, k] = ifs.constData [i, j, l];
					l++;
				}
			}
		}

		// DataTypeを使ってデータを生成
		for (int i = 0; i < DB.step; i++) {
			for (int j = 0; j < DB.fish; j++) {
				for (int k = 0; k < DB.dataTypes.Count; k++) {
					DataType dataT = DB.dataTypes [k];
					DB.data [i, j, DB.tags [dataT.dataName]] = dataT.Eval (i, j);
				}
			}
		}


		StreamWriter sw = new StreamWriter (ProjectData.FileName.GetNameWithPath (ProjectData.FileKey.Output), false, Encoding.GetEncoding ("UTF-8"));
		sw.WriteLine ((DB.step - DB.start) + "," + DB.fish + "," + DB.constNums ["dt"]);

		string t = "";
		for (int i = 0; i < DB.fish; i++) {
			for (int j = 0; j < tagNames.Count; j++) {
				t += tagNames [i] + ",";
			}
			t += ",";
		}
		sw.WriteLine (t);

		for (int i = DB.start; i < DB.step; i++) {
			string s = "";
			for (int j = 0; j < DB.fish; j++) {
				for (int k = 0; k < DB.tags.Count; k++) {
					s += DB.data [i, j, k] + ",";
				}
				s += ",";
			}

			sw.WriteLine (s);
		}

		sw.Close ();

		
	}
}

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

		constNums.Add ("dt", dt);
		constNums.Add ("pi", 3.141592f);
	}
}