using UnityEngine;

public static class CameraPositionHelper
{
    public static Vector2 GetScreenPositionInPercent(this Transform obj)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(obj.position);
        return new Vector2(viewportPosition.x * 100f, viewportPosition.y * 100f);
    }
    
    public static void MoveToScreenPercent(this Transform obj, Vector2 percent, float distance = 10f)
    {
        Vector3 viewportPos = new Vector3(percent.x / 100f, percent.y / 100f, distance);
        obj.position = Camera.main.ViewportToWorldPoint(viewportPos);
    }
}