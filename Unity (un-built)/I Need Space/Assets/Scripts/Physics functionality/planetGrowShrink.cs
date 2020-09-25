using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetGrowShrink : MonoBehaviour
{
    //
    //  causes the object attached to grow and shrink using the Sin value of time since scene was loaded (sine wave)
    //

    private float count;
    private float originalScale;
    private float nextScale;

    public float changeMag = 3.5f;
    public float changeRate = 2;
    public float offset = 0;
    private void Awake()
    {
        count = offset; //indirectly sets start point in oscillation "cycle"
        originalScale = transform.localScale.x; //fiunds original scale of object
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        count += Time.fixedDeltaTime; //increases count by time elapsed
        nextScale = originalScale + (Mathf.Sin(count / changeRate) * changeMag); //alters scale by taking (sine of the time elapsed / the rate of change wanted) * size of change (e.g. mag of 5 will cause overall scale oscillation of 10)
        transform.localScale = new Vector3(nextScale, nextScale, transform.localScale.z); //applies altered scale to object
    }
}
