using UnityEngine;
[System.Serializable]
public struct MinMax
{
    public int min;
    public int max;
    public int random => Random.Range(min, max + 1);
    public int averageInt => (min + max) / 2;
    public float average => (min + max) * .5f;
    public int delta => max - min;
    public MinMax(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
    public float lerp(float t) => Mathf.Lerp(min, max, t);
    public int lerpInt(float t, int baseValue = 1) => (lerp(t) / baseValue).roundToInt() * baseValue;
}