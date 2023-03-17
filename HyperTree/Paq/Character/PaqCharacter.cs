namespace Paq
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    [AddComponentMenu("Paq/Paq Character")]
    public class PaqCharacter : PaqMovable
    {
        CapsuleCollider _collider;
        public new CapsuleCollider collider => this.cachedComponent(ref _collider);






    }
}