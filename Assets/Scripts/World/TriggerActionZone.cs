using UnityEngine;
using UnityEngine.Events;

public class TriggerActionZone : MonoBehaviour
{
    [SerializeField] private bool executeExitFunctionAtStart;
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;

    private void Start()
    {
        if(executeExitFunctionAtStart)
        {
            TriggerExit();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        TriggerEnter(other.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        TriggerEnter(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        TriggerExit();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        TriggerExit();
    }

    private void TriggerEnter(GameObject obj)
    {
        Debug.Log($"TRIGGER {transform.name} -> Enter : {obj.name}");

        OnEnter?.Invoke();
    }

    private void TriggerExit()
    {
        //Debug.LogError("TRIGGER -> Exit");

        OnExit?.Invoke();
    }
}
