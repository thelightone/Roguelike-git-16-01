using UnityEngine;

public class NavMeshEnemy : MonoBehaviour

{
    // Положение точки назначения
    public Transform goal;
    void Start()
    {
        goal = PlayerMoveController.Instance.transform;
        // Получение компонента агента
        UnityEngine.AI.NavMeshAgent agent
            = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Указаие точки назначения
        agent.destination = goal.position;
    }
}
