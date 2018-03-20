using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Need this for calling UI scripts

public class Manager : MonoBehaviour
{

    [SerializeField]
    Transform UIPanel; //Will assign our panel to this variable so we can enable/disable it



    [SerializeField]
    Text timeText;
    [SerializeField]
    GameObject beam;
    [SerializeField]
    MeshRenderer beamMaterial;
    [SerializeField]
    Button readRadarButton;
    [SerializeField]
    Slider slider;
    [SerializeField]
    Camera camera;
    [SerializeField]
    MeshRenderer missle;
    [SerializeField]
    Button readTargetButton;
    [SerializeField]
    Transform objectInfo;
	[SerializeField]
	Button aboutButton;

    public GameObject oldCube;
    bool isPaused; //Used to determine paused state
    public double time;
    private double speed;
    double interval;
    private CSVReader instance;
    private int position = 1;
    private float difference = -1;
    private int maxPosition;
    private string[,] data;
    private string[,] targetData;
	Rect windowRect = new Rect(20,20,120,50);
	Rect newRect;
	bool showWindow = false;


    //declare startAz, stopAz, startEl, and stopEl so they can be accessed throughout the script
    private float startAz;
    private float stopAz;
    private float startEl;
    private float stopEl;

    private double[] accelX;
    private double[] accelY;
    private double[] accelZ;
    double[] targetPosX;
    double[] targetPosY;
    double[] targetPosZ;
    //The time until the lef is done read from file
    int[] targetLeg;
    //The location of the Leg in the file
    int[] targetLegPosition;
    public double[] targetVelocityX;
    public double[] targetVelocityY;
    public double[] targetVelocityZ;
    GameObject[] missileObjects;
    MeshRenderer[] missiles;
    // Alpha decrement to make the targets slowly go transparent
    float alphaDec = 0.0035f;

    private int fileLength = 0;

    //This is the number of ingame units per mile or measuremeant used Currently it is set to 20 units per mile
    private int scalingFactor = 2;


    public void resetTime()
    {
        time = 1;
        difference = -1;
        position = 1;
        data = readRadarButton.GetComponent<CSVReader>().data;

        initializeAngles();
    }

    public void initTarget()
    {

        if(missileObjects != null)
        {
            for(int x = 0; x<missileObjects.Length; x++)
            {
                DestroyObject(missileObjects[x]);
                Destroy(missiles[x]);
            }
        }

        time = 1;
        Debug.Log("Button pressed");
        if (readTargetButton.GetComponent<CSVReader>().data != null)
        {
            Debug.Log("Read target csv reader not null");
            targetData = readTargetButton.GetComponent<CSVReader>().data;
            fileLength = targetData.GetLength(1) - 1;

            missileObjects = new GameObject[fileLength];
            missiles = new MeshRenderer[fileLength];
            for (int x = 0; x < missileObjects.Length - 1; x++)
            {
                missileObjects[x] = new GameObject();
            }

            for (int x = 0; x < fileLength - 1; x++)
            {
                missileObjects[x].AddComponent<MeshRenderer>();
                missileObjects[x].AddComponent<MeshCollider>();
                missileObjects[x].AddComponent<ClickScript>();
                missiles[x] = missileObjects[x].GetComponent<MeshRenderer>();
                missiles[x] = Instantiate(missle.GetComponent<MeshRenderer>());
                missiles[x].GetComponent<ClickScript>().setNumber(x);

                //changes here
                //changes here
                //changes here
                missiles[x].enabled = false;

                Vector3 scale = new Vector3();
                scale.x = .1f;
                scale.y = .1f;
                scale.z = .1f;
                missiles[x].transform.localScale = scale;
            }

            //can tasks be marked as X percent complete
            Debug.Log("Target data init file length " + fileLength);

            fileLength--;
            targetPosX = new double[fileLength];
            targetPosY = new double[fileLength];
            targetPosZ = new double[fileLength];
            targetVelocityX = new double[fileLength];
            targetVelocityY = new double[fileLength];
            targetVelocityZ = new double[fileLength];
            targetLeg = new int[targetData.GetLength(0)];
            accelX = new double[fileLength];
            accelY = new double[fileLength];
            accelZ = new double[fileLength];

            targetLegPosition = new int[fileLength];
            fileLength++;

            //init all targets ignore descriptons line
            for (int x = 0; x < fileLength - 1; x++)
            {
                targetPosX[x] = double.Parse(targetData[0, x + 1]) * scalingFactor;
                targetPosY[x] = double.Parse(targetData[1, x + 1]) * scalingFactor;
                targetPosZ[x] = double.Parse(targetData[2, x + 1]) * scalingFactor;
                targetLegPosition[x] = 6;
                targetLeg[x] = int.Parse(targetData[targetLegPosition[x], x + 1]);

                Debug.Log("Target Position Init " + targetPosX[x] / 20 + " " + targetPosY[x] / 20 + "  " + targetPosZ[x] / 20 + " X = " + x);


                targetVelocityX[x] = 0;
                targetVelocityY[x] = 0;
                targetVelocityZ[x] = 0;

                Vector3 newPos = new Vector3();
                newPos.x = (float)targetPosX[x];
                newPos.y = (float)targetPosY[x];
                newPos.z = (float)targetPosZ[x];

                missiles[x].transform.position = newPos;

            }



        }
        Debug.Log("Init Done");
    }

