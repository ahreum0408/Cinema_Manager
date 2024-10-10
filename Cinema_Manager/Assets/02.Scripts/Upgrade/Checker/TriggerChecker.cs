using UnityEngine;

public class TriggerChecker : MonoBehaviour {
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("돈빠지는 중");
        }
    }
}
