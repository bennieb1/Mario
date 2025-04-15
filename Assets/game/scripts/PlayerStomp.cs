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
            }
        }

        if (other.CompareTag("Shell"))
        {
            
            Destroy(other);
           PlayerController.Instance.Bounce();
        }
    }
}
