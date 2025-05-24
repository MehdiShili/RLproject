using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class PoliceAgent: Agent
{
    
    [SerializeField] private Transform targetTransform;
    public override void OnEpisodeBegin(){
        transform.localPosition = new Vector3(Random.Range(-12, 0), 0, Random.Range(-8, 6));
        targetTransform.localPosition = new Vector3(Random.Range(6, 13), 0, Random.Range(-8, 7));
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float moveSpeed = 5f;
        transform.localPosition+=new Vector3(moveX, 0, moveZ) * Time.deltaTime*moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Criminal")){
            SetReward(1f);
            EndEpisode();
        }
        if (other.gameObject.CompareTag("Wall")){
            SetReward(-1f);
            EndEpisode();
        }
    }
}
