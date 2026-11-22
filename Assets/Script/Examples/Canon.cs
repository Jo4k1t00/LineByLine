using System.Collections;
using UnityEngine;

public class Canon : CodeDemo
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform punta;

    protected override IEnumerator Sequence()
    {
        yield return Line(0);
        yield return Line(2);
        var bala = Instantiate(prefab, punta.position, punta.rotation);
        yield return Line(3);
        bala.GetComponent<Rigidbody>().AddForce(punta.forward * 20, ForceMode.Impulse);
    }
}