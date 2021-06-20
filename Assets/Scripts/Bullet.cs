using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    private Robot target;

    internal void Seek(Robot t)
    {
        target = t;
    }
    
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.001f)
        {
            target.Damage();
            Destroy(gameObject);
        }
    }
}