    public Vector3 getPosition(int number)
    {
        Vector3 newPos = new Vector3();
        newPos.x = (float)targetPosX[number];
        newPos.y = (float)targetPosY[number];
        newPos.z = (float)targetPosZ[number];

        return newPos;
    }

    public double[] getVelocity(int number)
    {

        double[] holder = new double[3];
        holder[0] = targetVelocityX[number];
        holder[1] = targetVelocityY[number];
        holder[2] = targetVelocityZ[number];
        return holder;
    }

    public double[] getAcceleration(int number)
    {
        accelX[number] = double.Parse(targetData[(targetLegPosition[number] -3), number + 1]) * scalingFactor;
        accelY[number] = double.Parse(targetData[(targetLegPosition[number] -2), number + 1]) * scalingFactor;
        accelZ[number] = double.Parse(targetData[(targetLegPosition[number] -1), number + 1]) * scalingFactor;
        double[] holder = new double[3];
        holder[0] = accelX[number];
        holder[1] = accelY[number];
        holder[2] = accelZ[number];

        return holder;
    }


    private void disableObjectInfo()
    {
        objectInfo.gameObject.SetActive(false);
        for (int x = 0; x < fileLength - 1; x++)
        {
            missiles[x].GetComponent<ClickScript>().setActive(false);
        }
    }

    public void enableObjectInfo()
    {
        objectInfo.gameObject.SetActive(true);

    }






