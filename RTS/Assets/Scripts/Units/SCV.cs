using UnityEngine;
using UnityEngine.AI;

public class SCV : MonoBehaviour, IBuilder
{
    [Header("¼³Á¤")]
    public float mineDuration = 3f;
    public float buildTime = 5f;

    enum State { Idle, Moving, Mining, Returning, Building }
    State state = State.Idle;

    NavMeshAgent agent;
    GameObject targetMineral;
    GameObject buildingPrefab;
    Vector3 buildPosition;
    float mineTimer;
    float buildTimer;
    bool carryingResource = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Moving:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    OnArrived();
                break;

            case State.Mining:
                mineTimer -= Time.deltaTime;
                if (mineTimer <= 0f)
                    FinishMining();
                break;

            case State.Returning:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    OnReturnArrived();
                break;

            case State.Building:
                buildTimer -= Time.deltaTime;
                if (buildTimer <= 0f)
                    FinishBuilding();
                break;
        }
    }

    void OnArrived()
    {
        if (buildingPrefab != null)
        {
            BeginBuilding();
        }
        else if (targetMineral != null)
        {
            BeginMining();
        }
        else
        {
            state = State.Idle;
        }
    }

    public void OrderMove(Vector3 point)
    {
        ClearOrder();
        agent.isStopped = false;
        agent.SetDestination(point);
        state = State.Moving;
    }

    public void OrderMine(GameObject mineral)
    {
        ClearOrder();
        targetMineral = mineral;
        agent.isStopped = false;
        agent.SetDestination(mineral.transform.position);
        state = State.Moving;
    }

    public void OrderBuild(GameObject prefab, Vector3 placePos, Vector3 spawnPos)
    {
        ClearOrder();
        buildingPrefab = prefab;
        buildPosition = placePos;
        agent.isStopped = false;
        agent.SetDestination(spawnPos);
        state = State.Moving;
    }

    void BeginMining()
    {
        agent.isStopped = true;
        state = State.Mining;
        mineTimer = mineDuration;
    }

    void FinishMining()
    {
        carryingResource = true;
        targetMineral = null;

        var cc = GameManager.Instance.GetNearestCommandCenter(transform.position);
        if (cc != null)
        {
            agent.isStopped = false;
            agent.SetDestination(cc.spawnPoint.position);
            state = State.Returning;
        }
        else state = State.Idle;
    }

    void OnReturnArrived()
    {
        if (carryingResource)
        {
            GameManager.Instance.AddResources(5);
            carryingResource = false;
        }
        state = State.Idle;
    }

    void BeginBuilding()
    {
        agent.isStopped = true;
        state = State.Building;
        buildTimer = buildTime;
    }

    void FinishBuilding()
    {
        Instantiate(buildingPrefab, buildPosition, Quaternion.identity);
        buildingPrefab = null;
        state = State.Idle;
    }

    void ClearOrder()
    {
        targetMineral = null;
        buildingPrefab = null;
    }
}
