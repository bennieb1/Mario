using UnityEngine;

public class BulletCleanup : MonoBehaviour
{
    void Update()
    {
        // Convert world position to viewport coords
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        // vp.z < 0 means behind camera, vp.x/y outside [0,1] means off‐screen
        if (vp.z < 0f || vp.x < 0f || vp.x > 1f || vp.y < 0f || vp.y > 1f)
        {
            Destroy(gameObject);
        }
    }
}
