// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

Microsoft.js.ui.HyperLinkDialog = function(v) 
{
this.popup;
this.elLink;
this.elNoFollow;
this.elButtonOk;
this.elButtonCancel;
this.v = v;

//External observers who subscribe for on SetHyperLink command.
this.observers = new Array();

this.popup = document.createElement("div");
this.popup.className = "RTE_INSERTLINK"

this.elTitle = document.createElement("div");
this.elTitle.innerText = "Insert Hyperlink";
this.elTitle.className = "RTE_INSERTLINK_TITLE";

this.elLink = document.createElement("input");
this.elLink.className = "Selectable RTE_INSERTLINK_EDITBOX";	

var br;
var l;

if (browser.isMicrosoft && this.v && this.v.NoFollow)
{
	l = document.createElement("label");				

	this.elNoFollow = document.createElement("input");
	this.elNoFollow.id = this.elNoFollow.name = "nofollow"

	this.l.innerText = sz.pu_link_nf;
	this.l.htmlFor = "nofollow";
	this.elNoFollow.type = "checkbox";
}

this.popup.appendChild(this.elTitle);
this.popup.appendChild(this.elLink);

if (browser.isMicrosoft && this.v && this.v.NoFollow)
{
    br = document.createElement("br");
	this.popup.appendChild(br);
	this.popup.appendChild(this.elNoFollow);
	this.popup.appendChild(l);
}

var elButtonGroup = document.createElement("div");
elButtonGroup.className = "RTE_INSERTLINK_BUTTONS";

this.elButtonOk = document.createElement("input");
this.elButtonCancel = document.createElement("input");
this.elButtonOk.type = "button";

this.elButtonCancel.type = "reset";
this.elButtonOk.className = this.elButtonCancel.className = "RTE_INSERTLINK_BUTTON";
this.elButtonOk.value = "OK";
this.elButtonCancel.value = "Cancel";

elButtonGroup.appendChild(this.elButtonOk);
elButtonGroup.appendChild(this.elButtonCancel);

this.popup.appendChild(elButtonGroup);
this.popup.Initial = this.elLink;

if (this.v)
{
	this.elLink.innerText = this.v.link;
	if (browser.isMicrosoft && this.elNoFollow) this.elNoFollow.checked = this.v.nofollow;
}
else
{
	this.elLink.value = "http://"; 
}				

 this.PopupKeypress = new Microsoft.js.event.EventHandler(this.popup, 'keypress', 'OnPopupKeypress', this, false);
 Microsoft.js.event.EventManager.add(this.PopupKeypress);

 this.PopupClick = new Microsoft.js.event.EventHandler(this.popup, 'keypress', 'OnCancelEvent', this, false);
 Microsoft.js.event.EventManager.add(this.PopupClick);

 this.LinkSelStart = new Microsoft.js.event.EventHandler(this.elLink, 'selectstart', 'OnCancelEvent', this, false);
 Microsoft.js.event.EventManager.add(this.LinkSelStart);

 this.LinkSel = new Microsoft.js.event.EventHandler(this.elLink, 'select', 'OnCancelEvent', this, false);
 Microsoft.js.event.EventManager.add(this.LinkSel);

 this.LinkKeypress = new Microsoft.js.event.EventHandler(this.elLink, 'keypress', 'OnLinkKeypress', this, false);
 Microsoft.js.event.EventManager.add(this.LinkKeypress);

 this.OkButtonClick = new Microsoft.js.event.EventHandler(this.elButtonOk, 'click', 'OnOkButtonclick', this, false);
 Microsoft.js.event.EventManager.add(this.OkButtonClick);
 
 this.CancelButtonClick = new Microsoft.js.event.EventHandler(this.elButtonCancel, 'click', 'OnCancelButtonclick', this, false);
 Microsoft.js.event.EventManager.add(this.CancelButtonClick);
 
 this.uiElement = this.popup; 
 this.Hide();
}


Microsoft.js.ui.HyperLinkDialog.prototype.Destroy = function()
{
    if (this.elLink)
    {
	    Microsoft.js.event.EventManager.remove(this.LinkKeypress);
	    Microsoft.js.event.EventManager.remove(this.LinkSelStart);
	    Microsoft.js.event.EventManager.remove(this.LinkSel);
	    Microsoft.js.event.EventManager.remove(this.PopupClick);
	    Microsoft.js.event.EventManager.remove(this.PopupKeypress);
	    Microsoft.js.event.EventManager.remove(this.OkButtonClick);
	    Microsoft.js.event.EventManager.remove(this.CancelButtonClick);
    }
    	
    this.elLink = this.popup = this.elButtonOk = this.elButtonCancel = this.elNoFollow = null;
}

Microsoft.js.ui.HyperLinkDialog.prototype.OnPopupKeypress = function(e)
{
	e.cancelBubble = (e.keyCode!=27 || e.keyCode==13);
}

//elLink.onselectstart = elLink.onselect = elLink.onkeypress = popup.onclick = function()
Microsoft.js.ui.HyperLinkDialog.prototype.OnCancelEvent = function(e) 
{
	e.cancelBubble = true;
}

Microsoft.js.ui.HyperLinkDialog.prototype.OnLinkKeypress = function(e)
{
	if (e && e.keyCode && e.keyCode==13)
	{
		this.elButtonOk.click();
		e.cancelBubble=true;
		e.returnValue = false;
	}
}

String.prototype.startsWith = function(s) { return this.indexOf(s)==0; }

Microsoft.js.ui.HyperLinkDialog.prototype.OnOkButtonclick = function(e)
{
if (this.v)
{
 this.v.link = this.elLink.value;
}
else
{
 this.v = new Object;
 this.v.link = this.elLink.value;
}   

if (this.v.link!="")
{
    if (browser.isMicrosoft && this.elNoFollow)
	    this.v.nofollow = this.elNoFollow.checked;
}
this.Hide();
this.OnSetHyperLink();
return false;
}

Microsoft.js.ui.HyperLinkDialog.prototype.OnCancelButtonclick = function(e)
{
  this.popup.style.visibility = 'hidden';
  
  //. If Firefox
  if (e.stopPropagation) 
  {
		e.stopPropagation();
		e.preventDefault();
  }
  else 
    e.returnValue = false;
}

Microsoft.js.ui.HyperLinkDialog.prototype.Hide = function()
{
  this.popup.style.visibility = 'hidden';
}

Microsoft.js.ui.HyperLinkDialog.prototype.Show = function(parentId,v)
{
    this.v = v;
    this.elLink.value = "http://";
   
    
    if (this.v)
    {
        this.elLink.value = this.v.link;
        if (browser.isMicrosoft && this.elNoFollow) 
            this.elNoFollow.checked = this.v.nofollow;
    }
  
    var parentElement = document.getElementById(parentId);
    
    var p = parentElement;
    var c = this.uiElement;

    var top  = p.offsetHeight+1;
    var left = 0;

    for (; p; p = p.offsetParent)
    {
    top  += p.offsetTop;
    left += p.offsetLeft;
    }

    left -= 100;

    c.style.position   = "absolute";
    c.style.top        = top +'px';
    c.style.left       = left+'px';
    c.style.visibility = "visible";
    
    this.uiElement.focus()
    this.elLink.focus();  
    this.elLink.select();
}

Microsoft.js.ui.HyperLinkDialog.prototype.OnSetHyperLink = function()
{
  for(var i = 0; i < this.observers.length; i++)
  {
    this.observers[i](this.v);
  }
}

Microsoft.js.ui.HyperLinkDialog.prototype.SubscribeToOnChange= function(fn)
{
  this.observers.push(fn);
}