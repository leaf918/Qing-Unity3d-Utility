using UnityEngine;
using System.Collections.Generic;
public class Utility : Object {
	
	public static GameObject PickObjectWithCamerRay( Vector2 screenPos )
	{
		Ray		ray = Camera.main.ScreenPointToRay( screenPos );
		RaycastHit	hit;

		if ( Physics.Raycast( ray, out hit ) )
			return(hit.collider.gameObject);
		return(null);
	}
	
}