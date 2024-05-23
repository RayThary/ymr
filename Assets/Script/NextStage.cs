using UnityEngine;

public class NextStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.CardSelectStep();
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
}
