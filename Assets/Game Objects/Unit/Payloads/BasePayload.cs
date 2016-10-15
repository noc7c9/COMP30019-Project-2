using UnityEngine;

abstract public class BasePayload : MonoBehaviour {

    protected Vector3 startPos;
    protected GameObject target;
    protected Alignment alignment;

    public void Generate(Vector3 startPos, GameObject target, Alignment alignment) {
        GameObject o = Instantiate<GameObject>(this.gameObject);
        BasePayload payload = o.GetComponent<BasePayload>();
        payload.startPos = startPos;
        payload.target = target;
        payload.alignment = alignment;
    }

}