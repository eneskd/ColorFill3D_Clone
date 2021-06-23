using UnityEngine;

public static class ExtensionMethods
{
        public static Vector3 ToXZ(this Vector2 v)
        {
                return new Vector3(v.x, 0, v.y);
        }
        
        public static Vector3 ToXZ(this Vector2Int v)
        {
                return new Vector3(v.x, 0, v.y);
        }
}