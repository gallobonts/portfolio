public class Timer
{
	//variables
	float lastTimeSet;
	float timeLeft;
	bool done;
	bool autoRepeat;
	const float INFINITETIME=999999999;

	//constructor
	public Timer()
	{
		timeLeft = 0.0f;
		lastTimeSet = timeLeft;
		done = true;
		autoRepeat=false;
	}
	
	public Timer(float inTime,bool repeat)
	{
		timeLeft = inTime;
		lastTimeSet = timeLeft;
		done = false;
		autoRepeat=repeat;
	}

	public void SetRepeat(bool repeat)
	{
		autoRepeat=repeat;
	}

	//functions
	public void SetTimer(float inTime)
	{
		timeLeft = inTime;
		lastTimeSet = timeLeft;
		
		if (inTime >= 0.0f)
		{
			done = false;
		}
	}
	
	public void ResetTimer()
	{
		timeLeft=lastTimeSet;
	}

	//simply update the timer
	public void Update(float deltaTime)
	{
		if(!done&& timeLeft!=INFINITETIME)
		{
		timeLeft -= deltaTime;
		
		if (timeLeft <= 0.0f)
		{ done = true;
			if (autoRepeat)
			{
				timeLeft = lastTimeSet;
			}
			else
			{
					timeLeft=INFINITETIME;
			}
		}
		}//if not done
	}

	//just return whether it's done
	public bool CheckDone()
	{
		if(done)
		{done= false;
			return true;
		}
		else
		{return false;}
	
	}

	//update the timer and return if it's true
	public bool CheckTime(float deltaTime)
	{

		Update(deltaTime);
		return CheckDone();
	}//end of checkTime function
	
}//end of timer class

