using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

/// <summary>
/// Utils for processing data
/// </summary>
public static class Utils {

	/// <summary>
	/// Convert JSONArray to int List
	/// </summary>
	/// <param name="arr">JSONArray to convert</param>
	public static List<int> convertJSONToList(JSONArray arr){

		List<int> retList = new List<int> ();
		foreach (JSONNode element in arr) {
			retList.Add (element.AsInt);
		}
		return retList;
	}

	/// <summary>
	/// Check if JSONNode contains string key
	/// </summary>
	/// <param name="target">Target key to look for</param>
	/// <param name="dict">JSONNode to search through</param>
	public static bool containsKey(string target, JSONNode dict){

		foreach (var key in dict.Keys) {
			if(target.Equals(key)){
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Merge two JSONNodes, preferring new mappings over old
	/// </summary>
	/// <param name="src">Original node to add to</param>
	/// <param name="addition">Node updating src node</param>
	public static JSONNode MergeDict(JSONNode src, JSONNode addition){

		JSONNode ret = JSON.Parse ("{}");
		foreach (var key in src.Keys) {
			ret.Add (key, src[key]);
		}
		foreach (var key in addition.Keys) {
			ret.Add (key, addition[key]);
		}
		return ret;
	}
}
