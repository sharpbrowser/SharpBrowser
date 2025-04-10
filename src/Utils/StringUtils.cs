using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrowser {
	internal static class StringUtils {
		public static string ConvertToString(this object o) {
			if (o is string) {
				return o as string;
			}
			return null;
		}
		public static bool CheckIfValid(this string text, bool trimAndCheck = false) {
			return text != null && text.Length > 0;
		}
		public static bool BeginsWith(this string str, string beginsWith, bool caseSensitive = true) {
			if (beginsWith.Length > str.Length) {
				return false;
			}
			if (beginsWith.Length == str.Length) {
				return String.Equals(beginsWith, str, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
			}
			return str.LastIndexOf(beginsWith, beginsWith.Length - 1, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) == 0;
		}
		public static string RemovePrefix(this string str, string prefix, bool caseSensitive = true) {
			if (str.Length >= prefix.Length && str.BeginsWith(prefix, caseSensitive)) {
				return str.Substring(prefix.Length);
			}
			return str;
		}
		public static string GetAfter(this string text, string find, int startAt = 0, bool returnAll = false, bool forward = true) {
			if (text == null) { return returnAll ? text : ""; }
			int idx;
			if (!forward) {
				idx = text.LastIndexOf(find, text.Length - startAt, StringComparison.Ordinal);
			}
			else {
				idx = text.IndexOf(find, startAt, StringComparison.Ordinal);
			}
			if (idx == -1) { return returnAll ? text : ""; }
			idx += find.Length;
			return text.Substring(idx);
		}

		public static string GetAfterLast(this string text, string find, bool returnAll = false) {
			int idx = text.LastIndexOf(find, StringComparison.Ordinal);
			if (idx == -1) {
				return returnAll ? text : "";
			}
			idx += find.Length;
			return text.Substring(idx);
		}

		public static string Join(this IList<string> values, string separator) {
			StringBuilder result = new StringBuilder();
			var last = values.Count - 1;
			for (int i = 0; i < values.Count; i++) {
				var str = values[i];
				if (str != null) {
					result.Append(str);
				}
				if (i != last) {
					result.Append(separator);
				}
			}
			return result.ToString();
		}
	}
}
