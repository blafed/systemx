using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PaqMovable : MonoBehaviour
{
    public bool worldSpace = false;
    public Vector3 inVelocity;
    public Vector3 inVelocity2;

    public List<Vector3> netVelocity = new List<Vector3>();
    public float gravityFactor = 1;

    public Rigidbody rb => this.cachedComponent(ref _rb);
    public Vector3 velocity => rb.velocity;
    Rigidbody _rb;

    [SerializeField] bool _isGrounded;
    public bool isGrounded => this._isGrounded;
    //public bool isReallyGrounded => Physics.Raycast(Vector3.down * )


    private void OnEnable()
    {
        StartCoroutine(afterFixedUpdate());
    }

    public Vector3 totalInVelocity
    {
        get
        {
            var sum = new Vector3();
            if (netVelocity.Count > 0)
            {
                foreach (var x in netVelocity)
                {
                    sum += x;
                }
            }
            return inVelocity + sum + inVelocity2;
        }
    }

    IEnumerator afterFixedUpdate()
    {

        while (enabled)
        {
            vgo = vg;
            beforeCompat();
            _isGrounded = vg.sqrMagnitude < (Time.fixedDeltaTime * gravity.abs().maxComp() * 2).squared();
            yield return Extensions2.WaitForFixedUpdate;
            afterCompat();
        }
    }
    Vector3 inVelocityX => worldSpace ? totalInVelocity : transform.TransformDirection(totalInVelocity);


    Vector3 vo;
    // Vector3 vw;
    Vector3 vi;
    Vector3 vx;
    Vector3 vgo;

    Vector3 vg;

    Vector3 gravity => Physics.gravity * gravityFactor;
    //  public bool isGrounded => vg.sqrMagnitude < (Time.fixedDeltaTime * gravity.abs().maxComp() * 2).square();

    void beforeCompat()
    {


        var gravityDir = gravity.toDirOne();
        var gxScaled = Vector3.Scale(vx, gravityDir);
        vg += -gxScaled + gravity * Time.fixedDeltaTime;

        vi = this.inVelocityX;
        //vw = vx;
        vo = vi + vg;
        rb.velocity = vo;
        step1();
    }
    void afterCompat()
    {
        var vt = rb.velocity;
        var x = vt - vo;
        // vw += x;
        vx = x;
        step2();
    }
    protected virtual void step1() { }
    protected virtual void step2() { }
}