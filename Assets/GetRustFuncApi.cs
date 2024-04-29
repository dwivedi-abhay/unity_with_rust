using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetRustFuncApi : MonoBehaviour
{

    public TMP_Text greetingsText;

    public Button getGreetingsButton;

    public TMP_InputField getStringInput;
    GetFromRustBackend getFromRustBackend;

    // Start is called before the first frame update
    void Start()
    {
        getFromRustBackend = new GetFromRustBackend();


        getGreetingsButton.onClick.AddListener(GetGreetings);
        int a = 1;
        int b = 2;
        unsafe
        {
            int* aptr = &a;
            int* bptr = &b;
            int c = getFromRustBackend.GetSumFromRust((IntPtr)aptr, (IntPtr)bptr);
            Debug.Log($"Wow it worked {c}");

           
        }
        string inputString = "inputString";
        string outputString = getFromRustBackend.GetRustGreetingsFromBackend(inputString);
        Debug.Log($"result from rust is {outputString}");

        //greetingsText.text = outputString;   
       

    }

    public void GetGreetings()
    {
        string inputString = getStringInput.text;
        getStringInput.text = "";
        string outputString = getFromRustBackend.GetRustGreetingsFromBackend(inputString);
        Debug.Log($"result from rust is {outputString}");

        greetingsText.text = outputString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

class GetFromRustBackend
{


#if !UNITY_EDITOR && (UNITY_IOS || UNITY_WEBGL)
    private const string dllName = "__Internal";
#else
    private const string dllName = "rust_get_card";
#endif

    private int sum;

   


    public int GetSumFromRust(IntPtr a, IntPtr b)
    {
        return get_sum_of_two_numbers(a, b);
    }

    public string GetRustGreetingsFromBackend(string name)
    {
        IntPtr stringPtr = Marshal.StringToHGlobalAnsi(name);

        IntPtr resultPtr = get_greetings(stringPtr);

        string result = Marshal.PtrToStringAnsi(resultPtr);
        return result;
    }

    [DllImport(dllName)]
    private static extern int get_hello_world();

    [DllImport(dllName)]
    private static extern int get_sum_of_two_numbers(IntPtr a, IntPtr b);

    [DllImport(dllName)]
    private static extern IntPtr get_greetings(IntPtr name);

    [DllImport(dllName)]
    private static extern void free_string(IntPtr ptr);



}
