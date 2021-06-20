using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    
    private Executor executor;
    private LevelLoader levelLoader;

    private Robot target;
    private float lastShoot;
    private Vector3 position;
    
    internal int Id { get; set; }

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
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > Range)
            {
                target = null;
                return;
            }

            transform.LookAt(target.transform.position);
            
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
        bullet.Seek(target);
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
        if (levelLoader.Level.Robots.ContainsKey((int)args[0]))
            target = levelLoader.Level.Robots[(int)args[0]];
        return null;
    }
}
