using UnityEngine;
using System.Collections;

public class UnitWeaponry : MonoBehaviour {

    GameObject projectilePrefab;
    GameObject target;
    Alignment alignment;

    public float range = 35;
    public float projectileVelocity = 25;

    public float fireCooldownMax = 1f;
    float fireCooldown = 0;

    void Start() {
        alignment = GetComponent<Alignment>();
        projectilePrefab = (GameObject)Resources.Load("Projectile");
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
            GameObject projectile = Instantiate<GameObject>(projectilePrefab);
            Vector3 dir = (target.transform.position - transform.position).normalized;
            projectile.transform.position = transform.position + 2 * dir;
            projectile.GetComponent<Rigidbody>().velocity = projectileVelocity * dir;
            projectile.GetComponent<Alignment>().IsPlayerOwned(alignment.IsPlayerOwned());
        }
    }

    GameObject GetNearestEnemyTarget() {
        Targetable closest = null;
        float minDist = Mathf.Infinity;
        foreach (Targetable core in GameObject.FindObjectsOfType<Targetable>()) {
            if (core.GetComponent<Alignment>().IsPlayerOwned() != alignment.IsPlayerOwned()) {
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