// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


Microsoft.js.ui.EmoticonPicker = function()
{   
    this.emoticon_url = "images/emoticons/";	
	this.uiElement = document.createElement("DIV");
    this.uiElement.className = "EmoticonPicker";
    this.observers = new Array();
    this.mouseOutCount = 0;
    
    this.ecells = new Array();
    
	for (var i=0;i < emoticonsArray.length; i++)
	{
		var row;
	
		if (i%8==0)
		{
			row = document.createElement("div");
			row.className = "EmoticonRow";
			this.uiElement.appendChild(row);
		}
	    
	    var ecell = new Microsoft.js.ui.EmoticonCell(emoticonsArray[i],this);
	    this.ecells.push(ecell);
	        
		row.appendChild(ecell.uiElement);
	}
	
	this.MouseOut = new Microsoft.js.event.EventHandler(this.uiElement, 'mouseout', 'OnMouseOut',this, false);
	Microsoft.js.event.EventManager.add(this.MouseOut);
}


Microsoft.js.ui.EmoticonCell = function(imageSrc,parentObj)
{
		this.uiElement = document.createElement("img");
		this.uiElement.className = "EmoticonCell";
		this.uiElement.tabIndex=0;
		 
		if (browser.isMicrosoft)
		    this.uiElement.unselectable = "on"; 	 
		
        this.KeyPress = new Microsoft.js.event.EventHandler(this.uiElement, 'keypress', 'OnKeyPress', parentObj, false);
        this.Click = new Microsoft.js.event.EventHandler(this.uiElement, 'click', 'OnSelect', parentObj, false);
 
        Microsoft.js.event.EventManager.add(this.KeyPress);
        Microsoft.js.event.EventManager.add(this.Click);
		this.uiElement.setAttribute('src',imageSrc);		
}


Microsoft.js.ui.EmoticonPicker.prototype.OnMouseOut = function(e)
{
  if(e != null && e.toElement != null && e.srcElement != null)
   this.HideOnIE(e);
  else if(e && e.relatedTarget)
   this.HideOnFireFox(e);
   
   return false;
}

Microsoft.js.ui.EmoticonPicker.prototype.OnKeyPress = function(e)
{
  if (e.keyCode==13)
    this.OnSelect(e);

  event.cancelDefault = true;
  return false;
  
}

Microsoft.js.ui.EmoticonPicker.prototype.OnSelect = function(e)
{
   var src = e.srcElement || e.target;
   var imgSrc = src.src;
   
   this.OnChange(imgSrc);
      
   return true;
}

Microsoft.js.ui.EmoticonPicker.prototype.Show = function(parentId)
{
  this.parentElement = document.getElementById(parentId);
  
  var p = this.parentElement;
  var c = this.uiElement;

  var top  = p.offsetHeight+1;
  var left = 0;

  for (; p; p = p.offsetParent)
  {
    top  += p.offsetTop;
    left += p.offsetLeft;
  }
 
  left -= 180;
  top -= 2;
  c.style.position   = "absolute";
  c.style.top        = top +'px';
  c.style.left       = left+'px';
  c.style.visibility = "visible";
}

Microsoft.js.ui.EmoticonPicker.prototype.HideOnIE = function(e)
{ 
 var c = this.uiElement;
 
 if (   (e.srcElement.className == "EmoticonCell" && e.toElement.className == "EmoticonPicker")
     || (e.toElement.className == "EmoticonCell" && e.srcElement.className == "EmoticonPicker") 
     || (e.toElement.className == "EmoticonCell" && e.srcElement.className == "EmoticonCell"))
 {
    e.cancelBubble = true;
    e.returnValue = false;
    return false;
 }
  
  c.style.visibility = 'hidden';
}

Microsoft.js.ui.EmoticonPicker.prototype.HideOnFireFox = function(e)
{
 var c = this.uiElement;

   if(e.currentTarget.className == "EmoticonPicker" &&
           e.relatedTarget.className != "EmoticonRow" &&
           e.relatedTarget.className != "EmoticonCell" &&
	       e.currentTarget.className != e.relatedTarget.className)
        {
            this.Hide();
            e.returnValue = false;
            return;
        } 
}

Microsoft.js.ui.EmoticonPicker.prototype.Hide = function()
{
 var c = this.uiElement;

 c.style.visibility = 'hidden';
}

Microsoft.js.ui.EmoticonPicker.prototype.SubscribeToOnChange = function(fn)
{
  this.observers.push(fn);
}

Microsoft.js.ui.EmoticonPicker.prototype.OnChange = function(image)
{
  for(var i = 0; i < this.observers.length; i++)
  {
    this.observers[i](image);
  }
}
