using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class UKVector3Extension {
	
	public static Vector3 SetX(this Vector3 v, float f)
	{
		return new Vector3(f, v.y, v.z);
	}

	public static Vector3 SetY(this Vector3 v, float f)
	{
		return new Vector3(v.x, f, v.z);
	}

	public static Vector3 SetZ(this Vector3 v, float f)
	{
		return new Vector3(v.x, v.y, f);
    }

    #region swizzle

    public static Vector2 SwizzleXZ(this Vector3 v) { return new Vector2(v.x, v.z); }
    public static Vector2 SwizzleXY(this Vector3 v) { return new Vector2(v.x, v.y); }
    
    public static Vector3 SwizzleXZ_(this Vector3 v) { return new Vector3(v.x, v.z, v.z); }
    public static Vector3 SwizzleX_Z(this Vector3 v) { return new Vector3(v.x, v.y, v.z); }

    
    public static Vector3 SwizzleX0Y(this Vector2 v) { return new Vector3(v.x, 0f, v.y); }

    
    #endregion
}
