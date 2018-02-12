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
//		Debug.Log ("SimulationManager.Init ()");
		foreach (Transform t in content)
			Destroy (t.gameObject);

		for (int i = 1; i < ProjectData.DefaultData.defaultGraphTexts.Length; i++) {
			GameObject obj = Instantiate (shortcutNode, content) as GameObject;
			string hoge = ProjectData.DefaultData.defaultGraphTexts [i];
			obj.GetComponent<Button> ().onClick.AddListener (() => mwm.AddWindow (hoge));
			ProjectData.DefaultData.shortcutTexts.Add (hoge);
			obj.GetComponentInChildren<Text> ().text = ProjectData.DefaultData.defaultGraphNames [i];
		}

		char[] sep = { '@' };
		foreach (string s in DataBase.shortcutList) {
			GameObject obj = Instantiate (shortcutNode, content) as GameObject;
			string[] tmp = s.Split (sep, System.StringSplitOptions.RemoveEmptyEntries);
			obj.gameObject.GetComponent<Button> ().onClick.AddListener (() => mwm.AddWindow (tmp[1]));
			ProjectData.DefaultData.shortcutTexts.Add (tmp[1]);
			obj.gameObject.GetComponentInChildren<Text> ().text = tmp[0];
		}

		mbc.UpdateButtonImage ();


		StartCoroutine (AddWindowCoroutine ());

		fileNameText.text = ProjectData.FileName.GetName (ProjectData.FileKey.Read);
	}

	private IEnumerator AddWindowCoroutine () {
		yield return null;

		mwm.AddWindow (ProjectData.DefaultData.defaultGraphTexts [0]);
		mwm.GetLastWindowController ().gameObject.GetComponentInChildren<MyWindowContent> ().defaultSize = new Vector2 (1f, 1f) * (Screen.height - 100);
		mwm.GetLastWindowController ().gameObject.GetComponentInChildren<GraphContent> ().SetGridMode (ViewMode.ShowGridCompletely);

		yield break;
	}
}
