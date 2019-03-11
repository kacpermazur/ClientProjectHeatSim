using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    bool heatingOn;
    int currentTime;
    float wallTemp;
    float airTemp;
    float airWallDifference;
    int[] heatingPeriod1;
    int[] heatingPeriod2;

    //Variables
    int wallType;
    int heatTime;
    int targetTemp;

    //UI Elements
    public Dropdown wallList;
    public Dropdown timeList;
    public Dropdown tempList;

    public Text timeText;
    public Text wallTempText;
    public Text airTempText;
    public Text thermoStatus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void RunSim()
    {
        //Read variables
        wallType = wallList.value;
        switch (timeList.value)
        {
            case 0:
                heatTime = 4; //Number of half hours heating is on for
                break;
            case 1:
                heatTime = 6;
                break;
            case 2:
                heatTime = 10;
                break;
            default:
                break;
        }
        switch (tempList.value)
        {
            case 0:
                targetTemp = 18;
                break;
            case 1:
                targetTemp = 20;
                break;
            case 2:
                targetTemp = 22;
                break;
            default:
                break;
        }

        //Reset to starting values
        airTemp = 14;
        wallTemp = 14;
        currentTime = 0;
        heatingOn = true;
        
        //Get the time period the heating is on for
        heatingPeriod1 = new int[heatTime];
        heatingPeriod2 = new int[heatTime];

        for (int i = 0; i < heatTime; i++)
        {
            heatingPeriod1[i] = 0 + i;
        }
        for (int i = 0; i < heatTime; i++)
        {
            heatingPeriod2[i] = 22 + i;
        }

        //Update displayed values
        UpdateDisplay();
    }

    public void AdvanceTime()
    {
        currentTime += 1;  
        
        if (heatingOn) //If the heating is currently on
        {
            

            //Calc difference in temperature between air and wall
            airWallDifference = (airTemp - wallTemp);

            //Heat walls based on current air temperature
            if (airWallDifference > 0) //Walls heat up if air is warmer
            {
                switch (wallType) //Heating depends on type of wall
                {
                    case 0: //Quick
                        wallTemp += (1.5f * airWallDifference / 2.0f);
                        airTemp -= (0.5f * airWallDifference / 2.0f);
                        break;
                    case 1: //Medium
                        wallTemp += (1.0f * airWallDifference / 2.0f);
                        airTemp -= (1.0f * airWallDifference / 2.0f);
                        break;
                    case 2: //Slow
                        wallTemp += (0.5f * airWallDifference / 2.0f);
                        airTemp -= (2.0f * airWallDifference / 2.0f);
                        break;
                    default:
                        break;
                }
            }

            //Air heats up
            airTemp += 4;
            //Air Temperature should not go above the target temperature
            if (airTemp > targetTemp)
            {
                airTemp = targetTemp;
            }

        }
        else //If the heating is off
        {
            switch (wallType) //Heating depends on type of wall
            {
                case 0: //Quick
                    wallTemp -= 0.25f;
                    break;
                case 1: //Medium
                    wallTemp -= 0.5f;
                    break;
                case 2: //Slow
                    wallTemp -= 0.25f;
                    break;
                default:
                    break;
            }

            airTemp -= 1.0f;

            //Air Temperature should not drop below 14 degrees
            if (airTemp < 14)
            {
                airTemp = 14;
            }
        }



        //Turns heating on/off at end of time step
        for (int i = 0; i < heatTime; i++)
        {
            if (heatingPeriod1[i] == currentTime || heatingPeriod2[i] == currentTime)
            {
                heatingOn = true; //Turns heating on if within set heating periods
            }
        }
        if (airTemp >= targetTemp)
        {
            heatingOn = false; //Overrides heating setting if current air temperature is above limit
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        airTempText.text = airTemp.ToString();
        wallTempText.text = wallTemp.ToString();
        thermoStatus.text = heatingOn.ToString();
        timeText.text = currentTime.ToString();
        timeText.text = (6 + Mathf.FloorToInt(currentTime / 2)).ToString() + ":" + (Mathf.FloorToInt(currentTime % 2) * 30).ToString();
    }

}
