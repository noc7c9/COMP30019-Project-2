using UnityEngine;
using System.Collections;

public class UnitWeaponry : MonoBehaviour {

    public BasePayload payload;

    public float range = 35;

    public float fireCooldownMax = 1f;
    float fireCooldown = 0;

    GameObject target;
    Alignment alignment;

    void Start() {
        alignment = GetComponent<Alignment>();
    }
	
	void Update() {
        if (!target || !IsTargetInRange(target)) {
            target = GetNearestEnemyTarget();
            if (!target) {
                return;
            }
        }

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0) {
            fireCooldown = fireCooldownMax;
            payload.Generate(transform.position, target, alignment);
        }
    }

    GameObject GetNearestEnemyTarget() {
        Targetable closest = null;
        float minDist = Mathf.Infinity;
        foreach (Targetable core in GameObject.FindObjectsOfType<Targetable>()) {
            if (core.GetComponent<Alignment>().IsEnemyTo(alignment)) {
                float dist = (core.transform.position - transform.position).sqrMagnitude;
                if (dist < minDist) {
                    minDist = dist;
                    closest = core;
                }
            }
        }
        if (closest && IsTargetInRange(closest.gameObject)) {
            return closest.gameObject;
        } else {
            return null;
        }
    }

    bool IsTargetInRange(GameObject target) {
        return (target.transform.position - transform.position).sqrMagnitude < range * range;
    }

}