using UnityEngine;
using React.Core;

public class example : ScriptMono
{
    GameObject go;
    private void Start()
    {
        if (go)
        {
            go.name = "hello world";
            print(go.name);
        }
    }
    internal override void onRender()
    {
    }
}