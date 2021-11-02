using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proto_lineRenMover : MonoBehaviour
{
    [SerializeField] LineRenderer _lr;
    [SerializeField] Transform _aO;
    [SerializeField] Vector3 _offset;

    public List <Transform> lrPoints = new List<Transform>();
    void Start()
    {
        _lr.positionCount = 0;
        _offset = new Vector3(.15f, 1, 0);
    }

    // Update is called once per frame
    void StartTrail()
    {
        _lr.positionCount = 5;
        _lr.SetPosition(0, _aO.position);
        _lr.SetPosition(1, _aO.position + _offset);
        _lr.SetPosition(2, _aO.position + _offset * 2);
        _lr.SetPosition(3, _aO.position + _offset * 3);
        _lr.SetPosition(4, _aO.position + _offset * 4);
    }
}
