using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour {


	[SerializeField]
	private Transform buttonContentTra = null;

	private GameObject buttonObj = null;

	private List<SettingGroup> settingGroupList;
	private string parameter;

	void Awake () {
		buttonObj = Resources.Load ("SettingGroupButton") as GameObject;

		settingGroupList = new List<SettingGroup> ();

		foreach (Transform t in buttonContentTra)
			Destroy (t.gameObject);

		foreach (SettingGroup sg in this.GetComponentsInChildren<SettingGroup>()) {
			settingGroupList.Add (sg);
		}

		parameter = "";
	}

	// Use this for initialization
	void Start () {
		if (!parameter.Equals (""))
			SetParameter ();

		foreach (SettingGroup sg in this.GetComponentsInChildren<SettingGroup>()) {
			GameObject obj = Instantiate (buttonObj, buttonContentTra) as GameObject;
			obj.GetComponent<Button> ().onClick.AddListener (() => sg.transform.SetAsLastSibling ());
			obj.GetComponentInChildren<Text> ().text = sg.sgName;
		}
			
		this.GetComponentInChildren<SettingGroup> ().transform.SetAsLastSibling ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CoverSettingElement (int group, int element) {
		settingGroupList [group].CoverSettingElement (element);
	}

	public void CoverSettingElementImmediately(int group, int element) {
		settingGroupList [group].CoverSettingElementImmediately (element);
	}

	public void RegisterParameterText (string parameter) {
		this.parameter = parameter;
	}

	public void SetParameter () {
		if (settingGroupList.Count == 1) {
			settingGroupList [0].RegisterParameterText (parameter);
			return;
		}

		Debug.Log (parameter);
		char[] separator = { ':', ' ', ',' };
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		int sum = 0;
		foreach (SettingGroup sg in settingGroupList) {
			string s = "";
			for (int i = 0; i < sg.elementNum; i++) {
				s += tmp [sum + i] + ",";
			}
			sg.RegisterParameterText (s);
			sum += sg.elementNum;
		}
	}

	public string GetParameterText () {
		string ret = "";

		if (settingGroupList.Count == 1)
			return settingGroupList [0].GetParameterText ();

		foreach (SettingGroup se in settingGroupList) {
			ret += se.GetParameterText () + ":";
		}

		Debug.Log ("SettingManager = " + ret);
		return ret;
	}
}
