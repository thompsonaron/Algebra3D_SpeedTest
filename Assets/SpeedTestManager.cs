using System;
using System.Diagnostics;
using UnityEngine;

public class SpeedTestManager : MonoBehaviour
{
    private Stopwatch stopwatch = new Stopwatch();
    private int numberOfExection = 10000000;

    public string PropertyString { get; set; }
    public string fieldString;

    void Start()
    {
        //DebugLogTest(); // DONT USE DEBUG IN PRODUCTION

        // PropertyVsFieldTest(); // DONT USE PROP - use field

        // StringBuilderVsStringPlusOperator(); // NEVER use plust operator for strings

        // CachingVsNonCachingComponents(); // BIG WIN for caching - always cache

        // GenericVsExplicit(); // A big win for generic
    }

    private void GenericVsExplicit()
    {
        Transform t;
        Rigidbody rb;

        measureExecutionTime("Get Component generic", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                //t = GetComponent<Transform>();
                rb = GetComponent<Rigidbody>();
            }
        });

        measureExecutionTime("Get Component explicit", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                //t = (Transform) GetComponent(typeof(Transform));
                rb = (Rigidbody)GetComponent(typeof(Rigidbody));
            }
        });
    }

    private void CachingVsNonCachingComponents()
    {
        Transform tempTransform;
        Transform cachedTransformed = transform;
        Vector3 tempPosition;

        measureExecutionTime("Accesing exposed transform component", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                tempTransform = transform;
            }
        });

        measureExecutionTime("Accesing cached transform component", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                tempTransform = cachedTransformed;
            }
        });

        measureExecutionTime("Accesing exposed transform position", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                tempPosition = transform.position;
            }
        });

        measureExecutionTime("Accesing cached transform position", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                tempPosition = cachedTransformed.position;
            }
        });
    }

    private void StringBuilderVsStringPlusOperator()
    {
        measureExecutionTime("Strings combined with + operator", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                fieldString += "Some string";
            }
        });
        print("String length: " + fieldString.Length);

        measureExecutionTime("Strings combined with StringBuilder class", () =>
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < numberOfExection; i++)
            {
                sb.Append("Some string");
            }
            fieldString = sb.ToString();
        });
        print("String length: " + fieldString.Length);
    }

    // Action block is "some code"
    void measureExecutionTime(string testName, Action block)
    {
        stopwatch.Start();

        block();

        stopwatch.Stop();

        print(testName + " finished in : " + stopwatch.ElapsedMilliseconds);

        stopwatch.Reset();
    }

    private void DebugLogTest()
    {
        // 1) Debug Log
        //    RESULT - DONT HAVE DEBUG IN PRODUCTION
        // () => { code } sends code data to Action block
        measureExecutionTime("Just a string test ", () =>
        {
            print("Whatever");
        });
    }

    private void PropertyVsFieldTest()
    {
        // 2) Property vs field - reading and writing
        //  RESULT: FIELDS WIN
        measureExecutionTime("Setting properties ", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                PropertyString = "Some string";
            }
        });

        measureExecutionTime("Setting field", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                fieldString = "Some string";
            }
        });

        string testString;
        PropertyString = "String";
        fieldString = "String";

        measureExecutionTime("Getting properties", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                testString = PropertyString;
            }
        });

        measureExecutionTime("Getting field", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                testString = fieldString;
            }
        });


        measureExecutionTime("Getting from function", () =>
        {
            for (int i = 0; i < numberOfExection; i++)
            {
                fieldString = getString();
            }
        });

        string getString() { return "String"; }
    }
}
