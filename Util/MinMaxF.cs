using UnityEngine;
[System.Serializable]
public struct MinMaxF
{
    public float min;
    public float max;
    public float random => Random.Range(min, max);

    public float average => (min + max) * .5f;
    public MinMaxF(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
    public float lerp(float t) => Mathf.Lerp(min, max, t);

}