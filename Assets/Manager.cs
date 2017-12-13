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
<<<<<<< HEAD
    [SerializeField]
    Transform objectInfo;
=======
	[SerializeField]
	Button aboutButton;
>>>>>>> parent of 40771f3... force mergeMerge remote-tracking branch 'origin/master' into jeff_aboutButton


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

    double[] targetPosX;
    double[] targetPosY;
    double[] targetPosZ;
    int[] targetLeg;
    int[] targetLegPosition;
<<<<<<< HEAD
    public double[] targetVelocityX;
    public double[] targetVelocityY;
    public double[] targetVelocityZ;
    GameObject[] missileObjects;
    MeshRenderer[] missiles;

    private int fileLength = 0;

    //This is the number of ingame units per mile or measuremeant used Currently it is set to 20 units per mile
    private int scalingFactor = 2;


    public void resetTime()
    {
        time = 0;
    }
=======
    double[] targetVelocityX;
    double[] targetVelocityY;
    double[] targetVelocityZ;
    MeshRenderer[] missles;
	Rect windowRect = new Rect(20,20,120,50);
	Rect newRect;
	bool showWindow = false;
>>>>>>> parent of 40771f3... force mergeMerge remote-tracking branch 'origin/master' into jeff_aboutButton

    public void initTarget()
    {
		Debug.Log ("Button pressed");
        if(readTargetButton.GetComponent<CSVReader>().data != null)
        {

            targetData = readTargetButton.GetComponent<CSVReader>().data;
			int fileLength = targetData.GetLength (1);

			Debug.Log("Target data init file length " + fileLength);


			targetPosX = new double[fileLength];
			targetPosY = new double[fileLength];
			targetPosZ = new double[fileLength];
			targetVelocityX = new double[fileLength];
			targetVelocityY = new double[fileLength];
			targetVelocityZ = new double[fileLength];
			targetLeg = new int[targetData.GetLength (0)];
			//quick fix
			//TODO calculate real length
			targetLegPosition = new int[6];


			//so its works but it travels in that direction for 90 seconds not sure if it swtiches to next leg correctly



            //init all targets ignore descriptons line
            for(int x = 0; x < 6-1; x++)
            {
                targetPosX[x] = double.Parse(targetData[0, x+1]);
                targetPosY[x] = double.Parse(targetData[1, x+1]);
                targetPosZ[x] = double.Parse(targetData[2, x+1]);
                targetLeg[x] = int.Parse(targetData[6, x+1]);
                targetLegPosition[x] = 0;
                targetVelocityX[x] = 0;
                targetVelocityY[x] = 0;
                targetVelocityZ[x] = 0;

                //TODO missles[x] = new createMissleObjectHere
            }

			Vector3 newPos = new Vector3 ();
			newPos.x = (float)targetPosX [0];
			newPos.y = (float)targetPosY [0];
			newPos.z = (float)targetPosZ [0];

			missle.transform.position = newPos;
            
        }
    }

	private int accelX;
	private int accelY;
	private int accelZ;

    private void updateTargetData()
    {
        if (readTargetButton.GetComponent<CSVReader>().data != null)
        {
            targetData = readTargetButton.GetComponent<CSVReader>().data;
			Debug.Log("The target data has been run");

            for (int x = 0; x < 6-1; x++)
            {

                //if time greater than leg, increase le counter by one 
                if(time / 60 > targetLeg[targetLegPosition[x]])
                {
                    targetLegPosition[x]++;
                }


				accelX = int.Parse(targetData[ x + 1, (targetLegPosition[x] * 4 + 3)])*20;
				accelY = int.Parse(targetData[x+1,(targetLegPosition[x] * 4 + 4)])*20;
				accelZ = int.Parse(targetData[x+1,(targetLegPosition[x] * 4 + 5)])*20;


				Debug.Log ("Accel " + accelX);
                //update position data on targets

                //time = 1/60 of a second


                //update velocity = previous velocity + accel * time
                targetVelocityX[x] = targetVelocityX[x] + accelX / 60;
                targetVelocityY[x] = targetVelocityY[x] + accelY / 60;
                targetVelocityZ[x] = targetVelocityZ[x] + accelZ / 60;

				Debug.Log ("velocity " + targetVelocityX[0]);

                //distance = intial velocity *t + 1/2 * accel * time * time
                targetPosX[x] = targetVelocityX[x] / 60 + accelX / 60 / 60 / 2;
                targetPosY[x] = targetVelocityY[x] / 60 + accelY / 60 / 60 / 2;
                targetPosZ[x] = targetVelocityY[x] / 60 + accelZ / 60 / 60 / 2;

				Debug.Log ("Target X pos = " + targetPosX [0]);
                //assign target locations to Object missle
                //TODO programatically create Missles equal to X


				Vector3 newPos = new Vector3 ();
				newPos.x = (float)targetPosX [0];
				newPos.y = (float)targetPosY [0];
				newPos.z = (float)targetPosZ [0];

				//Create old position marker
				Vector3 oldPosition = missle.transform.position;
				Quaternion oldRotation = missle.transform.rotation;
				Instantiate (oldCube, oldPosition, oldRotation);

				missle.transform.position = newPos;
                
            }


        }
    }




    void Start()
    {
        UIPanel.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
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
                scale.x = 1;
                scale.y = 1;
                scale.z = distance / 100;

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