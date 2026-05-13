using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private HealthSystem targetHealthSys;
    private void Start()
    {
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetHealthSys = collision.gameObject.GetComponent<HealthSystem>())
        {
            targetHealthSys = collision.gameObject.GetComponent<HealthSystem>();
            targetHealthSys.Die();
        }
    }
   

}
