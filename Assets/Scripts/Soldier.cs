using UnityEngine;

public class Soldier : Unit
{
    void Start()
    {
        GetComponent<MeshRenderer>().material = Resources.Load("Soldier") as Material; //Change material and id for different type of unit
        unitId = 1;
    }

    public override void Update()
    {
        base.Update();
    }
}
