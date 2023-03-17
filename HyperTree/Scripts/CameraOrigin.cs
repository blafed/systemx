using UnityEngine;
using System.Collections.Generic;
public class CameraOrigin : MonoBehaviour
{


    public static CameraOrigin main;
    public CameraOriginState state
    {
        get => new CameraOriginState
        {
            lockAtCenter = this.lockAtCenter,
            baseRot = this.baseRot,
            pos = this.pos,
            pivot = this.pivot,
            dir = this.dir,
            rot = this.dir,
            zoom = this.zoom,
        };
        set
        {
            this.baseRot = value.baseRot;
            this.pos = value.pos;
            this.rot = value.rot;
            this.dir = value.dir;
            this.pivot = value.pivot;
            this.lockAtCenter = value.lockAtCenter;
            this.zoom = value.zoom;
        }
    }
    public Vector3 baseRot
    {
        get => transform.eulerAngles;
        set => transform.eulerAngles = value;
    }
    public Vector3 pos
    {
        get => transform.position;
        set => transform.position = value;
    }
    public Vector3 pivot;
    public Vector3 dir = Vector3.forward;
    public Vector3 rot;
    public float zoom = 1f;

    public bool lockAtCenter;

    public List<CameraManipulator> customManipualators = new List<CameraManipulator>();
    CameraManipulator[] inManipulators;

    public CameraOriginState originalState { get; private set; }
    public CameraOriginState prevState { get; set; }
    public CameraManipulator prevManipulator { get; private set; }
    public CameraManipulator nextManipulator { get; private set; }

    public new Camera camera => this.cachedComponentInChild(ref _camera);

    Camera _camera;

    Transform _pivot, _direction, _rotx, _roty, _rotz;


    Transform build(ref Transform current, string name, System.Action<Transform> processor = null)
    {
        Transform t;
        if (current.childCount > 0)
            t = current.GetChild(0);

        else
        {
            (t = new GameObject(name).transform).orient(current, true);
            if (processor != null)
                processor(t);
        }
        current = t;
        return t;
    }
    private void Start()
    {
        inManipulators = GetComponentsInChildren<CameraManipulator>(true);

        Transform current = transform;
        _pivot = build(ref current, "pivot");
        _direction = build(ref current, "direction");
        _rotx = build(ref current, "rotx");
        _roty = build(ref current, "roty");
        _rotz = build(ref current, "rotz");
        build(ref current, "camera", x =>
        {
            bool otherWay = false;
            if (tag == "MainCamera")
            {
                if (Extensions2.mainCamera)
                {
                    Extensions2.mainCamera.transform.parent = x.parent;
                    Destroy(x.gameObject);
                    otherWay = true;
                }
                else
                {
                }
            }
            if (!otherWay)
            {
                _camera = x.gameObject.AddComponent<Camera>();
                if (tag == "MainCamera")
                    _camera.tag = "MainCamera";

                // if (UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset.GetType().Name == "UniversalRenderPipelineAsset")
                // {
                //     var uac = _camera.getOrAdd<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
                //     if (uac)
                //         uac.renderPostProcessing = true;
                // }
            }
        });

        pivot = _pivot.localPosition;
        dir = _direction.forward;
        rot = new Vector3(_rotx.localEulerAngles.x, _roty.localEulerAngles.y, _rotz.localEulerAngles.z);
    }

    void manipulate()
    {
        this.originalState = this.state;
        for (int i = 0; i < this.inManipulators.Length; i++)
        {
            var c = inManipulators.safeAccess(i);
            if (c == null)
                continue;
            var p = inManipulators.safeAccess(i - 1);
            var n = inManipulators.safeAccess(i + 1);
            this.nextManipulator = n;
            this.prevManipulator = p;
            this.prevState = state;
            c.manipulate(this);
        }
        for (int i = 0; i < this.customManipualators.Count; i++)
        {
            var c = customManipualators.safeAccess(i);
            if (c == null)
                continue;
            this.prevState = state;


            var p = customManipualators.safeAccess(i - 1);
            var n = customManipualators.safeAccess(i + 1);
            this.nextManipulator = n;
            this.prevManipulator = p;
            c.manipulate(this);
        }
    }

    private void Update()
    {
        manipulate();
        if (lockAtCenter)
        {
            camera.transform.localEulerAngles = transform.localEulerAngles;
            // rot = transform.eulerAngles;
        }
        camera.fieldOfView = 60 * zoom;
        camera.orthographicSize = 10f * zoom;
        _pivot.localPosition = pivot;
        _direction.forward = dir;
        _rotx.localEulerAngles = Vector3.right * rot.x;
        _roty.localEulerAngles = Vector3.up * rot.y;
        _rotz.localEulerAngles = Vector3.forward * rot.z;
    }


}

public interface ICameraManipulator
{
    public bool enabled { get; set; }
    void manipulate(CameraOrigin o);
}
//.CAMERA MAnipulation trance
[System.Serializable]
public struct CameraOriginState
{
    public Vector3 pos;
    public Vector3 baseRot;
    public Vector3 dir;
    public Vector3 rot;
    public Vector3 pivot;

    public float zoom;
    public bool lockAtCenter;
}
// public class CameraFollowCharacter : MonoBehaviour, ICameraManipulator
// {
//     public Transform target;
//     public void manipulate(CameraOrigin o)
//     {
//         if (target)
//             o.pos = target.position;
//     }
// }


public class CameraManipulator : MonoBehaviour
{

    public virtual void manipulate(CameraOrigin o) { }
}