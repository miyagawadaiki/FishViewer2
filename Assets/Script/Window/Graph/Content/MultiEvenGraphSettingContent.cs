﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiEvenGraphSettingContent : GraphSettingContent {

	public override void Awake () {
		base.Awake ();
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();

		sm.CoverSettingElement (2, 0);
		sm.CoverSettingElement (3, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
