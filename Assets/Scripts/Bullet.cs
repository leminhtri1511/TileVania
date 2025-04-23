using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private GameObject bullet;

    private Rigidbody2D _myRigidbody;
    private PlayerMovement _player;
    private float _bulletDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerMovement>();
        _bulletDirection = _player.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _myRigidbody.velocity = new Vector2(_bulletDirection, 0f);
        Destroy(bullet.gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}