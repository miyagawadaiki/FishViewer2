using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour {

	public GraphContent gc;
	public GraphContentType type;
	public GameObject coverPanelObj;
	public List<RectTransform> elements = new List<RectTransform>();
	public List<GameObject> coverPanelList = new List<GameObject> ();

	/*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	*/

	public virtual void CoverElement(int idx) {
		if (idx >= elements.Count)
			idx = elements.Count - 1;

		GameObject obj = Instantiate (coverPanelObj, elements [idx]);
		Transform t = obj.transform.parent;
		obj.transform.parent = t.parent;
		coverPanelList.Add (obj);
	}

	public virtual void DiscoverElement() {
		foreach (GameObject obj in coverPanelList)
			Destroy (obj);
	}

	/*
	public virtual void SetMemoData() {

	}
	*/

	public virtual string GetData() {
		return "";
	}
}
