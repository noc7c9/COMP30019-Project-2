using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class UnitMovement : MonoBehaviour {

    NavMeshAgent agent;
    Alignment alignment;

    Vector3? destination;

    void Start() {
        alignment = GetComponent<Alignment>();
        agent = GetComponent<NavMeshAgent>();
        destination = GetNearestEnemyCore();
    }

    void Update() {
        if (destination.HasValue) {
            if (ReachedDestination()) {
                //destination = GenerateRandomDestination();
            }
            agent.SetDestination(destination.Value);
            //Debug.DrawLine(transform.position, destination, Color.black);
            //Debug.DrawLine(transform.position, agent.steeringTarget, Color.blue);
        }
    }

    bool ReachedDestination() {
        if (destination.HasValue) {
            return Vector3.Distance(transform.position, destination.Value) <= agent.stoppingDistance;
        } else {
            return true;
        }
    }

    Vector3 GenerateRandomDestination() {
        float x = Random.Range(-50, 50);
        float z = Random.Range(-50, 50);
        return new Vector3(x, 0, z);
    }

    Vector3? GetNearestEnemyCore() {
        CoreController closest = null;
        float minDist = Mathf.Infinity;
        foreach (CoreController core in GameObject.FindObjectsOfType<CoreController>()) {
            if (core.GetComponent<Alignment>().IsEnemyTo(alignment)) {
                float dist = (core.transform.position - transform.position).sqrMagnitude;
                if (dist < minDist) {
                    minDist = dist;
                    closest = core;
                }
            }
        }
        if (closest) {
            return closest.transform.position;
        } else {
            return null;
        }
    }

    public void SetDestination(Vector3 dest) {
        destination = dest;
    }

}