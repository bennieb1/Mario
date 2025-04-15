using UnityEngine;

public class Turtle : MonoBehaviour
{

    [Tooltip("Shell to spawn when stomped")]
    public GameObject shellPrefab;
    
    private bool stomped = false;
    
    public void turnToShell()
    {
        if (stomped) return;
        
      stomped = true;

      if (shellPrefab != null)
      {
          Instantiate(shellPrefab,transform.position,transform.rotation);
          
            Destroy(gameObject);
      }
    }
}
