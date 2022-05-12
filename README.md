# EyeTrackingUnity
Eye Tracking or Gaze Tracking technique is getting popular recent years. Thanks to the state of art VR techs, some VRs offer high accurarcy eye tracking. \
This is an experimental project that I test on Unity platform to explore some kinds of "playable" way with eye tracking techniques. 

### Problem Statement: 
In many racing games, we could switch our views from third-person to first-person. \
However, in first-person view, players usually have to look the actual 3D models of rear-view mirror or speedometer, which is, sometimes really hard to see. \
If we always show the big UI or big mirror on the screen that will somehow distract players. \
I think we may use eye tracking to display or zoom in the UI/Button/Models that users want to look. 

### Pipeline/Workflow:
I'm using a OpenSource software for eye tracking and transmit the eye position data to Unity game engine through TCP socket.\ 
[GazeFlow](https://gazerecorder.com/)
<pre>
<b>GazeFlow</b> ---→ <b>TCP socket</b>  --→ <b>Unity</b> 
</pre>


### Gameplay:
##### Using eye tracking to zoom in the UI or Mirror when player watching.
