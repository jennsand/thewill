using UnityEngine;
using System.Collections;

namespace myExtensions
{
	public static class vector3Extension
	{
		public static Vector2 XY (this Vector3 v) {
			return new Vector2 (v.x, v.y);
		}
	}
}