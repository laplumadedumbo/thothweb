// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


Microsoft.js.ui.ColorPicker = function()
{  
	var colorArray = new Array("#000000","#993300","#333300","#003300","#003366","#000080","#333399",
		"#333333","#800000","#FF6600","#808000","#008000","#008080","#0000FF","#666699","#808080","#FF0000",
		"#FF9900","#99CC00","#339966","#33CCCC","#3366FF","#800080","#999999","#FF00FF","#FFCC00","#FFFF00",
		"#00FF00","#00FFFF","#00CCFF","#993366","#C0C0C0","#FF99CC","#FFCC99","#FFFF99","#CCFFCC","#CCFFFF",
		"#99CCFF","#CC99FF","#FFFFFF");


	this.uiElement = document.createElement("DIV");
    this.uiElement.className = "ColorPicker";
   
    this.observers = new Array();
    this.colorType = null;
    this.ccells = new Array();
    
	for (var i=0;i < colorArray.length; i++)
	{
		var row;
	
		if (i%8==0)
		{
			row = document.createElement("div");
			row.className = "ColorRow";
			this.uiElement.appendChild(row);
		}
	    
	    var ccell = new Microsoft.js.ui.ColorCell(colorArray[i],this);
	    this.ccells.push(ccell);
	        
		row.appendChild(ccell.uiElement);
	}
	
	this.MouseOut = new Microsoft.js.event.EventHandler(this.uiElement, 'mouseout', 'Hide',this, false);
	Microsoft.js.event.EventManager.add(this.MouseOut);
}


Microsoft.js.ui.ColorCell = function(bgColor,parentObj)
{
		this.uiElement = document.createElement("a");
		this.uiElement.className = "ColorCell";
		this.uiElement.innerHTML = "&nbsp;";
		this.uiElement.tabIndex=0;
		
		if (browser.isMicrosoft)
		this.uiElement.unselectable = "on"; 	    
		
        this.KeyPress = new Microsoft.js.event.EventHandler(this.uiElement, 'keypress', 'OnKeyPress', parentObj, false);
        this.Click = new Microsoft.js.event.EventHandler(this.uiElement, 'click', 'OnSelect', parentObj, false);
 
        Microsoft.js.event.EventManager.add(this.KeyPress);
        Microsoft.js.event.EventManager.add(this.Click);
	
		this.uiElement.style.backgroundColor = bgColor;
}


Microsoft.js.ui.ColorPicker.prototype.OnKeyPress = function(e)
{
  if (e.keyCode==13)
    this.OnSelect(e);

  event.cancelDefault = true;
  return false;
  
}

Microsoft.js.ui.ColorPicker.prototype.OnSelect = function(e)
{
   var src = e.srcElement || e.target;
   var bgcolor = src.style.backgroundColor;
   
   this.OnChange(bgcolor);
      
   return true;
}

Microsoft.js.ui.ColorPicker.prototype.Show = function(parentId,type)
{
  this.parentElement = document.getElementById(parentId);
  this.colorType = type;
  
  var p = this.parentElement;
  var c = this.uiElement;

  var top  = p.offsetHeight+1;
  var left = 0;

  for (; p; p = p.offsetParent)
  {
    top  += p.offsetTop;
    left += p.offsetLeft;
  }

  left -= 130;
  
  c.style.position   = "absolute";
  c.style.top        = top +'px';
  c.style.left       = left+'px';
  c.style.visibility = "visible";
}

Microsoft.js.ui.ColorPicker.prototype.Hide = function(e)
{ 
  var src = null;
  var to = null;
  
  if(e != null)
  {
    src = e.srcElement || e.target;
    to =  e.toElement || e.relatedTarget;
  }
  
  if(e != null && src != null && to != null)
  {
     if (   (src.className == "ColorCell" && to.className == "ColorPicker")
          || (to.className == "ColorCell" && src.className == "ColorPicker") 
          || (to.className == "ColorCell" && src.className == "ColorCell")
         )
         {
            e.cancelBubble = true;
            e.returnValue = false;
            return false;
         }
  }
  else if (!to && e != null) 
  {
    return false;
  }

  var c = this.uiElement;
  c.style.visibility = 'hidden';
  this.colorType = null;
}


Microsoft.js.ui.ColorPicker.prototype.SubscribeToOnChange = function(fn)
{
  this.observers.push(fn);
}

Microsoft.js.ui.ColorPicker.prototype.OnChange = function(bgColor)
{
  for(var i = 0; i < this.observers.length; i++)
  {
    this.observers[i](bgColor,this.colorType);
  }
}