using UnityEngine;
using System.Collections;
namespace Blafed.Testing
{
    public abstract class TestScript : MonoBehaviour
    {
        [ContextMenu("Run")]
        public virtual void Run()
        {
        }
        public virtual void RunPlay()
        {

        }
        public virtual bool disabled { get; }

        // public virtual IEnumerable Run()
        // {
        //     yield break;
        // }
    }
}