using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Blafed.Testing
{
    public class TestRunner : MonoBehaviour
    {
        private void Start()
        {
            if (Application.isEditor)
                runTests();
        }
#if UNITY_EDITOR
        [MenuItem("Tools/Run Tests")]
#endif
        static void runTests()
        {
            var tests = FindObjectsOfType<TestScript>();
            foreach (var x in tests)
            {
                if (x.disabled)
                {
                    Debug.Log(x + " is disabled");
                    continue;
                }
                if (Application.isPlaying)
                    x.RunPlay();
                x.Run();
            }
            runTestsGlobally();
        }

        static void runTestsGlobally()
        {
            var tests = findGlobally();
            foreach (var x in tests)
            {
                if (x is ITestGlobal edit)
                    edit.run();
                if (Application.isPlaying)
                {
                    if (x is ITestGlobalPlay play)
                        play.runPlay();
                }
            }
        }
        static IEnumerable findGlobally()
        {
            var typeofTest = typeof(ITestGlobal);
            var typeofTest2 = typeof(ITestGlobalPlay);
            var result =
            from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where type.GetInterfaces().Any(x => x == typeofTest || x == typeofTest2)
            select Activator.CreateInstance(type);
            return result;
        }
    }

    public interface ITestGlobal
    {
        void run();
    }
    public interface ITestGlobalPlay
    {
        void runPlay();
    }
}