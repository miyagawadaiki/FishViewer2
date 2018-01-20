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
				string line = UnityEngine.PlayerPrefs.GetString (System.Enum.GetName (typeof(FileKey), key), "C:/Users/aaa.csv");
				string[] tmp = line.Split (separator);
				names [0, (int)key] = tmp [0];
				for (int i = 1; i < tmp.Length - 1; i++)
					names [0, (int)key] += "/" + tmp [i];
			}
			return names[0, (int)key];
		}

		public static string GetName(FileKey key) {
			if (names [1, (int)key] == null) {
				string line = UnityEngine.PlayerPrefs.GetString (System.Enum.GetName (typeof(FileKey), key), "C:/Users/aaa.csv");
				string[] tmp = line.Split (separator);
				names [1, (int)key] = tmp [tmp.Length - 1];
			}
			return names [1, (int)key];
		}

		public static string GetNameWithPath(FileKey key) {
			return GetPath (key) + "/" + GetName (key);
		}
	}

	public class ColorList {

		public static UnityEngine.Color[] colors = {
			new UnityEngine.Color(1f, 0f, 0f),						// red
			new UnityEngine.Color(1f, 96f / 255f, 0f),				// orange
			new UnityEngine.Color(1f, 1f, 0f),						// yellow
			new UnityEngine.Color(0f, 1f, 0f),						// yello-green
			new UnityEngine.Color(0f, 163f / 255, 40f / 255f),		// green
			new UnityEngine.Color(0f, 224f / 255f, 1f),				// light-blue
			new UnityEngine.Color(0f, 32f / 255f, 1f),				// blue
			new UnityEngine.Color(28f / 255f, 0f, 88f / 255f),		// deap-blue
			new UnityEngine.Color(149f / 255f, 0f, 183f / 255f),	// purple
			new UnityEngine.Color(1f, 0f, 146f / 255f),				// pink
		};

		public static string[] names = {
			"Red",
			"Orange",
			"Yellow",
			"Light Green",
			"Green",
			"Light Blue",
			"Blue",
			"Deep Blue",
			"Purple",
			"Pink",
		};

		public static int Count() {
			return colors.Length;
		}

		public static UnityEngine.Color GetColor(int i) {
			return colors [i % colors.Length];
		}

	}
}