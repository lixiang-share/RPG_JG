#pragma strict

var anims:AnimationClip[];
private var next:float;
private var animName:String;
private var time:float;

function Start () {

next=0;
}

function Update () {

animName=anims[next].name;
time+=Time.deltaTime;

if (!animation.IsPlaying(animName))
{
time=0;
animation.Play(animName);
}

if (time>anims[next].length)
{
next+=1;
if (next==anims.length) next=0;
}



}