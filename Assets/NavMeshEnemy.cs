using UnityEngine;

public class NavMeshEnemy : MonoBehaviour

{
    // ��������� ����� ����������
    public Transform goal;
    void Start()
    {
        goal = PlayerMoveController.Instance.transform;
        // ��������� ���������� ������
        UnityEngine.AI.NavMeshAgent agent
            = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // ������� ����� ����������
        agent.destination = goal.position;
    }
}
