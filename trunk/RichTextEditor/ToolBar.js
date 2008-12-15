// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

Microsoft.js.ui.Toolbar = function(id)
{
    this.Id = id;
    this.uiElement = document.getElementById(id);
    this.buttons = new Array();
}

Microsoft.js.ui.Toolbar.prototype.AddButton = function(tbBtn)
{
  this.buttons.push(tbBtn);
}

Microsoft.js.ui.Toolbar.prototype.UpdateState = function()
{
  for(var i=0; i < this.buttons.length; i++)
  {
    var Btn = this.buttons[i];
    
    if(Btn instanceof Microsoft.js.ui.ToolbarButton)
    {
      if(Btn.getType() != Microsoft.js.ui.ToolbarButton.ButtonType.ToggleButton)
        continue;
        
      Btn.UpdateState();
    }
    else if(Btn instanceof Microsoft.js.ui.DropDownToolbarButton)
        Btn.UpdateState();
  }

}

Microsoft.js.ui.Toolbar.prototype.Disable = function()
{
  for(var i=0; i < this.buttons.length; i++)
  {
    var Btn = this.buttons[i];
    
    if(Btn.Disable)
    {  
      Btn.Disable();
    }
  }
}

Microsoft.js.ui.Toolbar.prototype.Enable = function()
{
  for(var i=0; i < this.buttons.length; i++)
  {
    var Btn = this.buttons[i];
    
    if(Btn.uiElement)
    {  
      Btn.uiElement.disabled = false;
      Btn.SetState("Default");
      Btn.UpdateState(true);
    }
  }
}


Microsoft.js.ui.ToolbarButton = function(id,type,state,width,height,queryStateCallback)
{
    Microsoft.js.ui.ToolbarButton.ButtonState = {Default:0,Highlighted:1,Selected:2,Greyed:3};
    Microsoft.js.ui.ToolbarButton.ButtonType = {ImageButton:0,ToggleButton:1};
   
    switch(state)
    {
    case "Default": this.State =  Microsoft.js.ui.ToolbarButton.ButtonState.Default;
                    break;
    case "Hightlighted": this.State =  Microsoft.js.ui.ToolbarButton.ButtonState.Highlighted;
                    break;
    case "Selected": this.State =  Microsoft.js.ui.ToolbarButton.ButtonState.Selected;
                    break;
    case "Greyed": this.State =  Microsoft.js.ui.ToolbarButton.ButtonState.Greyed;
                    break;
    }
    
    /// Private. You cannot directly access id,width,height
    this.getId = function() { return id; }
    this.getWidth = function() { return width; }
    this.getHeight = function() { return height; }
    this.getType = function() 
    { 
       var t;
       switch(type)
       {
       case "ImageButton": t = Microsoft.js.ui.ToolbarButton.ButtonType.ImageButton;
       break;
       case "ToggleButton": t = Microsoft.js.ui.ToolbarButton.ButtonType.ToggleButton;
       break;
       }
       return t;
    }
    
    this.QueryStateCallback = function() { return queryStateCallback; }
    this.uiElement = document.getElementById(id).firstChild;
    this.innerImg = this.uiElement;
    this.RelatedButtons = new Array();
    this.MouseOver = new Microsoft.js.event.EventHandler(this.uiElement, 'mouseover', 'OnMouseOver', this, false);
    this.MouseOut = new Microsoft.js.event.EventHandler(this.uiElement, 'mouseout', 'OnMouseOut', this, false);
    this.Click = new Microsoft.js.event.EventHandler(this.uiElement, 'click', 'OnClick', this, false);
 
    Microsoft.js.event.EventManager.add(this.MouseOver);
    Microsoft.js.event.EventManager.add(this.MouseOut);
    Microsoft.js.event.EventManager.add(this.Click);
}

Microsoft.js.ui.ToolbarButton.prototype.AddRelatedButton = function(btn)
{
  if(btn instanceof Microsoft.js.ui.ToolbarButton)
  {
    this.RelatedButtons.push(btn);
  }
}

Microsoft.js.ui.ToolbarButton.prototype.Disable = function()
{ 
  this.SetState("Greyed",false);
  this.uiElement.disabled = true;
}

Microsoft.js.ui.ToolbarButton.prototype.OnMouseOver = function(e)
{
  if(this.State == Microsoft.js.ui.ToolbarButton.ButtonState.Selected ||
     this.State == Microsoft.js.ui.ToolbarButton.ButtonState.Greyed)
  {
    return true;
  }
  this.SetState("Highlighted",true);
  return true;
}

