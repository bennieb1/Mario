using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFollow : MonoBehaviour
{
    [Header("Waypoints (children)")]
    [Tooltip("Empty child Transforms marking the path")]
    public List<Transform> points = new List<Transform>();

    [Tooltip("If true, loops; otherwise ping‑pongs back and forth")]
    public bool loop = false;

    // Internal state
    private int currentIndex = 0;
    private bool goingForward = true;

    /// <summary>Current target waypoint</summary>
    public Transform CurrentPoint => (points.Count > 0) ? points[currentIndex] : null;

    /// <summary>Auto‑fill points from direct children</summary>
    void Reset()
    {
        points.Clear();
        foreach (Transform child in transform)
            points.Add(child);
    }

    /// <summary>Call to advance to the next waypoint</summary>
    public void AdvancePoint()
    {
        if (points.Count < 2) return;

        if (loop)
        {
            currentIndex = (currentIndex + 1) % points.Count;
        }
        else
        {
            if (goingForward)
            {
                if (currentIndex < points.Count - 1) currentIndex++;
                else { goingForward = false; currentIndex--; }
            }
            else
            {
                if (currentIndex > 0) currentIndex--;
                else { goingForward = true; currentIndex++; }
            }
        }
    }

    /// <summary>Draws spheres & lines in the Scene view for each waypoint</summary>
    void OnDrawGizmos()
    {
        if (points == null || points.Count < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < points.Count; i++)
        {
            Transform p = points[i];
            if (p == null) continue;

            Gizmos.DrawSphere(p.position, 0.1f);

            Transform next = (i + 1 < points.Count) ? points[i + 1] : (loop ? points[0] : null);
            if (next != null)
                Gizmos.DrawLine(p.position, next.position);
        }
    }
}
