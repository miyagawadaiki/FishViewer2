using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphTypeSettingGroup : SettingGroup {

	private Dropdown graphTypeDD;
	private SourceSettingGroup ssg;

	protected override void Awake () {
		base.Awake ();

		sgName = "Graph Type";

		graphTypeDD = this.GetComponentInChildren<Dropdown> ();

		ssg = this.transform.parent.GetComponentInChildren<SourceSettingGroup> ();
	}

	// Use this for initialization
	protected override void Start () {
		graphTypeDD.ClearOptions ();
		foreach (string name in System.Enum.GetNames(typeof(GraphType)))
			graphTypeDD.options.Add (new Dropdown.OptionData {text = name});
		graphTypeDD.value = 1;	graphTypeDD.value = 0;

		graphTypeDD.onValueChanged.AddListener (value => ssg.CoverXType (value));

		defParameter = "0 ";

		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected override void SetParameter () {
		string[] tmp = parameter.Split (separator, System.StringSplitOptions.RemoveEmptyEntries);

		graphTypeDD.value = int.Parse (tmp [0]);
	}

	public override string GetParameterText () {
		string ret = "";

		ret += graphTypeDD.value + " ";

		return ret;
	}
}