Microsoft.js.ui.ToolbarButton.prototype.UpdateState = function(bForceUpdate)
{
  //Get the current state and then set the state to that.
  if(this.QueryStateCallback())
  {
    var curState = this.QueryStateCallback()(this);
    if(this.State != curState || bForceUpdate)
	    this.SetState(curState,false);
  }
}

Microsoft.js.ui.ToolbarButton.prototype.SetState = function(state,toggle)
{
 var imgObj = this.innerImg;
 var backgroundPositionLeft = imgObj.style.backgroundPosition.substring(0, imgObj.style.backgroundPosition.indexOf(" "));
    switch(state)
    {
    case "Default": 
        this.State =  Microsoft.js.ui.ToolbarButton.ButtonState.Default;
        imgObj.style.backgroundPosition = backgroundPositionLeft + " " + (this.State * -1 * this.getHeight()) + "px";
        break;
    case "Highlighted": 
        this.State =  Microsoft.js.ui.ToolbarButton.ButtonState.Highlighted;
        imgObj.style.backgroundPosition = backgroundPositionLeft + " " + (this.State * -1 * this.getHeight()) + "px";
        break;
    case "Selected": 
        if(this.getType() == Microsoft.js.ui.ToolbarButton.ButtonType.ToggleButton)
        {
            if(this.State == Microsoft.js.ui.ToolbarButton.ButtonState.Selected && toggle)
            {
              this.State = Microsoft.js.ui.ToolbarButton.ButtonState.Default;  
            }
            else 
              this.State = Microsoft.js.ui.ToolbarButton.ButtonState.Selected;
        }
        imgObj.style.backgroundPosition = backgroundPositionLeft + " " + (this.State * -1 * this.getHeight()) + "px";
        
        for(var i=0; i < this.RelatedButtons.length; i++)
        {
          this.RelatedButtons[i].SetState("Default",false);
        }
        
        break;
    case "Greyed": 
        this.State =  Microsoft.js.ui.ToolbarButton.ButtonState.Greyed;
        imgObj.style.backgroundPosition = backgroundPositionLeft + " " + (this.State * -1 * this.getHeight()) + "px";
        break;
    }
}

Microsoft.js.ui.ToolbarButton.prototype.OnMouseOut = function(e)
{
 if(this.State == Microsoft.js.ui.ToolbarButton.ButtonState.Greyed)
   return true;
   
  if(this.State != Microsoft.js.ui.ToolbarButton.ButtonState.Highlighted)
  {
    return true;
  }
  
  this.SetState("Default",true);
  
  return true;
}

Microsoft.js.ui.ToolbarButton.prototype.OnClick = function(e)
{
  if(this.State == Microsoft.js.ui.ToolbarButton.ButtonState.Greyed)
   return true;

  if(this.RelatedButtons.length >= 2)
    this.SetState("Selected",false);
  else 
    this.SetState("Selected",true);
    
  return true;
}

Microsoft.js.ui.DropDownToolbarButton = function(id,width,height,queryStateCallback)
{
  this.uiElement = document.getElementById(id);
  Microsoft.js.ui.DropDownToolbarButton.ButtonState = {Default:0,Highlighted:1,Selected:2,Greyed:3};
  this.listContainer = document.getElementById(id + "_Children");
  this.observers = new Array();
  this.Items = new Array();
  this.QueryStateCallback = function() { return queryStateCallback; }
   
  //For parent button.
  this.MouseOver = new Microsoft.js.event.EventHandler(this.uiElement, 'mouseover', 'OnMouseOver', this, false);
  Microsoft.js.event.EventManager.add(this.MouseOver);
  
  this.MouseOut = new Microsoft.js.event.EventHandler(this.uiElement, 'mouseout', 'OnMouseOut', this, false);
  Microsoft.js.event.EventManager.add(this.MouseOut);   
  
  this.Click = new Microsoft.js.event.EventHandler(this.uiElement, 'click', 'OnClick', this, false);
  Microsoft.js.event.EventManager.add(this.Click);     

  //For List container which displays the menu
  this.MenuMouseOver = new Microsoft.js.event.EventHandler(this.listContainer, 'mouseover', 'OnListMouseOver', this, false);
  Microsoft.js.event.EventManager.add(this.MenuMouseOver);
  
  this.MenuMouseOut = new Microsoft.js.event.EventHandler(this.listContainer, 'mouseout', 'OnListMouseOut', this, false);
  Microsoft.js.event.EventManager.add(this.MenuMouseOut);
}

