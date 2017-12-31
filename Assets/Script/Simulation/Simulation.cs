using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation {

	public static int step;

	private static List<GraphManager> graphList;

	public static void Init() {
		step = 0;

		graphList = new List<GraphManager> ();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Graph")) {
			graphList.Add(obj.GetComponent<GraphManager>());
		}
	}

	public static void Reset() {
		step = 0;
	}

	public static bool IsEnd() {
		return step >= DataBase.step;
	}

	public static void Execute(int step) {
		foreach (GraphManager gm in graphList) {
			gm.Plot (step);
		}
	}

	public static void GoNext() {
		Execute (step);
		step++;
	}

	public static void GoBack() {
		Execute (step);
		step--;
	}
}
