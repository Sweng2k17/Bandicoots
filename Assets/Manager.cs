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
    MeshRenderer beam;
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


	public GameObject oldCube;
    bool isPaused; //Used to determine paused state
    public double time;
    private double speed;
    double interval;
    private CSVReader instance;
    private int position = 1;
    private float difference =-1;
    private int maxPosition;
    private string[,] data;
    private string[,] targetData;
	int numTarget = 1;



    private double[] accelX;
    private double[] accelY;
    private double[] accelZ;
    double[] targetPosX;
    double[] targetPosY;
    double[] targetPosZ;
    int[] targetLeg;
    int[] targetLegPosition;
    public double[] targetVelocityX;
    public double[] targetVelocityY;
    public double[] targetVelocityZ;
    GameObject[] missileObjects;
    MeshRenderer[] missiles;

    private int fileLength = 0;

    //This is the number of ingame units per mile or measuremeant used Currently it is set to 20 units per mile
    private int scalingFactor = 2;

    public void initTarget()
    {
        Debug.Log("Button pressed");
        if (readTargetButton.GetComponent<CSVReader>().data != null)
        {

            targetData = readTargetButton.GetComponent<CSVReader>().data;
            fileLength = targetData.GetLength(1)-1;

            missileObjects = new GameObject[fileLength];
            missiles = new MeshRenderer[fileLength];
            for (int x = 0; x < missileObjects.Length - 1; x++)
            {
                missileObjects[x] = new GameObject();
            }

            for (int x = 0; x < fileLength-1; x++)
            {
                missileObjects[x].AddComponent<MeshRenderer>();
                //ClickScript holder = missle.GetComponent<ClickScript>();
                //missileObjects[x].AddComponent<ClickScript>();
                missiles[x] = missileObjects[x].GetComponent<MeshRenderer>();
                missiles[x] = Instantiate(missle.GetComponent<MeshRenderer>());
                //missiles[x].transform.localScale = missle.transform.localScale;

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
            for (int x = 0; x < fileLength-1 ; x++)
            {
                targetPosX[x] = double.Parse(targetData[0, x + 1])*scalingFactor;
                targetPosY[x] = double.Parse(targetData[1, x + 1])*scalingFactor;
                targetPosZ[x] = double.Parse(targetData[2, x + 1])*scalingFactor;
                targetLeg[x] = int.Parse(targetData[6, x + 1]);

                Debug.Log("Target Position Init " + targetPosX[x]/20 + " " + targetPosY[x]/20 + "  " + targetPosZ[x]/20 + " X = " + x);

                targetLegPosition[x] = 0;

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
        accelX[number] = int.Parse(targetData[number + 1, (targetLegPosition[number] * 4 + 3)]) * scalingFactor;
        accelY[number] = int.Parse(targetData[number + 1, (targetLegPosition[number] * 4 + 4)]) * scalingFactor;
        accelZ[number] = int.Parse(targetData[number + 1, (targetLegPosition[number] * 4 + 5)]) * scalingFactor;
        double[] holder = new double[3];
        holder[0] = accelX[number];
        holder[1] = accelY[number];
        holder[2] = accelZ[number];

        return holder;
    }


    private void disableObjectInfo()
    {
        objectInfo.gameObject.SetActive(false);
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
            Debug.Log("The target data has been run");



            for (int x = 0; x < fileLength-1; x++)
            {

                //if time greater than leg, increase le counter by one 
                if (time / 60 > targetLeg[targetLegPosition[x]])
                {
                    Debug.Log("Target Number " + " Leg Possition Increased to " + targetLegPosition[x]+1);
                    targetLegPosition[x]++;
                }


                //TODO convert mph to seconds
                accelX[x] = double.Parse(targetData[(targetLegPosition[x] * 4 + 3), x + 1])*scalingFactor/60;
                accelY[x] = double.Parse(targetData[(targetLegPosition[x] * 4 + 4), x + 1])*scalingFactor/60;
                accelZ[x] = double.Parse(targetData[(targetLegPosition[x] * 4 + 5), x + 1])*scalingFactor/60;
                Debug.Log("Target Leg Position " + targetLegPosition[x] + "  x " + x);

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
                Debug.Log("Target change in position " + targetPosX[x] + "  " + targetPosY[x] + "    " + targetPosZ[x]);
                


                Vector3 newPos = new Vector3();
                newPos.x = (float)targetPosX[x];
                newPos.y = (float)targetPosY[x];
                newPos.z = (float)targetPosZ[x];



                Debug.Log("Target " + x + " Vel " + targetVelocityX[x] + " Y " + targetVelocityY[x] + "  Z " + targetVelocityZ[x] + " Accel " + accelX[x] + "  " + accelY[x] + "   " + accelZ[x] + "   position" +
                    newPos.ToString());
                missiles[x].transform.position = newPos;

                //missle.transform.position = newPos;

            }


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

    }

    private void updateRadarBeam()
    {
        if(readRadarButton.GetComponent<CSVReader>().data != null)
        {


            data = readRadarButton.GetComponent<CSVReader>().data;

            if (difference < 0)
            {
                difference = float.Parse(data[0, 2]);
            }

            //number of lines in csv file
            maxPosition = data.GetLength(1);
            Debug.Log("The value of maaxPosition is " + data.GetLength(1));

            float distance = float.Parse(data[2, position]);
            Debug.Log("The value of distance is " + data[2, position]);

            //works accross both distances as the speed is light based
            difference = .0107364f * distance;
            Debug.Log("The value of difference is " + difference);
            if (position < maxPosition)
            {
                position = (int)(time / 60 / difference * 1000);
                Debug.Log("The value of position is " + position);


                float degreesRotation = float.Parse(data[6, position]);
                Debug.Log("The value of rotation is " + data[6, position]);
                float degreesElevation = float.Parse(data[7, position]);
                Debug.Log("The value of elevation is is " + data[7, position]);

                if (data[3, position].Equals("1"))
                {
                    //high powered
                    beam.material.color = Color.red;
                }
                else
                {
                    //low powered
                    beam.material.color = Color.cyan;
                }




                //move slider bar with time
                //slider.value = (float)position / (float)maxPosition;




                Vector3 scale = new Vector3();
                scale.x = 2;
                scale.y = 2;
                //scale.z = distance / 100;
				scale.z = 464;

                //nextPos.Scale(scale);

                Vector3 rotation = new Vector3();
                rotation.x = degreesElevation;
                rotation.y = degreesRotation;
                rotation.z = 0;



                beam.transform.transform.rotation = Quaternion.Euler(degreesElevation, degreesRotation, 0);
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

		if (!isPaused)
		{
			updateRadarBeam();
			updateTargetData ();
		}

       

        if (Input.GetMouseButtonDown(0))
        {
            if(fileLength > 0)
            {
                for (int x = 0; x < fileLength-2; x++)
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
}