Microsoft.js.ui.DropDownToolbarButton.prototype.ShowList = function()
{
  var p = this.uiElement;
  var c = this.listContainer;

  var top  = p.offsetHeight+1;
  var left = 0;

  for (; p ; p = p.offsetParent)
  {
    top  += p.offsetTop;
    left += p.offsetLeft;
    
    if(p.id == "rteditor-tb")
    break;
  }
  
  this.uiElement.style.backgroundPosition = "0px 48px";
  c.style.position   = "absolute";
  c.style.top        = top +'px';
  c.style.left       = left+'px';
  c.style.visibility = "visible";
}

Microsoft.js.ui.DropDownToolbarButton.prototype.HideList = function()
{
  var c = this.listContainer;
  c.style.visibility = 'hidden';
}

Microsoft.js.ui.DropDownToolbarButton.prototype.OnHide = function()
{
    this.uiElement.style.backgroundPosition = "0px 96px";
}

Microsoft.js.ui.DropDownToolbarButton.prototype.OnMouseOver = function(e)
{
  var imgObj = this.uiElement;
  if(imgObj != 'undefined')
  {
    imgObj.style.backgroundPosition = "0px 72px";
  }
  return true;
}

Microsoft.js.ui.DropDownToolbarButton.prototype.OnMouseOut = function(e)
{
  var imgObj = this.uiElement;
  if(imgObj != 'undefined' && this.listContainer.style.visibility != 'visible')
  {
    imgObj.style.backgroundPosition = "0px 96px";
  }
  return true;
}

Microsoft.js.ui.DropDownToolbarButton.prototype.OnClick= function(e)
{
  this.ShowList();
  return true;
}

Microsoft.js.ui.DropDownToolbarButton.prototype.OnListMouseOver = function(e)
{
  if(this.listContainer.style.visibility == 'hidden')
    this.ShowList();
  return true;
}

Microsoft.js.ui.DropDownToolbarButton.prototype.OnListMouseOut = function(e)
{
 if(e && e.relatedTarget)
  this.HideOnFireFox(e);
 else
 {
   this.HideList();
   return true;
 }
}

Microsoft.js.ui.DropDownToolbarButton.prototype.HideOnFireFox = function(e)
{
   if(e.currentTarget.className == "tbDropDownListContainer" &&
   (e.relatedTarget.className != "tbClassicDropDownListItem" && e.relatedTarget.className != "tbDropDownListItem") &&
    e.currentTarget.className != e.relatedTarget.className)
    {
      this.HideList();
      return true;
    }
}

Microsoft.js.ui.DropDownToolbarButton.prototype.AddItem = function(item)
{
 this.Items.push(item);
}

Microsoft.js.ui.DropDownToolbarButton.prototype.OnSelect = function(e)
{
 //For sake of browser portability
 var src = e.srcElement || e.target;
 this.uiElement.value = src.innerHTML;
 this.HideList();
 this.OnHide();
 this.OnChange(this.uiElement.value);
 return true;
}

Microsoft.js.ui.DropDownToolbarButton.prototype.SubscribeToOnChange = function(fn)
{
  this.observers.push(fn);
}

Microsoft.js.ui.DropDownToolbarButton.prototype.OnChange = function(fontName)
{
  for(var i = 0; i < this.observers.length; i++)
  {
    this.observers[i](fontName);
  }
}

Microsoft.js.ui.DropDownToolbarButton.prototype.UpdateState = function(bForceUpdate)
{
  //Get the current state and then set the state to that.
  if(this.QueryStateCallback())
  {
    var curState = this.QueryStateCallback()(this);
    if(curState)
    {
       this.uiElement.value = curState;
    }
  }
}

Microsoft.js.ui.DropDownToolbarButton.prototype.Disable = function()
{ 
this.SetState("Greyed");
this.uiElement.disabled = true;
}

Microsoft.js.ui.DropDownToolbarButton.prototype.SetState = function(state)
{

    switch(state)
    {
    case "Default": 
        this.State =  Microsoft.js.ui.DropDownToolbarButton.ButtonState.Default;
        this.uiElement.style.backgroundPosition = "0px 96px";        
        break;
    case "Highlighted": 
        this.State =  Microsoft.js.ui.DropDownToolbarButton.ButtonState.Highlighted;
        this.uiElement.style.backgroundPosition = "0px 72px";
        break;
    case "Selected": 
        this.State = Microsoft.js.ui.DropDownToolbarButton.ButtonState.Selected;
        this.uiElement.style.backgroundPosition = "0px 48px";
        break;
    case "Greyed": 
        this.State =  Microsoft.js.ui.DropDownToolbarButton.ButtonState.Greyed;
        this.uiElement.style.backgroundPosition = "0px 24px";
        break;
    }
}

