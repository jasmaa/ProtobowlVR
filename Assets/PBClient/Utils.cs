using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

public static class Utils {

	public static List<int> convertJSONToList(JSONArray arr){
		// converts json array to int list

		List<int> retList = new List<int> ();
		foreach (JSONNode element in arr) {
			retList.Add (element.AsInt);
		}
		return retList;
	}

	public static bool containsKey(string target, JSONNode dict){
		// Checks if JSON node contains key

		foreach (var key in dict.Keys) {
			if(target.Equals(key)){
				return true;
			}
		}
		return false;
	}

	public static JSONNode MergeDict(JSONNode src, JSONNode addition){
		// Merge two JSONNodes precedenting addition

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
