using System.Collections;
using UnityEngine;

public class ObjectDispenser_Activatable : Activatable
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform dispensedContainer;
    [SerializeField] private GameObject dispPrefab;

    [Header("Properties")]
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float dispensedForce = 50;
    [SerializeField] private float delayBtwDisp = 0.1f;
    [SerializeField] private int dispAmount = 1;

    public override void Activate()
    {
        StartCoroutine(DispenseObject());
    }

    private IEnumerator DispenseObject()
    {
        var curDisp = dispAmount;

        while (curDisp > 0)
        {
            var obj = Instantiate(dispPrefab, firePoint.position, Quaternion.LookRotation(firePoint.forward), dispensedContainer);
            obj.GetComponent<Arrow>().Init(dispensedForce, lifetime);
        
            yield return new WaitForSeconds(delayBtwDisp);
            
            curDisp--;
        }

        yield break;
    }
}