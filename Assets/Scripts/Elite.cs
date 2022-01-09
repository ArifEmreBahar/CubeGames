using UnityEngine;

public class Elite : Unit
{
    void Start()
    {
        GetComponent<MeshRenderer>().material = Resources.Load("Elite") as Material; //Change material and id for different type of unit
        unitId = 2;
    }

    public override void Update()
    {
        base.Update();
    }
}
