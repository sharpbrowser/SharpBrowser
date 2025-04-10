
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Newtonsoft.Json;
using SharpBrowser.Utils;

namespace SharpBrowser.Managers {

	/// <summary>
	/// App config manager which saves app-level settings in a JSON file.
	/// </summary>
	public static class ConfigManager {

		public static bool InitDone;
		public static string AppDataPath;
		private static string SettingsFilePath;
		private static Dictionary<string, object> SettingsDict;
		private static bool SettingsChanged = false;
		private static Timer SaveTimer;

		/// <summary>
		/// Loads existing settings from the AppData folder if they exist.
		/// Starts a timer that saves settings every second if changes have occurred.
		/// </summary>
		public static void Init(string appName) {
			if (SettingsDict == null) {
				AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName);
				AppDataPath.EnsureFolderExists();
				SettingsFilePath = Path.Combine(AppDataPath, "settings.json");

				SettingsDict = LoadSettings();
				InitDone = true;

				SaveTimer = new Timer(1000);
				SaveTimer.Elapsed += SaveTimerElapsed;
				SaveTimer.Start();
			}
		}

		/// <summary>
		/// Returns the setting for the given key, with an optional default value
		/// </summary>
		public static object Get(string key, object defaultVal = null) {
			if (SettingsDict == null) throw new Exception("You need to call ZConfig.Init() first!");
			if (SettingsDict.ContainsKey(key)) return SettingsDict[key];
			return defaultVal;
		}

		/// <summary>
		/// Returns the integer setting for the given key
		/// </summary>
		public static int GetInt(string key, int defaultVal = 0) {
			var val = Convert.ToInt32(Get(key, defaultVal));
			return val == 0 ? defaultVal : val;
		}

		/// <summary>
		/// Returns the double setting for the given key
		/// </summary>
		public static double GetDouble(string key, double defaultVal = 0.0) {
			var val = Convert.ToDouble(Get(key, defaultVal));
			return val == 0 ? defaultVal : val;
		}

		/// <summary>
		/// Returns the string setting for the given key
		/// </summary>
		public static string GetString(string key, string defaultVal = "") {
			var value = Get(key, defaultVal);
			if (value == null) return defaultVal;
			return value is string valString ? valString : value.ToString();
		}

		/// <summary>
		/// Returns the boolean setting for the given key
		/// </summary>
		public static bool GetBool(string key, bool defaultVal = false) {
			var val = GetString(key, defaultVal.ToString());
			return val == "true" || val == "1";
		}

		/// <summary>
		/// Saves a setting by key, and persists it to disk within 1 second.
		/// </summary>
		public static void Set(string key, object value) {
			if (SettingsDict == null) throw new Exception("Call ConfigManager.Init() first!");
			SettingsDict[key] = value;
			SettingsChanged = true;
		}

		/// <summary>
		/// Returns the settings from the JSON file or an empty dictionary if the file doesn't exist
		/// </summary>
		private static Dictionary<string, object> LoadSettings() {
			if (File.Exists(SettingsFilePath)) {
				var json = File.ReadAllText(SettingsFilePath);
				return JsonConvert.DeserializeObject<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
			}
			return new Dictionary<string, object>();
		}

		/// <summary>
		/// Saves the settings to the JSON file
		/// </summary>
		public static void SaveSettings() {
			var json = JsonConvert.SerializeObject(SettingsDict, Formatting.Indented);
			File.WriteAllText(SettingsFilePath, json);
			SettingsChanged = false;
		}

		/// <summary>
		/// Timer event handler that checks if settings have changed and saves them
		/// </summary>
		private static void SaveTimerElapsed(object sender, ElapsedEventArgs e) {
			if (SettingsChanged) {
				SaveSettings();
			}
		}
	}

}
