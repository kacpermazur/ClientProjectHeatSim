﻿using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    bool heatingOn; //If thermo is currently on
    int currentTime; //Current time, in half hours starting at midnight (0 is 0:00, 1 is 0:30, 6am is 12, 5pm is 34 etc)
    float wallTemp; //Wall Temperature
    float airTemp; //Air Temperature
    float airWallDifference; //Difference in temperature between air and wall
    int[] heatingPeriod1; //Set of times the thermostat can be on
    int[] heatingPeriod2; //Second set of times the thermostat can be on

    //Variables
    int wallType;
    int heatTime;
    float targetTemp;

    //UI Elements
    public Dropdown wallList;
    public Dropdown timeList;
    //public Dropdown tempList;
    public Slider tempSlider;

    public Text timeText;
    public Text wallTempText;
    public Text airTempText;
    public Text thermoStatus;
    public Text tempSettingText;

    public Image airImage;
    public Image wallImage;

    public Button startButton;
   
    // Start is called before the first frame update
    void Start()
    {
        UpdateSliderText();
        startButton.interactable = true;
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
        //switch (tempList.value)
        //{
        //    case 0:
        //        targetTemp = 18;
        //        break;
        //    case 1:
        //        targetTemp = 20;
        //        break;
        //    case 2:
        //        targetTemp = 22;
        //        break;
        //    default:
        //        break;
        //}
        targetTemp = tempSlider.value;

        //Reset to starting values
        airTemp = 14;
        wallTemp = 14;
        currentTime = 12; //Set time to 6:00am
        heatingOn = true;
        
        //Get the time period the heating is on for
        heatingPeriod1 = new int[heatTime];
        heatingPeriod2 = new int[heatTime];

        for (int i = 0; i < heatTime; i++)
        {
            heatingPeriod1[i] = 12 + i;
        }
        for (int i = 0; i < heatTime; i++)
        {
            heatingPeriod2[i] = 34 + i;
        }

        //Update displayed values
        UpdateDisplay();
    }

    //Method to advance the current time by half an hour
    public void AdvanceTime()
    {
        currentTime += 1; //Advance time by half an hour
        if (currentTime == 48) //At midnight loop time
        {
            currentTime = 0;
        }
        
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

            //Wall Temperature should not drop below 14 degrees
            if (wallTemp < 14)
            {
                wallTemp = 14;
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

    //Testing method to advance time smoothly
    public void AdvanceTimeAlternative()
    {
               

        if (heatingOn) //If the heating is currently on
        {


            //Calc difference in temperature between air and wall
            airWallDifference = (airTemp - wallTemp);

            //Heat walls based on current air temperature
            if (airWallDifference > 0) //Walls heat up if air is warmer
            {
                switch (wallType) //Heating depends on type of wall. Heating equations divided by 30 to get the change per minute
                {
                    case 0: //Quick
                        wallTemp += (1.5f * airWallDifference / 2.0f) / 30.0f;
                        airTemp -= (0.5f * airWallDifference / 2.0f) / 30.0f;
                        break;
                    case 1: //Medium
                        wallTemp += (1.0f * airWallDifference / 2.0f) / 30.0f;
                        airTemp -= (1.0f * airWallDifference / 2.0f) / 30.0f;
                        break;
                    case 2: //Slow
                        wallTemp += (0.5f * airWallDifference / 2.0f) / 30.0f;
                        airTemp -= (2.0f * airWallDifference / 2.0f) / 30.0f;
                        break;
                    default:
                        break;
                }
            }

            //Air heats up
            airTemp += 4.0f / 30.0f;            

        }
        else //If the heating is off
        {
            switch (wallType) //Heating depends on type of wall
            {
                case 0: //Quick
                    wallTemp -= 0.25f / 30.0f;
                    break;
                case 1: //Medium
                    wallTemp -= 0.5f / 30.0f;
                    break;
                case 2: //Slow
                    wallTemp -= 0.25f / 30.0f;
                    break;
                default:
                    break;
            }

            airTemp -= 1.0f / 30.0f;

            //Air Temperature should not drop below 14 degrees
            if (airTemp < 14)
            {
                airTemp = 14;
            }

            //Wall Temperature should not drop below 14 degrees
            if (wallTemp < 14)
            {
                wallTemp = 14;
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

    public void TestAltMethod()
    {
        startButton.interactable = false;

        RunSim(); //Resets values to start    

        StartCoroutine(NextMinute()); //Runs Simulation over a day
        
        //if (currentTime == 48) //At midnight loop time
        //{
        //    currentTime = 0;
        //}
    }

    IEnumerator NextMinute()
    {
        for (int i = 0; i <= 48; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                AdvanceTimeAlternative();
                yield return new WaitForSecondsRealtime(0.01f);

            }
            currentTime++;
        }

        startButton.interactable = true;
    }

    public void UpdateDisplay()
    {
        airTempText.text = airTemp.ToString("F1");
        wallTempText.text = wallTemp.ToString("F1");
        thermoStatus.text = heatingOn.ToString();
        timeText.text = currentTime.ToString();
        if (currentTime >= 48)
        {
            timeText.text = (Mathf.FloorToInt((currentTime - 48) / 2)).ToString() + ":" + (Mathf.FloorToInt(currentTime % 2) * 30).ToString();
        }
        else
        {
            timeText.text = (Mathf.FloorToInt(currentTime / 2)).ToString() + ":" + (Mathf.FloorToInt(currentTime % 2) * 30).ToString();
        }
        

        //Sets Colours
        float airRed = Mathf.Floor(((airTemp - 14) / (targetTemp - 14)) * 250);
        float airBlue = 255 - airRed;
        Color airCol = new Color(airRed / 255f, 0, airBlue / 255f);
        airImage.color = airCol;

        float wallRed = Mathf.Floor(((wallTemp - 14) / (targetTemp - 14)) * 250);
        float wallBlue = 255 - wallRed;
        Color wallCol = new Color(wallRed / 255f, 0, wallBlue / 255f);
        wallImage.color = wallCol;
    }

    public void UpdateSliderText()
    {
        tempSettingText.text = tempSlider.value.ToString();
    }

    

}