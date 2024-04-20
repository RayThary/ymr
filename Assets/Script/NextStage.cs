using UnityEngine;

public class NextStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.CardSelectStep();
    }
}
