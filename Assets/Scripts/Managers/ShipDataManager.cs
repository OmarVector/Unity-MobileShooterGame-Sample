using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ShipDataManager : MonoBehaviour
{
    /* A Serializable class that store all data of the ship for weapons power level and 
     Collected items.*/
    [Serializable]
    private class ShipDetails
    {
        // Main Cannon Level Power
        public int MainCannonLevel;

        // Wing Cannon Level Power 
        public int WingCannonLevel;

        // Rocket Level Power
        public int RocketLevel;

        // Twin Laser Power
        public int LaserLevel;

        // Magnet Level Power
        public int MagnetLevel;

        // Shield Level Power
        public int ShieldLevel;

        // Collected amount of coins
        public int CoinsAmount;

        // Collected amount of Lasers
        public int LaserAmount;

        // Collected amount of Shields
        public int ShieldAmount;

    }

    [HideInInspector] public int MainCannonLevel;
    [HideInInspector] public int WingCannonLevel;
    [HideInInspector] public int RocketLevel;
    [HideInInspector] public int LaserLevel;
    [HideInInspector] public int MagnetLevel;
    [HideInInspector] public int ShieldLevel;

    [HideInInspector] public int CoinsAmount;
    [HideInInspector] public int LaserAmount;
    [HideInInspector] public int ShieldAmount;

    public static ShipDataManager shipDataManager = null;

    // Initializing singletone of the shipDetailManager
    private void Awake()
    {
        if (shipDataManager == null)
            shipDataManager = this;
        else if (shipDataManager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        
        LoadShipData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            PrintShipDetails();
            ResetShipLevelData();
            LoadShipData();
          
        }
    }

    // Loading Saved Ship Detail from desk;
    public void LoadShipData()
    {
        if (File.Exists(Application.persistentDataPath + "/ShipData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ShipData.dat", FileMode.Open);
            ShipDetails shipDetails = new ShipDetails();
            shipDetails = (ShipDetails) bf.Deserialize(file);

            MainCannonLevel = shipDetails.MainCannonLevel;
            WingCannonLevel = shipDetails.WingCannonLevel;
            RocketLevel = shipDetails.RocketLevel;
            LaserLevel = shipDetails.LaserLevel;
            MagnetLevel = shipDetails.MagnetLevel;
            ShieldLevel = shipDetails.ShieldLevel;

            CoinsAmount = shipDetails.CoinsAmount;
            LaserAmount = shipDetails.LaserAmount;
            ShieldAmount = shipDetails.ShieldAmount;

            file.Close();
        }
        else
        {
            // I've bumped up all the values for this demo while the default values are commented
            Debug.Log("HELLo");
            MainCannonLevel = 70; // == 0
            WingCannonLevel = 60; // == 0
            RocketLevel = 50; // == 0;
            LaserLevel = 50; // == 0;
            MagnetLevel = 50; // == 0;
            ShieldLevel = 50; // == 0;

            CoinsAmount = 9999999; // == 0;
            LaserAmount = 100; // == 0;
            ShieldAmount = 100; // == 0;
        }
    }

    // Saving the data of the ship on client side.
    public void SaveShipLevelsData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/ShipData.dat", FileMode.OpenOrCreate);

        ShipDetails shipDetails = new ShipDetails();
        shipDetails.MainCannonLevel = MainCannonLevel;
        shipDetails.RocketLevel = RocketLevel;
        shipDetails.WingCannonLevel = WingCannonLevel;
        shipDetails.LaserLevel = LaserLevel;
        shipDetails.MagnetLevel = MagnetLevel;
        shipDetails.ShieldLevel = ShieldLevel;


        shipDetails.CoinsAmount = CoinsAmount;
        shipDetails.LaserAmount = LaserAmount;
        shipDetails.ShieldAmount = ShieldAmount;

        bf.Serialize(file, shipDetails);
        file.Close();
    }

    // Reset ship data
    public void ResetShipLevelData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/ShipData.dat", FileMode.OpenOrCreate);

        ShipDetails shipDetails = new ShipDetails();
        shipDetails.MainCannonLevel = 70;
        shipDetails.RocketLevel = 60;
        shipDetails.WingCannonLevel = 50;
        shipDetails.LaserLevel = 50;
        shipDetails.MagnetLevel = 50;
        shipDetails.ShieldLevel = 50;


        shipDetails.CoinsAmount = 999999;
        shipDetails.LaserAmount = 100;
        shipDetails.ShieldAmount = 100;

        bf.Serialize(file, shipDetails);
        file.Close();
        
      
    }
    
    private void OnApplicationQuit()
    {
        SaveShipLevelsData();
    }

    private void PrintShipDetails()
    {
        Debug.Log("<b><color=teal> Main Cannon Level      : " + MainCannonLevel + "</color></b>");
        Debug.Log("<b><color=teal> Rockets Level          : " + RocketLevel + "</color></b>");
        Debug.Log("<b><color=teal> Wing Cannon Level      : " + WingCannonLevel + "</color></b>");
        Debug.Log("<b><color=teal> Twin Laser Level       : " + LaserLevel + "</color></b>");
        Debug.Log("<b><color=teal> Magnet Level           : " + MagnetLevel + "</color></b>");
        Debug.Log("<b><color=teal> Shield Level           : " + ShieldLevel + "</color></b>");
  
        Debug.Log("<b><color=teal> Gear Count             : " + CoinsAmount + "</color></b>");
        Debug.Log("<b><color=teal> Twin Laser Count       : " + LaserAmount + "</color></b>");
        Debug.Log("<b><color=teal> Shield Count           : " + ShieldAmount + "</color></b>");
    }
}