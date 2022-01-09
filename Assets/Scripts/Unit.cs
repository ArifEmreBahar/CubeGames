using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, ISelectable
{
    [HideInInspector] public bool selected;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public int unitId;
    private bool isTargetChanged;
    private GameObject selectRing;
    private GameObject directionArrow;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = transform.position;
        selectRing = transform.Find("SelectRing").gameObject;
        directionArrow = transform.Find("DirectionArrow").gameObject;
    }

    public virtual void Update()
    {
        //Actions to take when Unit is selected
        if (selected)
        {
            selectRing.SetActive(true);

            if (isTargetChanged)
            {
                navMeshAgent.SetDestination(target);
                isTargetChanged = false;
            }
        }
        else
        {
            selectRing.SetActive(false);
        }

        //Actions to take when Unit is arrived
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0)
            {
                directionArrow.SetActive(false);
            }
        }
        else
        {
            if (navMeshAgent.velocity.normalized != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
            }
            directionArrow.SetActive(true);
        }
    }

    //Sets unit speed
    public void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    //Sets unit stopdistance
    public void SetStopDistance(float stopDistance)
    {
        navMeshAgent.stoppingDistance = stopDistance;
    }
    //Toggle select status of unit
    public void OnSelected(bool isSelect)
    {
        selected = isSelect;
    }
    //Set target of unit
    public void SetTarget(Vector3 target)
    {   
        if (this.target != target)
        {
            this.target = target;
            isTargetChanged = true;
        }
    }
}
