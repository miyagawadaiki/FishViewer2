using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphTypeSetting : Setting {

	//[SerializeField]
	private Dropdown graphType;
	private SourceSetting ss;

	void Awake() {
		graphType = elements [0].gameObject.GetComponent<Dropdown> ();
		ss = this.transform.parent.GetComponentInChildren<SourceSetting> ();

		graphType.onValueChanged.AddListener (i => CoverFish (i));
	}

	// Use this for initialization
	void Start () {
		if (gc.memo != null)
			graphType.value = (int)gc.memo.graphType;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CoverFish (int i) {
		if (i == 1)
			ss.CoverElement (1);
		else
			ss.DiscoverElement ();
	}

	/*
	public override void SetMemoData() {
		graphType.value = (int)gc.mGraphType;
	}
	*/

	public GraphType GetGraphType() {
		return (GraphType)graphType.value;
	}

	public override string GetData() {
		return graphType.value + ",";
	}
}
