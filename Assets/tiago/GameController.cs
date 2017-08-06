using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 

public class GameController : Singleton<GameController>
{
    private int level = 3;                                  //Current level number, expressed in game as "Day 1".
    private string exampleVal = "val";


    public static int Level {
        get {
            return Instance.level;
        }

        set {
            Instance.level = value;
        }
    }   
    public static string ExampleVal {
        get {
            return Instance.exampleVal;
        }
    }
}