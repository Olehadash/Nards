

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{

    public static Dictionary<string, Place> Places = new Dictionary<string, Place>();
    public static Dictionary<string, Chip> WhiteChip = new Dictionary<string, Chip>();
    public static Dictionary<string, Chip> BlackChip = new Dictionary<string, Chip>();

    public static Dice dice_1;
    public static Dice dice_2;
    public static Dice dice_3;
    public static Dice dice_4;

    public static List<int> coordsW = new List<int>();
    public static List<int> coordsB = new List<int>();

    public static List<int> home_W_id = new List<int>();
    public static List<int> home_B_id = new List<int>();
    public static Place homeW = new Place(), homeB = new Place();
    
}
