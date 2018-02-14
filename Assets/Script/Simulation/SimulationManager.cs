using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour {

	[SerializeField]
	private RectTransform content = null;
	[SerializeField]
	private GameObject shortcutNode = null;
	[SerializeField]
	private Text fileNameText = null;

	private MyWindowManager mwm;
	private MenuBarController mbc;

	void Awake () {
		DataBase.simuMan = this;
		mwm = GameObject.Find ("MyWindowManager").GetComponent<MyWindowManager> ();
		mbc = GameObject.Find ("MenuBar").GetComponent<MenuBarController> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init () {
		// Shortcutメニュー直下を初期化
		foreach (Transform t in content)
			Destroy (t.gameObject);

		// ProjectData.DefaultDataからデフォルトのショートカットを読み込み
		ProjectData.DefaultData.shortcutTexts.Add (ProjectData.DefaultData.defaultGraphTexts [0]);	// PolarPosはメニューに追加しない
		for (int i = 1; i < ProjectData.DefaultData.defaultGraphTexts.Length; i++) {
			GameObject obj = Instantiate (shortcutNode, content) as GameObject;
			string hoge = ProjectData.DefaultData.defaultGraphTexts [i];
			obj.GetComponent<Button> ().onClick.AddListener (() => mwm.AddWindow (hoge));
			ProjectData.DefaultData.shortcutTexts.Add (hoge);
			obj.GetComponentInChildren<Text> ().text = i + ". " + ProjectData.DefaultData.defaultGraphNames [i];
		}

		int j = ProjectData.DefaultData.defaultGraphNames.Length;
		char[] sep = { '@' };
		foreach (string s in DataBase.shortcutList) {
			GameObject obj = Instantiate (shortcutNode, content) as GameObject;
			string[] tmp = s.Split (sep, System.StringSplitOptions.RemoveEmptyEntries);
			obj.gameObject.GetComponent<Button> ().onClick.AddListener (() => mwm.AddWindow (tmp[1]));
			ProjectData.DefaultData.shortcutTexts.Add (tmp[1]);
			obj.gameObject.GetComponentInChildren<Text> ().text = j + ". " + tmp[0];
			j++;
		}

		mbc.UpdateButtonImage ();

		char[] paramSep = { '/' };
		string[] foo = ProjectData.DefaultData.defaultGraphTexts [0].Split (paramSep, System.StringSplitOptions.RemoveEmptyEntries);
		if (!Simulation.isEnabled || !Simulation.HasGraphContent ((GraphContentType)System.Enum.Parse (typeof (GraphContentType), foo [0].Substring (0, foo [0].Length - 5)), foo [1]))
			StartCoroutine (AddWindowCoroutine ());

		fileNameText.text = ProjectData.FileName.GetName (ProjectData.FileKey.Read);
	}

	private IEnumerator AddWindowCoroutine () {
		yield return null;

		mwm.AddWindow (ProjectData.DefaultData.defaultGraphTexts [0]);
		mwm.GetLastWindowController ().gameObject.GetComponentInChildren<MyWindowContent> ().defaultSize = new Vector2 (1f, 1f) * (Screen.height * 0.6f);
		mwm.GetLastWindowController ().gameObject.GetComponentInChildren<GraphContent> ().SetGridMode (ViewMode.ShowGridCompletely);

		yield break;
	}
}
