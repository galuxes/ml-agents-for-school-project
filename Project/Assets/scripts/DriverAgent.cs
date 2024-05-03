using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DriverAgent : Agent
{
    [SerializeField] private CarController _controller;
    private List<GameObject> _gates;
    private int nextCheckPointIndex = 0;
    private Vector3 _spawnPosition;
    private Quaternion _spawnRotation;

    private void Start()
    {
        _gates = GateManager._instance.gates;
        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = _spawnPosition;
        transform.rotation = _spawnRotation;
        nextCheckPointIndex = 0;
        _controller.Stop();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float checkpointDistance = (transform.position - _gates[nextCheckPointIndex].transform.position).magnitude;
        sensor.AddObservation(checkpointDistance);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = 0, turn = 0;

        switch (actions.DiscreteActions[0])
        {
            case 0: move = 0; break;
            case 1: move = 1; break;
            case 2: move = 1; break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0: turn = 0; break;
            case 1: turn = 1; break;
            case 2: turn = -1; break;
        }

        //Debug.Log(move);

        _controller.SetInputs(move, turn);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            forwardAction = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            forwardAction = 2;
        }

        int turnAction = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            turnAction = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turnAction = 2;
        }

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            if (other.gameObject == _gates[nextCheckPointIndex])
            {
                AddReward(1);
                nextCheckPointIndex = (nextCheckPointIndex + 1) % _gates.Count;
            }
            else
            {
                AddReward(-1);
            }
        }

        if (other.gameObject.CompareTag("wall"))
        {
            AddReward(-0.5f);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            AddReward(-0.1f);
        }
    }
}
