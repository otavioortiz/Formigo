using jp.nyatla.nyartoolkit.cs.markersystem;
using NyARUnityUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CaptureScreen : MonoBehaviour {

    private NyARUnityMarkerSystem _ms;
    private NyARUnityWebCam _ss;
    private int mid;//marker id
    private GameObject _bg_panel;
    private ArrayList midList;
    private Renderer mr;
    bool markerExits;

    private int currentAntId;
    private int capturedAntId;
    private int lastTexturedAntId;

    private GameObject bCapture;
    private DateTime currentDate;

    private GameObject antModel;

    private WebCamTexture w;
    private bool initialized = false;

    void Awake()
    {
        Backend.getInstance().requestCurrentTime(requestTimeComplete);

        antModel = GameObject.Find("AntModel");
        mr = GameObject.Find("modelBody").GetComponent<SkinnedMeshRenderer>();

        midList = new ArrayList();

        //setup unity webcam
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length <= 0)
        {
            Debug.LogError("No Webcam.");
            return;
        }

        w = new WebCamTexture(320, 240, 15);
        w.Play();

        return;
    }

    private void init()
    {
        //Make WebcamTexture wrapped Sensor.
        this._ss = NyARUnityWebCam.createInstance(w);

        //Make configulation by Sensor size.
        NyARMarkerSystemConfig config = new NyARMarkerSystemConfig(this._ss.width, this._ss.height);

        this._ms = new NyARUnityMarkerSystem(config);

        for (int i = 0; i < DataManager.getInstance().getAntList().Count; i++)
            if (!DataManager.getInstance().userData.verifyAntCaptured(DataManager.getInstance().getAntList()[i].id))
                midList.Add((int)this._ms.addNyIdMarker(DataManager.getInstance().getAntList()[i].arGraphicId, 10));

        //setup background
        this._bg_panel = GameObject.Find("Plane");
        this._bg_panel.GetComponent<Renderer>().material.mainTexture = w;
        this._ms.setARBackgroundTransform(this._bg_panel.transform);

#if UNITY_IOS
        this._bg_panel.transform.localScale = new Vector3(this._bg_panel.transform.localScale.x, this._bg_panel.transform.localScale.y, this._bg_panel.transform.localScale.z * -1);
#endif

        //setup camera projection
        this._ms.setARCameraProjection(this.GetComponent<Camera>());

        this._ss.start();
    }

    // Use this for initialization
    void Start()
    {
        bCapture = GameObject.Find("bCapture");
        bCapture.SetActive(false);

        GameObject.Find("Canvas").transform.Rotate(new Vector3(0,0,90));

        //start sensor
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void requestTimeComplete()
    {
        Debug.Log("===> " + Backend.getInstance().jsonTime.time);
        currentDate = Convert.ToDateTime(Backend.getInstance().jsonTime.time);
    }

    public void clickInReturn(){
        Screen.orientation = ScreenOrientation.Portrait;
        this._ss.stop();
        SceneManager.LoadScene("GameMenu");
    }

    public void clickInCapture(){

        antModel.SetActive(false);

        GameObject.Find("Particles").GetComponent<ParticleSystem>().Emit(50);
        GameObject.Find("Particles").GetComponent<AudioSource>().Play();
        capturedAntId = currentAntId;
        DataManager.getInstance().userData.addAnt(capturedAntId);

        InvokeRepeating("continueCapture", 0.5f, 0);
    }

    private void continueCapture()
    {
        Backend.getInstance().setLockScreen(true);
        Backend.getInstance().requestCaptureAnt(captureAntComplete, DataManager.getInstance().userData.id, capturedAntId);
    }

    private void captureAntComplete()
    {
        Debug.Log(">>>>>>>>>> " + capturedAntId);
        int currentScore = DataManager.getInstance().getAntData(capturedAntId).rarity * 100;
        DataManager.getInstance().userData.score += currentScore;
        Backend.getInstance().requestAddScore(addScoreComplete, DataManager.getInstance().userData.id, currentScore);
    }

    private void addScoreComplete()
    {
        Backend.getInstance().setLockScreen(false);
        Screen.orientation = ScreenOrientation.Portrait;
        this._ss.stop();

        DataManager.getInstance().setCurrentShowedAntId(capturedAntId);
        SceneManager.LoadScene("AntDataScreen");
    }

    // Update is called once per frame
    void Update()
    {
        long currentgraphicId;

        if (Input.GetKeyUp(KeyCode.Escape))
            clickInReturn();

        if (w.width > 16)
        {
            if (!initialized)
            {
                initialized = true;
                init();
            }

            //Update SensourSystem
            this._ss.update();

            //Update marker system by ss
            this._ms.update(this._ss);

            markerExits = false;
            for (int i = 0; i < midList.Count; i++)
            {
                if (this._ms.isExistMarker((int)midList[i]))
                {
                    if (!bCapture.activeSelf)
                    {
                        currentgraphicId = _ms.getNyId((int)midList[i]);

                        //currentAntId = DataManager.getInstance().getAntList()[i].id;
                        currentAntId = DataManager.getInstance().getAntDataByGraphicId((int)currentgraphicId).id;

                        bool inDate = true;

                        if (DataManager.getInstance().getAntData(currentAntId).startTime != "" && DataManager.getInstance().getAntData(currentAntId).startTime != null && DataManager.getInstance().getAntData(currentAntId).startTime != "0000-00-00 00:00:00")
                            if (DataManager.getInstance().getAntData(currentAntId).endTime != "" && DataManager.getInstance().getAntData(currentAntId).endTime != null && DataManager.getInstance().getAntData(currentAntId).endTime != "0000-00-00 00:00:00")
                            {
                                DateTime startAntTime = Convert.ToDateTime(DataManager.getInstance().getAntData(currentAntId).startTime);
                                DateTime endAntTime = Convert.ToDateTime(DataManager.getInstance().getAntData(currentAntId).endTime);

                                if (currentDate.CompareTo(startAntTime) < 0 || currentDate.CompareTo(endAntTime) > 0)
                                    inDate = false;
                            }

                        if (inDate)
                        {
                            if (DataManager.getInstance().userData.verifyAntCaptured(currentAntId) == true)
                            {
                                bCapture.SetActive(false);
                            }
                            else
                            {
                                if (!antModel.active)
                                    antModel.SetActive(true);

                                bCapture.SetActive(true);
                            }


                            if (currentAntId != lastTexturedAntId)
                                mr.material.mainTexture = null;

                            if (mr.material.mainTexture == null)
                            {
                                lastTexturedAntId = currentAntId;
                                mr.material.mainTexture = DataManager.getInstance().getAntData(currentAntId).getTexture();
                            }
                        }
                    }

                    if (bCapture.activeSelf)
                        this._ms.setTransform((int)midList[i], GameObject.Find("MarkerObject").transform);

                    markerExits = true;
                    break;
                }
                else
                {
                    currentAntId = -1;
                    if (bCapture.activeSelf)
                        bCapture.SetActive(false);
                }
            }

            if (!markerExits)
                GameObject.Find("MarkerObject").transform.localPosition = new Vector3(0, 0, -100);

            c++;
        }
    }
    static int c = 0;
}
