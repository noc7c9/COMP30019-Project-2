using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : BasePayload {

    LineRenderer line;
    float lifeSpan = 0.1f;

	void Start() {
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, startPos);
        line.SetPosition(1, target.transform.position);
        line.SetColors(alignment.GetColor(), alignment.GetColor());

        // raycast and see if target is hit
        // if target is hit, then apply damage
        RaycastHit hit;
        bool isHit = Physics.Linecast(startPos, target.transform.position, out hit, ~LayerMask.GetMask("Territory"));
        GameObject hitObject = hit.collider.gameObject;
        if (isHit && alignment.IsEnemyTo(hitObject)) {
            HealthPoints hp = hitObject.GetComponent<HealthPoints>();
            hp.healthPoints--;
        } else {
            Destroy(gameObject);
        }
	}
	
	void Update() {
        // display laser beam for a split second
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0) {
            Destroy(gameObject);
        }
	}

}