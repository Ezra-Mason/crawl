using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySnakeMovement : MonoBehaviour
{
    private Vector2 _input;
    private Vector2 _oldInput;
    private Quaternion _startRotation;
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private List<Pose> _poseHistory;
    //[SerializeField] private float _unitSpacing = 1f;
    [Header("Party Composition")]
    [SerializeField] private int _space = 100;
    [SerializeField] private List<GameObject> _party = new List<GameObject>();
    [SerializeField] private List<GameObject> _currentParty = new List<GameObject>();
    private GameObject _leader;
    private Rigidbody _leaderRB;
    private bool _doneSetUp = false;
    //[SerializeField] private float _dx;
    //[SerializeField] private int _pointsPerUnitSpace;

    // Start is called before the first frame update
    void Start()
    {
        SetUpParty();
    }


    private void SetUpParty()
    {
        //numer of positions between each unit is (0.02f*_moveSpeed)/_unitSpacing
        //_dx  = _unitSpacing/ (0.02f * _moveSpeed);
        //_pointsPerUnitSpace = Mathf.RoundToInt(_dx);
        _poseHistory = new List<Pose>();
        for (int i = 0; i < _party.Count; i++)
        {
            GameObject instance = Instantiate(_party[i], transform.position , Quaternion.identity, transform);
            _currentParty.Add(instance);
        }

        _leader = _currentParty[0];
        if (_leader.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            _leaderRB = rb;
        }
        else
        {
            Debug.Log("failed to get leaders rb");
        }
        _doneSetUp = true;
    }


    // Update is called once per frame
    void Update()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

    }


    private void FixedUpdate()
    {
        if (!_doneSetUp)
            return;

        _leaderRB.velocity = _leader.transform.forward * _moveSpeed * Time.fixedDeltaTime;

        //leader turning
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

        //update the logs with the leaders pose
        _poseHistory.Insert(0,new Pose(_currentParty[0].transform.position, _currentParty[0].transform.rotation));

        //make the rest of the party follow behind the leaders pose history
        if (_currentParty.Count > 1)
        {
            for (int i = 1; i < _currentParty.Count; i++)
            {
                //if there isnt a pose for the correct spacing away, take the last pose available
                int index = Mathf.Min(i * _space, _poseHistory.Count-1);
                _currentParty[i].transform.position = _poseHistory[index].Position;
                _currentParty[i].transform.rotation = _poseHistory[index].Rotation;
            }
            
            //remove poses when the last unit has used them to avoid an always growing list
            if (_poseHistory.Count  > _currentParty.Count * _space)
            {
                _poseHistory.RemoveAt(_poseHistory.Count - 1);
            }
        }
    }

}
