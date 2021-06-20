using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    
    private Executor executor;
    private LevelLoader levelLoader;

    private float lastShoot;
    private Vector3 position;
    
    internal int Id { get; set; }
    internal Robot Target { get; set; }

    internal readonly float Range = 20f;
    internal readonly float FireDelay = 5f;
    internal readonly float RotationSpeed = 100f;

    void Awake()
    {
        executor = FindObjectOfType<Executor>();
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    void Start()
    {
        lastShoot = Time.time;
        position = transform.position;
    }

    void Update()
    {
        if (Target != null)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) > Range)
            {
                Target = null;
                return;
            }

            transform.LookAt(Target.transform.position);
            
            if (Time.time - lastShoot > FireDelay)
            {
                Shoot();
                lastShoot = Time.time;
            }
        }
    }

    void Shoot()
    {
        var go = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bullet = go.GetComponent<Bullet>();
        bullet.SetTarget(this);
    }

    void OnDestroy()
    {
        executor.StopExecution(gameObject);
    }

    public object X(object[] _)
    {
        return position.x;
    }

    public object Y(object[] _)
    {
        return position.z;
    }

    public object Shoot(object[] args)
    {
        Target = levelLoader.Level.Robots[(int)args[0]];
        return null;
    }
}
