using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TSystemRotator))]
[RequireComponent(typeof(TSystemConnector))]
[RequireComponent(typeof(TSystemBeltReceiver))]
public class Conveyor2 : MonoBehaviour
{
    public float Movespeed;

    private TSystemConnector connector;
    private TSystemBeltReceiver receiver;

    private void Awake()
    {
        connector = GetComponent<TSystemConnector>();
        receiver = GetComponent<TSystemBeltReceiver>();
    }

    private void FixedUpdate()
    {
        MoveItems();
    }

    private void MoveItems()
    {
        if (receiver.Items.Count == 0) { return; }

        Vector3 _moveOffset = new Vector3(Movespeed * Time.deltaTime, 0f, 0f);
        float _rightEdge = 0.5f;

        for (int i = 0; i < receiver.Items.Count; i++)
        {
            var _item = receiver.Items[i];
            float _currXPos = _item.transform.localPosition.x;
            float _inFrontXPos = 0f;

            // if not at the front, cache the pos of the item in front for spacing
            if (i != 0)
            {
                _inFrontXPos = receiver.Items[i - 1].transform.localPosition.x;
            }

            if (connector.CanOffloadItem(_item))
            {
                if (i == 0 && (_currXPos >= _rightEdge))
                {
                    connector.GetConnectedReceiver().Give(_item);
                    receiver.Items.Remove(_item);
                }

                if (i != 0 && ((_inFrontXPos - _currXPos) < receiver.gapWidth)) { continue; }
                _item.transform.localPosition += _moveOffset;
            }
            else
            {
                if (_currXPos >= (_rightEdge - (receiver.gapWidth * (i + 1)))) 
                { 
                    _rightEdge -= receiver.gapWidth;
                    continue;
                }

                if (i != 0 && ((_inFrontXPos - _currXPos) < receiver.gapWidth)) { continue; }
                _item.transform.localPosition += _moveOffset;
            }
        }
    }
}
