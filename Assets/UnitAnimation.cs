using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _gfx;
    private Vector3 _initialPosition;
    [SerializeField] private float _bobHeight;
    [SerializeField] private float _t;
    [SerializeField] private float _bobFrequency;
    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = _gfx.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime;
         float bobHeight = _initialPosition.y + (Mathf.Sin(_t/_bobFrequency * Mathf.PI)*_bobHeight+1f) ;
        _gfx.transform.localPosition = new Vector3(_initialPosition.x, bobHeight, _initialPosition.z);
        if (_t >2* Mathf.PI)
        {
            _t = 0;
        }
    }
}