    private void updateTargetData()
    {
        if (readTargetButton.GetComponent<CSVReader>().data != null)
        {
            //targetData = readTargetButton.GetComponent<CSVReader>().data;
            //Debug.Log("The target data has been run");



            for (int x = 0; x < fileLength - 1; x++)
            {

                try
                {
                    //if time greater than leg, increase le counter by one 
                    if (time / 60 > targetLeg[x])
                    {
                        targetLegPosition[x] += 4;
                        targetLeg[x] = int.Parse(targetData[targetLegPosition[x], x + 1]) + targetLeg[x];
                        Debug.Log("Target Number " + x + " Leg Possition Increased to " + targetLegPosition[x]);

                    }
               
               


                //TODO convert mph to seconds
                accelX[x] = double.Parse(targetData[(targetLegPosition[x] - 3), x + 1]) * scalingFactor / 60;
                accelY[x] = double.Parse(targetData[(targetLegPosition[x] - 2), x + 1]) * scalingFactor / 60;
                accelZ[x] = double.Parse(targetData[(targetLegPosition[x] - 1), x + 1]) * scalingFactor / 60;
                // Debug.Log("Target Leg Position " + targetLegPosition[x] + "  x " + x);

                //update position data on targets

                //time = 1/60 of a second


                //update velocity = previous velocity + accel * time
                targetVelocityX[x] = targetVelocityX[x] + accelX[x] / 60;
                targetVelocityY[x] = targetVelocityY[x] + accelY[x] / 60;
                targetVelocityZ[x] = targetVelocityZ[x] + accelZ[x] / 60;


                //distance = intial velocity *t + 1/2 * accel * time * time
                targetPosX[x] += targetVelocityX[x] / 60 + accelX[x] / 60 / 60 / 2;
                targetPosY[x] += targetVelocityY[x] / 60 + accelY[x] / 60 / 60 / 2;
                targetPosZ[x] += targetVelocityY[x] / 60 + accelZ[x] / 60 / 60 / 2;
                // Debug.Log("Target change in position " + targetPosX[x] + "  " + targetPosY[x] + "    " + targetPosZ[x]);



                Vector3 newPos = new Vector3();
                newPos.x = (float)targetPosX[x];
                newPos.y = (float)targetPosY[x];
                newPos.z = (float)targetPosZ[x];



                //Debug.Log("Target " + x + " Vel " + targetVelocityX[x] + " Y " + targetVelocityY[x] + "  Z " + targetVelocityZ[x] + " Accel " + accelX[x] + "  " + accelY[x] + "   " + accelZ[x] + "   position" +
                //   newPos.ToString());
                missiles[x].transform.position = newPos;

                    //missle.transform.position = newPos;                   
                    
                }
                catch (System.Exception e)
                {
            //COMMENTED OUT THIS DEBUG
	 		//Debug.Log("Error in target data, not enough data please enter empty data set for target legs");
		        }

                // Adjust missile's alpha value:
                if (missiles[x].enabled == true)
                {
                    Debug.Log("decAlpha being called");
                    decAlpha(missiles[x]);
                }
                //if((time % 400) == 0) { resetAlpha(missiles[x]); }

            }
        }
    }

    /**
     *  decAlpha(MeshRenderer target)
     * 
     *  PRE: Takes in a target's mesh renderer data
     *  
     *  POST: decrements the target's alpha value, making it slowly go transparent. 
     *  when the alpha is 0, the mesh rendering is disabled making the target no longer visible.
     *  (Pat M. 02.21.2018)
     **/
    private void decAlpha(MeshRenderer target)
    {
        Color colour = target.material.color;

        // If the alpha value is 0 or less for the target, disable its rendering 
        if (colour.a <= 0)
        {
            target.enabled = false;
        }
        // Otherwise, continue making the target transparent
        else
        {
            colour.a -= alphaDec;
            target.material.color = colour;
        }
    }

    void Start()
    {
        UIPanel.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
        objectInfo.gameObject.SetActive(false);

        isPaused = false; //make sure isPaused is always false when our scene opens
        time = 0;
        timeText.text = time.ToString();

        //adjust speed here
        speed = 1;
        interval = speed;
		aboutButton.onClick.AddListener (TaskOnClick);

    }

    private void updateRadarBeam()
    {
        if (data != null)
        {
            if (difference < 0)
            {
                //USED FOR Start sheet.csv
                //difference = float.Parse(data[0, 2]);
                difference = 0.644181649f;
            }

            //number of lines in csv file
            //CSVReader reads in empty line at end of csv files so last line is not included
            maxPosition = data.GetLength(1)-1;

            float distance = float.Parse(data[2, position]);

            //works accross both distances as the speed is light based
            difference = .0107364f * distance;
            if (position < maxPosition)
            {
                //TODO multiple by 10 time is artificially slowed so that you can see the beam
                //position = (int)(time / 60 / difference * 100);

                //COMMENTED OUT THE DEBUG
                //Debug.Log("The value of position is " + position);

                if (data[3, position].Equals("1"))
                {
                    //high powered
                    beamMaterial.material.color = Color.red;
                }
                else
                {
                    //low powered
                    beamMaterial.material.color = Color.cyan;
                }

                //move slider bar with time
                //slider.value = (float)position / (float)maxPosition;

                Vector3 scale = new Vector3();
                scale.x = 2;
                //scale.y = 2;
                scale.y = distance / 10;
                scale.z = 2;

                //nextPos.Scale(scale);

                //rotation vector, may not need
                /*Vector3 rotation = new Vector3();
                rotation.x = degreesElevation;
                rotation.y = degreesRotation;
                rotation.z = 0;*/

                beam.transform.transform.rotation = Quaternion.Euler(0, startAz, 0);
                Debug.Log("Beam Updating");
                beam.transform.transform.localScale = scale;
            }
            else
            {
                //file is done reading
            }

            //update postion of beam 8 lines of data per column
            //time, start range, end range, high power, number of pulses, width, azimuth degrees, Elvation degrees
        }
        else
        {
            position = 1;
        }
    }

