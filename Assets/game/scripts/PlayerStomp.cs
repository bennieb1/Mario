using Unity.VisualScripting;
using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Turtle"))
        {
            var turtle = other.GetComponent<Turtle>();
            if(turtle != null)
            {
                turtle.turnToShell();
               PlayerController.Instance.Bounce();
               GameManager.Instance.AddScore(200);
            }
        }

        if (other.CompareTag("Shell"))
        {
            GameManager.Instance.AddScore(200);
            Destroy(other);
           PlayerController.Instance.Bounce();
         
        }
        
        if(other.CompareTag("Bullet"))
        {
            GameManager.Instance.AddScore(200);
            Destroy(other.gameObject);
            PlayerController.Instance.Bounce();
            
        }
    }
}
