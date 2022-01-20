using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _unitSpacing = 1f;
    [SerializeField] private List<GameObject> _units = new List<GameObject>();
    [SerializeField] private GameObjectRuntimeCollection _currentUnitCollection;

    private Vector2 _input;
    private Vector2 _oldInput;
    private Quaternion _startRotation;

    private GameObject _leader;
    private Rigidbody _leaderRB;
    [SerializeField] private List<GameObject> _currentUnits = new List<GameObject>();
    [SerializeField] private List<PoseHistory> _unitPoses = new List<PoseHistory>();

    private float _countUp = 0f;
    private int _counter = 1;

    // Start is called before the first frame update
    void Start()
    {
        CreateBodyParts();
    }
    private void CreateBodyParts()
    {

        if (_currentUnits.Count ==0)
        {
            GameObject leader = Instantiate(_units[0], transform.position, transform.rotation, transform);
            _currentUnits.Add(leader);
            _currentUnitCollection.Add(leader);
            _leader = leader;
            _leaderRB =_leader.GetComponent<Rigidbody>();
            _units.RemoveAt(0);
            PoseHistory poseHistory = leader.GetComponent<PoseHistory>();
        }
        PoseHistory posHist = _currentUnits[_currentUnits.Count - 1].GetComponent<PoseHistory>();
        if (_countUp ==0f)
        {
            posHist.ResetList();
        }
        _countUp += Time.fixedDeltaTime;
        _counter++;
        if (_countUp >=_unitSpacing)
        {
            Debug.Log("CountUP =" + _countUp);
            _unitPoses.Add(posHist);
            GameObject instance = Instantiate(_units[0], posHist.History[0].Position, posHist.History[0].Rotation, transform);
            _currentUnits.Add(instance);
            _currentUnitCollection.Add(instance);
            _units.RemoveAt(0);
            instance.GetComponent<PoseHistory>().ResetList();
            _countUp = 0f;
            _counter = 0;
        }
        //Debug.Log("counter =" + _counter);
    }

    

    // Update is called once per frame
    void Update()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (_units.Count >0)
        {
            CreateBodyParts();
        }
        MoveParty();

        //Debug.Log("poselen = " + _unitPoses[0].History.Count);
    }


    private void MoveParty()
    { 
        _leaderRB.velocity = _leader.transform.forward * _moveSpeed * Time.deltaTime;

        float angle = 0f;
        if (_input != _oldInput)
        {
            _startRotation = _leader.transform.rotation;
        }
        if (_input.magnitude >= 0.1f)
        {
            angle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
            _leader.transform.rotation = Quaternion.Lerp(_startRotation, targetRotation, _turnSpeed * Time.deltaTime);
        }


        if (_currentUnits.Count > 1)
        {
            for (int i = 1; i < _currentUnits.Count; i++)
            {
                _currentUnits[i].transform.position = _unitPoses[i-1].History[0].Position; 
                _currentUnits[i].transform.rotation = _unitPoses[i - 1].History[0].Rotation;
                _unitPoses[i - 1].RemoveFirst();
                if (_unitPoses[i-1].History.Count>_unitSpacing/Time.fixedDeltaTime)
                {
                    _unitPoses[i - 1].ResetList();
                }
            }
        }
    }

}
