using System.Collections;
using UnityEngine;


namespace Blafed.Tween
{
    using Internal;

    public abstract class Tween
    {
        bool _isPlaying;
        bool isPhysics;
        float lifeTime = 0;
        public float progress => evaluate(lifeTime).clamp01();
        public virtual bool isPlaying => _isPlaying;
        public virtual bool isFinished => progress >= 1;
        // IConcept concept;
        // IManipulator manipulator = DefaultManipulator.identity;

        IEnumerator coro;

        protected Tween()
        {
            play();
        }

        void forcePlay()
        {
            _isPlaying = true;
            TweenManager.instance.StartCoroutine(coro = cycle());
        }
        public Tween play()
        {
            if (isPlaying)
                return this;
            forcePlay();
            return this;
        }
        public Tween stop()
        {
            if (!isPlaying)
                return this;
            _isPlaying = false;
            TweenManager.instance.StopCoroutine(coro);
            return this;
        }
        public void reset()
        {
            stop();
            lifeTime = 0;
            onReset();
        }

        protected virtual void onReset()
        {

        }


        public abstract void update(float p);
        public virtual float evaluate(float t) => t;

        IEnumerator cycle()
        {
            while (!isFinished)
            {
                if (isPhysics)
                {
                    lifeTime += Time.fixedDeltaTime;
                    yield return Extensions2.WaitForFixedUpdate;
                }
                else
                {
                    lifeTime += Time.deltaTime;
                    yield return null;
                }
                update(progress);
            }
            _isPlaying = false;
        }
    }


    public class GeneralTween : Tween
    {
        System.Action<float> action;


        public GeneralTween(System.Action<float> action)
        {
            this.action = action;
        }

        public override void update(float t)
        {
            action(t);
        }
    }
    public abstract class WrapperTween : Tween
    {
        public readonly Tween target;

        protected WrapperTween(Tween target)
        {
            this.target = target;
            target.stop();
        }
        public override void update(float p)
        {
            target.update(p);
        }

        public override sealed float evaluate(float t)
        {
            return target.evaluate(eval(t));
        }

        protected virtual float eval(float t) => t;
    }

    public class DurationTween : WrapperTween
    {
        public float duration;

        public DurationTween(Tween target, float duration) : base(target)
        {
            this.duration = duration;
        }

        protected override float eval(float t) => t / duration;
    }

    public class EaseTween : WrapperTween
    {
        public EaseType2 ease;

        public EaseTween(Tween target, EaseType2 ease) : base(target)
        {
            this.ease = ease;
        }

        protected override float eval(float t) => ease.evaluate(t);
    }


    public class LoopTween : WrapperTween
    {
        int loops;
        LoopType loopType;


        int loopCounter;


        bool isInfinit => loops <= 0;
        public override bool isFinished => loopCounter >= loops && !isInfinit;

        public LoopTween(Tween target, int loops, LoopType loopType = 0) : base(target)
        {
            this.loops = loops;
            this.loopType = loopType;
        }


        protected override void onReset()
        {
            loopCounter = 0;
        }


        public override void update(float t)
        {
            base.update(t);
            if (target.isFinished)
            {
                loopCounter++;
                target.reset();
                target.play();
            }
        }


        protected override float eval(float t)
        {
            switch (loopType)
            {
                case LoopType.restart:
                case LoopType.increment:
                    return t % 1;
                case LoopType.yoyo:

                    if (loopCounter % 2 == 0)
                        return t;
                    else
                        return (1 - t);
            }
            throw new System.Exception();
        }
    }


    public enum LoopType
    {
        restart,
        yoyo,
        increment,
    }


    /// <summary>
    /// Applys only one action
    /// </summary>
    public abstract class MonoTween : Tween
    {
        public override float evaluate(float t)
        {
            return t > Mathf.Epsilon ? 1 : 0;
        }
        public override void update(float t)
        {
            if (t >= 1)
            {
                execute();
            }
        }
        protected abstract void execute();
    }

    public class ActionTween : MonoTween
    {
        public System.Action action;


        public ActionTween(System.Action action)
        {
            this.action = action;
        }
        protected override void execute()
        {
            action();
        }
    }

}