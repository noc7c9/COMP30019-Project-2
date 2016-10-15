using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

    Alignment alignment;

    void Start() {
        alignment = GetComponent<Alignment>();
	}

    void OnTriggerEnter(Collider c) {
        HealthPoints hp = c.GetComponent<HealthPoints>();
        Alignment alignment = c.GetComponent<Alignment>();
        if (hp && alignment.IsPlayerOwned() != this.alignment.IsPlayerOwned()) {
            hp.healthPoints--;
            Destroy(gameObject);
        }
    }

}