using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    private float _timeToDie = 10f;

    protected Rigidbody _rigidbody;
    protected float _livingTime;
    protected Unit _owner;
    protected bool _initialized;

    protected int _minDamage;
    protected int _maxDamage;
    
    void Awake()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        _rigidbody.velocity = Vector3.zero;
        _livingTime = 0;
        _initialized = false;
    }

    protected virtual void Update()
    {
        if (!_initialized) return;

        _livingTime += Time.deltaTime;
        if (_livingTime > _timeToDie)
        {
            SimplePool.Despawn(gameObject);
        }

        OnUpdate();
    }
    
    public virtual void Init(Unit owner)
    {
        if (_initialized) return;
        _owner = owner;
        _initialized = true;
        OnInit();
    }

    protected int GetDamage()
    {
        return Random.Range(_minDamage, _maxDamage);
    }
    
    protected abstract void OnInit();
    protected abstract void OnUpdate();

}