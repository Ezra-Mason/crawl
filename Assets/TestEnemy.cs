using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private GameObjectRuntimeCollection _currentUnits;
    private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private float _radius;
    // Start is called before the first frame update
    void Start()
    {
        _target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentUnits.Count()>0)
        {
            _target = _currentUnits.List()[Random.Range(0, _currentUnits.Count())].transform;

        }
        if (_target!=null)
        {
            if (Vector3.Distance(_target.position, transform.position) > _radius)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
            }

        }
    }

    public void Damage(int amount)
    {

    }
}
