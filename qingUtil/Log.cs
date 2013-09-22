using UnityEngine;
using System.Collections;

public class Log : Object {
	public static void Trace( params object[] v )
	{
		string o = "~~";
		for ( int i = 0; i < v.Length; i++ )
		{
			if(o.Length>2)o	+= ",";
			o	+= v[i].ToString();
		}
		Debug.Log( o );
	}
}
