using System.Collections;
using System.Collections.Generic;

namespace ProjectData {
	public enum FileKey {
		Read,
		Write,
	}

	public class FileName {

		private static string[,] names = new string[2,System.Enum.GetNames(typeof(FileKey)).Length];
		private static char[] separator = { '/', '\\' };

		public static void Set(FileKey key, string path, string name) {
			names [0, (int)key] = path;
			names [1, (int)key] = name;
			UnityEngine.PlayerPrefs.SetString (System.Enum.GetName(typeof(FileKey), key), path + "/" + name);
		}

		public static string GetPath(FileKey key) {
			if (names [0, (int)key] == null) {
				string line = UnityEngine.PlayerPrefs.GetString (System.Enum.GetName (typeof(FileKey), key), "C:/Users/Daiki/Desktop/aaa.csv");
				string[] tmp = line.Split (separator);
				names [0, (int)key] = tmp [0];
				for (int i = 1; i < tmp.Length - 1; i++)
					names [0, (int)key] += "/" + tmp [i];
			}
			return names[0, (int)key];
		}

		public static string GetName(FileKey key) {
			if (names [1, (int)key] == null) {
				string line = UnityEngine.PlayerPrefs.GetString (System.Enum.GetName (typeof(FileKey), key), "C:/Users/Daiki/Desktop/aaa.csv");
				string[] tmp = line.Split (separator);
				names [1, (int)key] = tmp [tmp.Length - 1];
			}
			return names [1, (int)key];
		}

		public static string GetNameWithPath(FileKey key) {
			return GetPath (key) + "/" + GetName (key);
		}
	}
}