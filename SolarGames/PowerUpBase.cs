﻿using UnityEngine;
using System.Collections;

abstract public class PowerUpBase : MonoBehaviour {

    	public bool attached=false;
		protected Transform mesh;
		protected GameObject wep;
		protected bool ReturnToStarting=false;
		protected bool StartAnimation=false;
		protected string rootName;
		protected string playerRootName="Buggy_Tier_2(Clone)";
        public bool IsPressed;
        public bool aiUse=false;

        protected Quaternion rotateBegin;
        protected Vector3 startPosition = new Vector3(0, 0, 0);

	    abstract public void Attach();


	abstract public void Detach();

    public void Attack()
    {

        if (wep.activeSelf)
        {
            aiUse = true;
        }
    }

    protected void StandardAttach()
    {
        StartAnimation = false;
        attached = true;
        wep.SetActive(true);
        IsPressed = false;
        mesh.localPosition = startPosition;
        mesh.localRotation = rotateBegin;
        ReturnToStarting = false;
   
    }

    protected void StandardDetach()
    {
        attached = false;
        wep.SetActive(false);
    }
}