    void Update()
    {
        try { 
        if (time < 0)
        {
            time = 0;
        }

        if (!isPaused)
        {
                //BEAM UPDATING PROCEDURE:
                //There will be a desired start elevation and stop elevation that the beam will need to search.
                //The beam starts at the start elevation and performs an enitre revolution around the ship at the elevation.
                //The beam elevation is then incremented after an entire revolution is performed. This procedure is repeated for each degree
                //increment in elevation. Each time Update() is executed, the beam should revolve 1 degree around the ship. The degrees of revolution
                //around the ship are reflected in the azimuth. If the start azimuth equals the stop azimuth, that means an enitre revolution was performed.
                //The elevation of the beam is then incremented and the azimuth is reset.

                //both radar beam and target data get updated if there are still more unread lines in the beam data file
                //otherwise just the target data will be updated.
                if (position < maxPosition)
                {
                    //the elevation of the search beam is increasing in the desired search area.
                    if (startEl <= stopEl)
                    {
                        //the azimuth is increasing
                        if (startAz <= stopAz)
                        {
                            //beam and targets are updated for every increment in the azimuth
                            Debug.Log("Increasing, Increasing");
                            updateRadarBeam();
                            updateTargetData();
                            //azimuth has completed a full revolution, so now the elevation can be incremented.
                            if (startAz == stopAz)
                            {
                                //a full revolution has been completed for every increment in elevation
                                if (startEl == stopEl)
                                {
                                    //the position can now be incremented and the azimuths and elevations can be re-initialized based on the incremented position
                                    position++;
                                    initializeAngles();
                                }
                                //a full revolution has been completed and the elevation needs to be incremented
                                else
                                {
                                    startEl = startEl + 1f;
                                    //the start azimuth needs to be reset so another revolution can be performed
                                    startAz = float.Parse(data[4, position]);
                                }
                            }
                            else
                            {
                                //increment the azimuth 1 degree as a full revolution has not been completed yet
                                startAz = startAz + 1f;
                            }
                        }
                        //the azimuth is decreasing
                        else
                        {
                            //beam and targets are updated for every decrement in the azimuth
                            Debug.Log("Increasing, Decreasing");
                            updateRadarBeam();
                            updateTargetData();
                            //azimuth has completed a full revolution, so now the elevation can be incremented.
                            if (startAz == stopAz)
                            {
                                //a full revolution has been completed for every increment in elevation
                                if (startEl == stopEl)
                                {
                                    //the position can now be incremented and the azimuths and elevations can be re-initialized based on the incremented position
                                    position++;
                                    initializeAngles();
                                }
                                //a full revolution has been completed and the elevation needs to be incremented
                                else
                                {
                                    startEl = startEl + 1f;
                                    //the start azimuth needs to be reset so another revolution can be performed
                                    startAz = float.Parse(data[4, position]);
                                }
                            }
                            else
                            {
                                //decrement the azimuth 1 degree as a full revolution has not been completed yet
                                startAz = startAz - 1f;
                            }
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //the elevation of the search beam is decreasing in the desired search area.
                    else
                    {
                        //the azimuth is increasing
                        if (startAz <= stopAz)
                        {
                            //beam and targets are updated for every increment in the azimuth
                            Debug.Log("Decreasing, Increasing");
                            updateRadarBeam();
                            updateTargetData();
                            //azimuth has completed a full revolution, so now the elevation can be decremented.
                            if (startAz == stopAz)
                            {
                                //a full revolution has been completed for every decrement in elevation
                                if (startEl == stopEl)
                                {
                                    //the position can now be incremented and the azimuths and elevations can be re-initialized based on the incremented position
                                    position++;
                                    initializeAngles();
                                }
                                //a full revolution has been completed and the elevation needs to be decremented
                                else
                                {
                                    startEl = startEl - 1f;
                                    //the start azimuth needs to be reset so another revolution can be performed
                                    startAz = float.Parse(data[4, position]);
                                }
                            }
                            else
                            {
                                //increment the azimuth 1 degree as a full revolution has not been completed yet
                                startAz = startAz + 1f;
                            }
                        }
                        //the azimuth is decreasing
                        else
                        {
                            //beam and targets are updated for every decrement in the azimuth
                            Debug.Log("Decreasing, Decreasing");
                            updateRadarBeam();
                            updateTargetData();
                            //azimuth has completed a full revolution, so now the elevation can be decremented.
                            if (startAz == stopAz)
                            {
                                //a full revolution has been completed for every decrement in elevation
                                if (startEl == stopEl)
                                {
                                    //the position can now be incremented and the azimuths and elevations can be re-initialized based on the incremented position
                                    position++;
                                    initializeAngles();
                                }
                                //a full revolution has been completed and the elevation needs to be decremented
                                else
                                {
                                    startEl = startEl - 1f;
                                    //the start azimuth needs to be reset so another revolution can be performed
                                    startAz = float.Parse(data[4, position]);
                                }
                            }
                            else
                            {
                                //decrement the azimuth 1 degree as a full revolution has not been completed yet
                                startAz = startAz - 1f;
                            }
                        }
                    }
                }
                else
                {
                    updateTargetData();
                }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (fileLength > 0)
            {
                for (int x = 0; x < fileLength - 2; x++)
                {
                    missiles[x].GetComponent<ClickScript>().setActive(false);
                }
            }
            disableObjectInfo();
        }

        //If player presses escape and game is not paused. Pause game. If game is paused and player presses escape, unpause.
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pause();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
            {
                UnPause();
            }
        }
        time += interval;
        timeText.text = ((int)time).ToString();

        }
        catch(System.IndexOutOfRangeException e)
        {
            Debug.Log("Error in data file");
        }

    }






    public void Pause()
    {
        isPaused = true;
        pauseTime();
        UIPanel.gameObject.SetActive(true); //turn on the pause menu
        camera.GetComponent<CameraMovement_script>().isPaused = false;
        objectInfo.gameObject.SetActive(false);

    }

    public void UnPause()
    {
        isPaused = false;
        UIPanel.gameObject.SetActive(false); //turn off pause menu
        camera.GetComponent<CameraMovement_script>().isPaused = true;

        play();
    }

    public void ExitProgram()
    {
        Application.Quit();
    }

    public void Fastforward()
    {
        interval = 2 * speed;
    }

    public void Rewind()
    {
        interval = -2 * speed;
    }

    public void play()
    {
        interval = speed;

    }

    public void pauseTime()
    {
        interval = 0;
    }

    //called from slider object
    public void setTime()
    {


        double sliderPosition = slider.value;
        position = (int)(sliderPosition * maxPosition);
        time = position * 60 / 1000 * difference;
    }


	void OnGUI()
	{

		if (showWindow==true) {
			windowRect.width = 200;
			windowRect.height = 50;
			windowRect.x = ((Screen.width - windowRect.width) / 2);
			windowRect.y = ((Screen.height - windowRect.height) / 2) ;
			windowRect = GUI.Window (0, windowRect, DoMyWindow, "VR Search Visualization");
		}


	}


	void DoMyWindow(int windowID) {
		if (GUI.Button (new Rect (10, 20, 180, 20), "Version: 0.1"))
        {
            showWindow = false;
        }
			print ("got a click");

	}





	void TaskOnClick()
	{
		if (showWindow) {
			showWindow = false;
		} else {
			showWindow = true;
		}

		Debug.Log ("show window is: " + showWindow);
	}

    private void initializeAngles()
    {
        //store initial values of startAz, stopAz, startEl, and stopEl so they can be accessed in Update()
        startAz = float.Parse(data[4, position]);
        stopAz = float.Parse(data[5, position]);
        startEl = float.Parse(data[6, position]);
        stopEl = float.Parse(data[7, position]);
    }
}
