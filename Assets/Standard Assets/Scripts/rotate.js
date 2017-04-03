var speed : int; 
var friction : float; 
var lerpSpeed : float; 
 var xDeg : float; 
 var yDeg : float; 
private var fromRotation : Quaternion; 
private var toRotation : Quaternion;

function Update () { 
	
	if(Input.GetMouseButton(0)) 
	
	RotateTransform(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); 
	
	else RotateTransform(0f, 0f); 
	
	}


function RotateTransform(xNum:float, yNum:float) { 

xDeg -= xNum * speed * friction; 
yDeg -= yNum * speed * friction; 
fromRotation = transform.rotation; 
toRotation = Quaternion.Euler(yDeg,xDeg,0); 
transform.rotation = Quaternion.Lerp(fromRotation,toRotation,Time.deltaTime * lerpSpeed); 

}