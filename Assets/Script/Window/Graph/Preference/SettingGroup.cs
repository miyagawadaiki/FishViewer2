using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingGroup : MonoBehaviour {

	public int elementNum;
	public string sgName;

	protected char[] separator = { ' ', ',' };

	private List<RectTransform> elementList;
	private List<GameObject> coverPanelList;
	private GameObject coverPanelObj;

	protected string parameter = "", defParameter = "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,";

	protected virtual void Awake () {
		elementList = new List<RectTransform> ();

		foreach (Transform t in this.transform) {
			if (t.gameObject.GetComponent<Text> () == null) {
				elementList.Add (t.gameObject.GetComponent<RectTransform> ());
			}
		}

		elementNum = elementList.Count;

		sgName = "";

		coverPanelList = new List<GameObject> ();

		coverPanelObj = Resources.Load ("CoverPanel") as GameObject;
	}

	// Use this for initialization
	protected virtual void Start () {
		if (parameter.Equals (""))
			SetDefault ();
		else
			SetParameter ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CoverSettingElement (int element) {
		if (element >= elementNum)
			element = elementNum - 1;

		Instantiate (coverPanelObj, elementList [element]);
	}

	public void CoverSettingElementImmediately(int element) {
		if (element >= elementNum)
			element = elementNum - 1;

		GameObject obj = Instantiate (coverPanelObj, elementList [element]);
		Transform t = obj.transform.parent;
		obj.transform.parent = t.parent;
		coverPanelList.Add (obj);
	}

	public void DiscoverElement () {
		foreach (GameObject obj in coverPanelList)
			Destroy (obj);
	}

	public virtual void RegisterParameterText (string parameter) {
		this.parameter = parameter;
	}

	protected virtual void SetParameter () {

	}

	public virtual void SetDefault () {
		parameter = defParameter;
		SetParameter ();
	}

	public virtual string GetParameterText () {
		string ret = "";



		return ret;
	}
}
