using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;

    private Robot target;
    private Turret turret;

    internal void SetTarget(Turret t)
    {
        turret = t;
        target = turret.Target;
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
            Destroy(gameObject);
            target.Damage();
            if (turret.Target == target)
                turret.Target = null;
        }
    }
}
