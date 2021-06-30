#pragma strict

var hero :GameObject;
var joyStickPlayer:Joystick;



function Start () 
{
	hero = GameObject.Find("Hero");

}
function Update ()
{
if(joyStickPlayer.tapCount >0 )
{
	var joyPosition_x = joyStickPlayer.position.x;
	var joyPosition_y = joyStickPlayer.position.y;
	
	if(joyPosition_y != 0 || joyPosition_x != 0)
	{
		hero.transform.Translate(Vector3.forward* Time.deltaTime * 5);
		hero.transform.LookAt(Vector3(hero.transform.position.x + joyPosition_x,hero.transform.position.y,hero.transform.position.z + joyPosition_y));
		hero.animation.Play("run");
	}
	else
	{
		hero.animation.Play("idle");
	}
}	
	
}