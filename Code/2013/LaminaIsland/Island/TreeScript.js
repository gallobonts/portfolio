
#pragma strict

var timer:float=3;
function Interaction()
{
timer-=.01;
if(timer<0)
{
this.GetComponent(targetScript).done=true;
timer=3;
}
}