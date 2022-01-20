using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyAttack : MonoBehaviour
{
    [SerializeField] private GameObjectRuntimeCollection _currentUnits;
    [SerializeField] private bool _canAttack;
    [SerializeField] private float _timeBetweenAttack;

    // Start is called before the first frame update
    void Start()
    {
        _canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        _canAttack = false;
        List<GameObject> units =_currentUnits.List();
        for (int i = 0; i < _currentUnits.Count(); i++)
        {
            if (units[i].TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
            {
                playerUnit.Attack();
            }
            yield return new WaitForSeconds(_timeBetweenAttack);
        }
        _canAttack = true;
        yield return null;
    }
}
