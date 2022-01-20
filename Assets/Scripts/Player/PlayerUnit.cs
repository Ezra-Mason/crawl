using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _bulletPrefab;
    private List<GameObject> _bulletPool;
    [SerializeField] private Transform _left;
    [SerializeField] private Transform _right;
    private int _poolSize = 20;
    [SerializeField] private bool _shootRight;
    private Vector3 _target;
    [SerializeField] private float _shootForce;
    [SerializeField] private float _killTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            _target = hit.point;
            Debug.DrawLine(ray.origin, _target, Color.cyan);
        }
        Vector3 toMouse = _target - transform.position;
        toMouse.y = 0f;
        Vector3 forward = transform.TransformDirection(transform.forward);
        forward.y = 0f;
        Vector3 cross = Vector3.Cross(forward.normalized, toMouse.normalized);
        if (Vector3.Distance(_right.transform.position, _target) > Vector3.Distance(_left.transform.position, _target))
        {
            _shootRight = true;
        }
        else
        {
            _shootRight = false;
        }
    }

    public void Attack()
    {
        Vector3 point = !_shootRight ? _right.position : _left.position;
        GameObject instance = Instantiate(_bulletPrefab, point, transform.rotation);
        if (instance.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 direction = (point - transform.position).normalized;
            rb.AddForce(direction * _shootForce);
        }
        //Invoke("instnace.Destroy",_killTime);
    }
}
