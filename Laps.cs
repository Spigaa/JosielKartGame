using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Laps : MonoBehaviour
{
	public Transform[] checkPointArray;
	public Transform[] checkpointA;
	public int currentCheckpoint = 0;
	public int currentLap = 0;
	public int Lap;
	public GameObject points;
	public GameObject voltas;
	public GameObject checkT;
	public GameObject _canvas;

	//public float playerDistance;
	//public Vector3 startPos;

	void Start()
	{
		this.currentCheckpoint = 0;
		this.currentLap = 0;
		this.Lap = this.currentLap;
		this.checkpointA = this.checkPointArray;
		this.voltas = GameObject.Find("HUD/Canvas/Laps/Voltas");
		//this.checkT = GameObject.Find("HUD/Canvas/CheckPoint");

		if (GameObject.Find("Checkpoints"))
		{
			this.points = GameObject.Find("Checkpoints");
			this.checkPointArray = new Transform[this.points.transform.childCount];
			for (int i = 0; i < this.points.transform.childCount; i++)
			{
				this.checkPointArray[i] = this.points.transform.GetChild(i);
			}
		}
	}

	void Update()
	{
		this.Lap = this.currentLap;
		this.checkpointA = this.checkPointArray;
		this.checkpointA[currentCheckpoint].transform.GetComponent<MeshRenderer>().enabled = true;


		if (this.currentLap > 0)
		{
			this.voltas.GetComponent<Text>().text = this.currentLap.ToString() + " / 3";
		}

		if (this.Lap == 4)
		{
			_canvas = GameObject.Find("Canvas");
			_canvas.SetActive(false);

			this.voltas.GetComponent<Text>().text = "3 / 3";
			this.gameObject.GetComponent<PlayerScript>().hasEndedRace = true;

			this.transform.GetChild(1).gameObject.SetActive(true);
			this.checkpointA[currentCheckpoint].transform.GetComponent<MeshRenderer>().enabled = false;
			this.checkpointA[currentCheckpoint].transform.GetComponent<BoxCollider>().enabled = false;
		}
	}

	void OnTriggerEnter(Collider coll)
	{
        if (this.gameObject)
        {
			if (coll.gameObject.tag == ("Checkpoint"))
			{
				if (this.points.transform.GetChild(this.currentCheckpoint).transform == this.checkpointA[this.currentCheckpoint].transform)
				{
					if (this.currentCheckpoint < this.checkpointA.Length)
					{
						if (this.currentCheckpoint == 0 && coll.gameObject.name == "CheckpointSingle")
						{
							this.currentLap++;
							if (this.currentLap == 4)
							{
								this.gameObject.GetComponent<PlayerScript>().endRace();
							}
						}
						this.checkpointA[currentCheckpoint].transform.GetComponent<MeshRenderer>().enabled = false;
						this.checkpointA[currentCheckpoint].transform.GetComponent<BoxCollider>().enabled = false;
						this.currentCheckpoint++;
						if (coll.gameObject.name == "CheckpointSingle (10)")
						{
							this.currentCheckpoint = 0;
						}
						this.checkpointA[currentCheckpoint].transform.GetComponent<MeshRenderer>().enabled = true;
						this.checkpointA[currentCheckpoint].transform.GetComponent<BoxCollider>().enabled = true;

						
					}
					else
					{
						this.currentCheckpoint = 0;
					}
				}	
			}
		}
	}
}

// ---------------------------------------------------------------------------//

/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class Laps : MonoBehaviour
{
	private PhotonView photonview;
	//public List<Transform> checkPointArray = new List<Transform>();
	public Transform[] checkPointArray;
	public Transform[] checkpointA;
	public int currentCheckpoint = 0;
	public int currentLap = 0;
	public Vector3 startPos;
	public int Lap;

	void Start()
	{
		startPos = transform.position;
		currentCheckpoint = 0;
		currentLap = 0;
	}

	void Update()
	{
		Lap = currentLap;
		checkpointA = checkPointArray;
		checkpointA[currentCheckpoint].transform.GetComponent<MeshRenderer>().enabled = true;
		if (Lap == 5)
		{
			Debug.Log("Acabou");
		}
	}
}*/

