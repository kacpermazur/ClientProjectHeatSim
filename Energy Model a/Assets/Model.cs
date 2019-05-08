using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    //Model Variables
    bool heatingOn; //If thermo is currently on
    int currentTime; //Current time, in half hours starting at midnight (0 is 0:00, 1 is 0:30, 6am is 12, 5pm is 34 etc)
    float wallTemp; //Wall Temperature
    float airTemp; //Air Temperature
    float airWallDifference; //Difference in temperature between air and wall
    int[] heatingPeriod1; //Set of times the thermostat can be on
    int[] heatingPeriod2; //Second set of times the thermostat can be on
    bool paused;
    int timeWarm; //Number of minutes house is 'warm' for
    int timeHeatingOn; //Total time the heating is on for

    bool boilerOn; //If the boiler is giving off heat after turning off
    int boilerTimeLeft; //Time before the boiler stops giving off heat

    //User Variables
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

    public Image windowImage;

    public Button startButton;
    public Slider timeSlider;
    public Toggle pauseToggle;

    public GameObject statScreen;

    public GameObject settingsMenu;

    public ParticleSystemRenderer radiatorHeat;
   
    // Start is called before the first frame update
    void Start()
    {
        UpdateSliderText();
        startButton.interactable = true;
        paused = false;
        settingsMenu.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void RunSim() //Sets up values for start of simulation
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
        timeWarm = 0;
        timeHeatingOn = 0;

        //Make sky colour dark
        Color darkCol = new Color(0, 0, 0);
        windowImage.color = darkCol;

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

        //settingsMenu.SetActive(false);
        settingsMenu.GetComponent<Animator>().SetBool("DropIn", false);
        settingsMenu.GetComponent<Animator>().SetBool("DropDown", true);
        Camera.main.GetComponent<Animator>().SetBool("CamUp", false);

        //Update displayed values
        UpdateDisplay();
    }

    //Method to advance the current time by half an hour (Old method not in use)
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
            if ((heatingPeriod1[i] == currentTime || heatingPeriod2[i] == currentTime) && airTemp < targetTemp - 1)
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
               

        if (heatingOn || boilerTimeLeft > 0) //If the heating is currently on, or the boiler is still giving off heat
        {

            radiatorHeat.sortingOrder = 20;
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

            if (heatingOn)
            {
                timeHeatingOn++;
            }
            

        }
        else //If the heating is off
        {
            radiatorHeat.sortingOrder = -20;
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

        boilerTimeLeft--; //Reduce the time left for the boiler. Reducing this below 0 is fine as it will be reset when it needs to be turned back on

        //Turns heating on/off at end of time step
        for (int i = 0; i < heatTime; i++)
        {
            if ((heatingPeriod1[i] == currentTime || heatingPeriod2[i] == currentTime) && airTemp < targetTemp - 1)
            {
                heatingOn = true; //Turns heating on if within set heating periods
                

                if (airTemp >= targetTemp && boilerOn == false) //If within heating period and boiler would turn off, turn boiler on
                {
                    boilerOn = true;
                    boilerTimeLeft = 30; //boiler on for half an hour

                }
                else //If temp is lower than required amount
                {
                    boilerOn = false; //Allows the boiler to be turned on again - used so boiler is not always on while within heating time                    
                }
            }
        }
        if (airTemp >= targetTemp)
        {
            heatingOn = false; //Overrides heating setting if current air temperature is above limit

            if (boilerOn == false)
            {
                boilerOn = true;
                boilerTimeLeft = 30; //boiler on for half an hour
            }

        }

        if (airTemp >= 18 && wallTemp >= 17)
        {
            timeWarm++;
        }
        UpdateDisplay();
    }

    public void TestAltMethod() //Sets up and runs simulation
    {
        startButton.interactable = false;

        RunSim(); //Resets values to start    

        StartCoroutine(SimulateDay()); //Runs Simulation over a day
        
        //if (currentTime == 48) //At midnight loop time
        //{
        //    currentTime = 0;
        //}
    }

    IEnumerator SimulateDay() //Simulates a whole day continously
    {
        yield return new WaitForSecondsRealtime(1f); //Wait for a second (for animation purposes)
        for (int i = 0; i <= 48; i++) //48 half hour chunks
        {
            for (int j = 0; j < 30; j++) //30 mins per half hour
            {
                
                AdvanceTimeAlternative(); //Simulates a minute
                yield return new WaitForSecondsRealtime(0.01f * timeSlider.value); //Waits a little before simulating the next minute, so UI can display progress

            }
            currentTime++;
        }

        statScreen.GetComponent<StatsScreen>().OutputStats(timeWarm, timeHeatingOn);

        //settingsMenu.SetActive(false);
        settingsMenu.GetComponent<Animator>().SetBool("DropDown", false);
        //settingsMenu.GetComponent<Animator>().SetBool("DropIn", true);
        Camera.main.GetComponent<Animator>().SetBool("CamDown", true);
        
        startButton.interactable = true; //Sets button to be usable when the simulation is over
    }

    public void UpdateDisplay() //Updates UI elements
    {
        airTempText.text = airTemp.ToString("F0");
        wallTempText.text = wallTemp.ToString("F0");
        thermoStatus.text = heatingOn.ToString();
        timeText.text = currentTime.ToString();
        if (currentTime >= 48) //Wraps time around 24 hour clock
        {
            timeText.text = (Mathf.FloorToInt((currentTime - 48) / 2)).ToString() + ":" + (Mathf.FloorToInt(currentTime % 2) * 30).ToString("00");
        }
        else
        {
            timeText.text = (Mathf.FloorToInt(currentTime / 2)).ToString() + ":" + (Mathf.FloorToInt(currentTime % 2) * 30).ToString("00");
        }


        //Sets colours of air/wall images
        float tempAlpha = airImage.color.a;
        float airRed = Mathf.Floor(((airTemp - 14) / (targetTemp - 14)) * 250); //Generates red value based on percentage of current temperature and target temperature
        float airBlue = 255 - airRed; //Blue value is the inverse of red value
        Color airCol = new Color(airRed / 255f, 0, airBlue / 255f, tempAlpha);
        airImage.color = airCol;

        float wallRed = Mathf.Floor(((wallTemp - 14) / (targetTemp - 14)) * 250);
        float wallBlue = 255 - wallRed;
        Color wallCol = new Color(wallRed / 255f, 0, wallBlue / 255f);
        wallImage.color = wallCol;

        //Change Window Colour
        if (currentTime > 36)
        {
            Color tempCol = new Color(0, 0, 0);
            windowImage.color = Color.Lerp(windowImage.color, tempCol, Time.deltaTime);
        }
        else if (currentTime > 14)
        {
            Color tempCol = new Color(1,1,1);
            windowImage.color = Color.Lerp(windowImage.color, tempCol, Time.deltaTime);
        }

        //Show radiator particles
        //if (heatingOn || boilerTimeLeft >= 0)
        //{
        //    radiatorHeat.SetActive(true);
        //}
        //else
        //{
        //    radiatorHeat.SetActive(false);
        //}

        
    }

    public void UpdateSliderText() //Updates text value above temperature slider
    {
        tempSettingText.text = tempSlider.value.ToString();
    }

    public void Pause()
    {
        paused = pauseToggle.isOn;
    }

    

}
