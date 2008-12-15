// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

function InitRte()
{
selCurrent = null;	
csLock = null;
blnRefresh = false;
blnLock = false;
bookmark = null;
caretpos = null;
pos = 0;
blnMarkedCodeBlock = false;

browser = { 
version: parseInt(navigator.appVersion),
isNetscape: navigator.appName.indexOf("Netscape") != -1,
isMicrosoft: navigator.appName.indexOf("Microsoft") != -1 };

// Get the initial contents for the RichTextEditor from what is there in the textarea
contents =  top.document.getElementById(editorId).value;

top.document.getElementById(editorId).style.display = "none";

// Build the document contents for the RichTextEditor
links = "<link rel=\"stylesheet\" type=\"text/css\" href='" + editCssUrl + "'>";
if(!contents)
    htmltxt = '<html><head>' + links + '</head><body>' + "<div style=\"font-family:arial;font-size:10pt;\"></div>" + '</body></html>';
else
    htmltxt = '<html><head>' + links + '</head><body>' + contents + '</body></html>';

//Testing the Object version - Still a work in progress
richeditor = new RichTextEditor(RichEditId);
if(RteMsg2)
    richeditor.SelectCBMsg = RteMsg2;
if(RteMsgCodeBlock)
    richeditor.CodeBlockStr = RteMsgCodeBlock;
    
richeditor.SetContent(htmltxt);
richeditor.EditMode = true;

var rteElem = document.getElementById(RteId);
var p = document.body.lastChild;

//Color
ColorDialog = new Microsoft.js.ui.ColorPicker();
ColorDialog.Hide();

p.appendChild(ColorDialog.uiElement);

ColorDialog.SubscribeToOnChange(OnColorChange);

//Emoticon
EmoticonDialog = new Microsoft.js.ui.EmoticonPicker();
EmoticonDialog.Hide();

p.appendChild(EmoticonDialog.uiElement);

EmoticonDialog.SubscribeToOnChange(OnEmoticonChange);

HyperLinkDialog = new Microsoft.js.ui.HyperLinkDialog();
p.appendChild(HyperLinkDialog.uiElement);
HyperLinkDialog.SubscribeToOnChange(SetHyperLink);

toolb = this[RteToolbarId];

// This was causing the control to steal focus on page load when it was loaded
// with an empty Text property.
// TODO: The benefit of not having this control steal focus is greater than
// setting the initial state of the FontSize and Justify buttons. Still it
// would be good have the initial button states be correct. - yorkrj
/*if(!contents)
{
  OnFontSizeChange(lastFontSize);
  SetDefaultJustify();
}*/

// Add relationship to buttons.
RteJustifyLeft.AddRelatedButton(RteJustifyCenter);
RteJustifyLeft.AddRelatedButton(RteJustifyRight);
RteJustifyCenter.AddRelatedButton(RteJustifyLeft);
RteJustifyCenter.AddRelatedButton(RteJustifyRight);
RteJustifyRight.AddRelatedButton(RteJustifyLeft);
RteJustifyRight.AddRelatedButton(RteJustifyCenter);

RteUnorderedList.AddRelatedButton(RteOrderedList);
RteOrderedList.AddRelatedButton(RteUnorderedList);


if(browser.isMicrosoft)
{
 this.MouseUp = new Microsoft.js.event.EventHandler(richeditor.GetDocument(), 'mouseup', 'OnMouseUp', this, false);  
 this.KeyUp = new Microsoft.js.event.EventHandler(richeditor.GetDocument(), 'keyup', 'OnKeyUp', this, false);

 Microsoft.js.event.EventManager.add(this.MouseUp);
 Microsoft.js.event.EventManager.add(this.KeyUp);

 this.KeyDown = new Microsoft.js.event.EventHandler(richeditor.GetDocument(), 'keydown', 'StopBack', this, false);  
 Microsoft.js.event.EventManager.add(this.KeyDown);
 
 RteHyperLink.SetState("Greyed");
}
else
{
 this.MouseUp = new Microsoft.js.event.EventHandler(richeditor.GetDocument(), 'mouseup', 'UpdateToolBar', this, false);  
 this.KeyUp = new Microsoft.js.event.EventHandler(richeditor.GetDocument(), 'keyup', 'UpdateToolBar', this, false);

 this.KeyDown = new Microsoft.js.event.EventHandler(richeditor.GetDocument(), 'keydown', 'OnFireFoxRteMenu', this, false);  
 Microsoft.js.event.EventManager.add(this.KeyDown);

 Microsoft.js.event.EventManager.add(this.MouseUp);
 Microsoft.js.event.EventManager.add(this.KeyUp);
}

if(RteFgColor.uiElement)
{
    RteFgColor.uiElement.style.backgroundColor = "#000000";
}
toolb.UpdateState();
}

function OnMouseUp(e) 
{
 CacheSelection();
 UpdateToolBar(e);
}

function OnKeyUp(e) 
{
 CacheSelection();
 UpdateToolBar(e);
}

function CacheSelection()
{
	try
	{
	    if(richeditor.EditMode == false)
	    return;
	       
		selCurrent = richeditor.GetDocument().selection.createRange()
		selCurrent.type =  richeditor.GetDocument().selection.type;
	    
		// Refresh toolbar when selection goes from complete to empty
		var blnCurrent = selCurrent.text!="";
		
		if(blnCurrent)
		  RteHyperLink.SetState("Default");
		else
		  RteHyperLink.SetState("Greyed");
		

		if (blnRefresh && !blnCurrent)
		    UpdateToolbar(richeditor.GetWindow().event);
		
		blnRefresh = blnCurrent;
	
		if (selCurrent.type=="Text" && selCurrent.text=="" && !csLock)	
		{
			csLock = setTimeout(CacheSelection,0);
		}
		else
		{
			clearTimeout(csLock);
			csLock = null;
		}
	}
	catch (ex)
	{}
}	
	
function SetSelection()
{
	if (browser.isMicrosoft && richeditor.EditMode == true)
	{
		try
		{
			richeditor.Focus();
			selCurrent = this.GetSelection();
			selCurrent.select()
			RteHyperLink.SetState("Default");
		}
		catch (ex)
		{
		}
	}
	richeditor.Focus();			
	return selCurrent;
}

function GetSelection()
{
	if (!selCurrent)
	{
		CacheSelection();
	}
	return selCurrent;
}

function UpdateToolBar(e)
{
    if (!blnLock && richeditor.EditMode == true)
    {
         blnLock=true;
         // Get the toolbar javascript object.
         var toolb = this[RteToolbarId];
         var key_code;
         
         if (e.keyCode) 
          key_code = e.keyCode;
         else if (e.which) 
          key_code = e.which;

         // If key was pressed or key was up.
         // In addition the key was a space or more than 49 
         var skipUpdate = e && (e.type=="keydown" || e.type=="keyup") && ( key_code == 32 || key_code > 46);

         if(!skipUpdate)
         {
           toolb.UpdateState();
           UpdateColorButtons();
         }
         richeditor.Focus();
         blnLock = false;
         
     }
}

function UpdateColorButtons()
{
 var color = QueryCommandValue("ForeColor")();
 var colorStr;
 
 if(color != null)
 {
     if(browser.isMicrosoft)
     {
         //This color conversion code is suspect/broken. -- yorkrj
         if(color == 0)
            colorStr = "#000000";
         else if(color.toString(16).length == 4)
            colorStr = "00" + color.toString(16);
         else if(color.toString(16).length == 2 && color == 255)
            colorStr = "#ffffff";
         else 
            colorStr = color.toString(16);
          
          if(colorStr.length == 6)
          {
            colorStr = colorStr.substring(4) + colorStr.substring(2,4) + colorStr.substring(0,2);
          }
     }
     else if (color != "" )
     {
        colorStr = color;
     }
     else 
     {
        colorStr = "rgb(0,0,0)";
     }

     if(RteFgColor.uiElement)
     {
        RteFgColor.uiElement.style.backgroundColor = colorStr;
     }
 }
}

function StopBack(e)
{
    if(richeditor.EditMode == false)
    return;
    
	var sel = this.GetSelection();
	if (e && sel.type=="Control" && e.keyCode==8)
	{
		sel.item(0).removeNode();
		return false;
	}
	else if(e && e.keyCode == 13)
	{
	    setTimeout(SetDefaultJustify,100);
	    return false;
	}
	else if(e && e.keyCode == 9)
	    HandleTabKeys(e);
	else 
	{
	    e.returnValue = true;
	    e.cancelBubble = false;
	    return true;
    }
}

function OnFireFoxRteMenu(e)
{
    if(richeditor.EditMode == false)
    return;
    
    if(e && e.ctrlKey == true)
    {
        /// Ctrl-b is pressed
        if(e.keyCode == 66)
	    {
	        e.preventDefault();
	        OnBold();
	        return false;
	    }
	    /// Ctrl-i is pressed
	    else if(e.keyCode == 73)
	    {
	        e.preventDefault();
	        OnItalics();
	        return false;
	    
	    }
	    /// Ctrl-u is pressed.
	    else if(e.keyCode == 85)
	    {
	       e.preventDefault();
	        OnUnderline();
	        return false;	    
	    }
    }
    else if(e && e.keyCode == 9)
    {
        HandleTabKeys(e);
    }
}

function HandleTabKeys(e)
{
    //Tabe key has been pressed
    if(e.keyCode == 9)
    {   
        if(e.shiftKey == true)
        {
            OnOutdent();
        }
        else
        {
            OnIndent();
        }    
    
        if(e.preventDefault)
            e.preventDefault();
        else
        {
            e.returnValue = false;
        }
        
        richeditor.Focus();
    }
    return false;
}

function SetDefaultJustify()
{	  
var jl =  richeditor.QueryRichEditCommandValue("JustifyLeft");
var jc =  richeditor.QueryRichEditCommandValue("JustifyCenter");
var jr =  richeditor.QueryRichEditCommandValue("JustifyRight");

if(!jl && !jc && !jr)
{
    richeditor.ExecRichEditCommand("JustifyLeft");
    RteJustifyLeft.UpdateState();
}

}

function DoKeyCheck()
{
    if(richeditor.EditMode == false)
    return;
    
	CacheSelection();		
}
	
function QueryCommandState(cmdString)
{
        function GetState(toolBarToggleBtn)
		{
		  var btnState = null;
		  if (toolBarToggleBtn) 
		  {
		    btnState = richeditor.QueryRichEditCommandState(cmdString) ? "Selected" : "Default" ; 
	      	    
		  }
		  
		  return btnState;
		}
		
  return GetState;
}

function QueryCommandValue(cmdString)
{
    function GetValue(btn)
    {
      var val = richeditor.QueryRichEditCommandValue(cmdString);
      if(cmdString == "fontsize")
      {
        var retVal;
        for(var fz in fontsizes)
        {
          if(fontsizes[fz] == val)
           retVal = fz;
        }
        if(retVal)
        lastFontSize = retVal;
        return retVal;
      }
      else if(cmdString == "fontname" && val)
      {
        if(browser.isMicrosoft)
        {
          val = fontDescNameMap[val];
        }
        
        lastFontName = val;
        return val;
      }
      else
        return val;
    }
  
  return GetValue;
}

function ShowMessage(msg)
{
  alert(msg);
}

function HasClass(element,className)
{
    var re = new RegExp('(?:^|\\s+)' + className + '(?:\\s+|$)');
    return re.test(element['className']);
}

function OnClosePreviewDlg(e)
{
   if(richeditor.EditMode == true)
   PreviewDlg.Hide();
}

function OnBgColor(Id)
{
if(richeditor.EditMode == true)
ColorDialog.Show(RteBgColor.getId(),'BackColor');
}

function OnFgColor(Id)
{
if(richeditor.EditMode == true)
ColorDialog.Show(RteBgColor.getId(),'ForeColor');
}

function OnColorChange(colorValue,type)
{

if(type == 'BackColor')
{
  if(browser.isMicrosoft == true)
    richeditor.ExecRichEditCommand('BackColor',colorValue);
  else 
    richeditor.ExecRichEditCommand('hilitecolor',colorValue);
  ColorDialog.Hide();
}
else
{
  richeditor.ExecRichEditCommand('ForeColor',colorValue);
  ColorDialog.Hide();
  if(RteFgColor.uiElement)
  {
    RteFgColor.uiElement.style.backgroundColor = colorValue;
  }
}
}

function OnEmoticon(Id)
{
 if(richeditor.EditMode == false)
 return;

 EmoticonDialog.Show(RteEmoticon.getId());
}

function OnEmoticonChange(imagesrc)
{
  richeditor.ExecRichEditCommand('InsertImage',imagesrc);
  EmoticonDialog.Hide();
}


function OnHtmlView()
{
 if(richeditor.EditMode == true)
    richeditor.EditMode = false;
 else
    return;
    
 ViewasHtml();
} 

function ViewasHtml()
{
    
 getCursorPos();
 richeditor.EditMode = false;
 
 toolb.Disable();
 editor = top.document.getElementById(editorId);
 editor.value = richeditor.toHtmlString();
 
 if(richeditor.GetDocument().body.innerText)
  richeditor.GetDocument().body.innerText = richeditor.GetDocument().body.innerHTML;
 else
  richeditor.GetDocument().body.textContent = richeditor.GetDocument().body.innerHTML;
  
}


function OnPreviewView()
{
    editor = top.document.getElementById(editorId);
    editor.value = richeditor.toHtmlString();
    sendRequest("forumservices/postservice.asmx/GetPostPreview",'postMessage=' + editor.value)
}
  
function CopyText()
{
 editor = top.document.getElementById(editorId);
 editor.value = richeditor.toHtmlString();
}

function OnTextView(e)
{
    if(richeditor.EditMode == false)
        richeditor.EditMode = true;
    else
        return;

   ViewasText();        
} 

function ViewasText()
{
    if(richeditor.GetDocument().body.innerText)
    {
        var hstr = richeditor.GetDocument().body.innerText;
        if(hstr)
        {
          hstr = hstr.replace(/<P align=left>&nbsp;<\/P>$/,"<P align=left><\/P>");
          richeditor.GetDocument().body.innerHTML =  hstr;
        }
        else
            richeditor.GetDocument().body.innerHTML =  "";
    }
    else 
    {
        var hstr =  richeditor.GetDocument().body.textContent;
        if(hstr)
        {
            hstr = hstr.replace(/<P align=left>&nbsp;<\/P>$/,"<P align=left><\/P>");
            richeditor.GetDocument().body.innerHTML =  hstr;
        }
        else
            richeditor.GetDocument().body.innerHTML = "";
    }
    editor = top.document.getElementById(editorId);
    var htmlString = richeditor.toHtmlString();
    if(htmlString)
     editor.value = htmlString;
    else
     editor.value = "";

     if(pos > -1)
        setCaret(pos);     
     toolb.Enable();
     toolb.UpdateState();
}


function   getCursorPos()
{   
  pos=-1;   
  var   sText= this.GetSelection();
  var   tempText=null;
  
  if(richeditor.GetDocument().selection) // IE
  {
    tempText = richeditor.GetDocument().body.createTextRange();   
    
    if(sText != null)
    {
      sText.select();
      if(sText.htmlText != "")   
        tempText.setEndPoint("EndToStart",sText);   
    }
    
    pos=tempText.text.length;    
  }
  else if(richeditor.GetDocument().body.selectionEnd)
  {
    sText.select();   
    pos = richeditor.GetDocument().body.selectionEnd;
  } 
}  

function setCaret(pos) 
{ 
 try 
 { 
  if(richeditor.GetDocument().selection)
  {
   var r =richeditor.GetDocument().body.createTextRange(); 
   r.moveStart('character',pos); 
   r.collapse(true); 
   r.select(); 
  }
  else if(richeditor.GetDocument().body.selectionEnd)
  {
    richeditor.GetDocument().body.selectionEnd = pos;
  }
 } 
 catch(e) 
 {} 
}  


function OnBold()
{
richeditor.ExecRichEditCommand('bold');
}

function OnCut()
{
    if(browser.isMicrosoft)
        richeditor.ExecRichEditCommand('cut');
    else if(RteMsg1)
    {
      ShowMessage(RteMsg1);
    }
}

function OnPaste()
{
    if(browser.isMicrosoft)
        richeditor.ExecRichEditCommand('paste');
    else if(RteMsg1)
    {
      ShowMessage(RteMsg1);
    }
}

function OnCopy()
{
    if(browser.isMicrosoft)
        richeditor.ExecRichEditCommand('copy');
    else if(RteMsg1)
    {
      ShowMessage(RteMsg1);
    }
}

function OnItalics()
{
richeditor.ExecRichEditCommand('italic');
}

function OnUnderline()
{
richeditor.ExecRichEditCommand('underline');
}

function OnIndent()
{
 richeditor.ExecRichEditCommand('indent');
}

function OnOutdent()
{
richeditor.ExecRichEditCommand('outdent');
}

function OnFontChange(fontName)
{
  richeditor.ExecRichEditCommand('fontname', fontName);
  richeditor.ExecRichEditCommand('fontsize',fontsizes[lastFontSize]);
 
  lastFontName = fontName;
}

function OnFontSizeChange(fontSize)
{
 var fname = richeditor.QueryRichEditCommandValue('fontname');
 if(fname == "")
 richeditor.ExecRichEditCommand('fontname', lastFontName);
 richeditor.ExecRichEditCommand('fontsize',fontsizes[fontSize]);
 lastFontSize = fontSize;
}


function OnLeftJustify(btn)
{
   richeditor.ExecRichEditCommand('JustifyLeft');
   
}

function OnCenterJustify(btn)
{
    richeditor.ExecRichEditCommand('JustifyCenter');
}


function OnRightJustify(btn)
{
    richeditor.ExecRichEditCommand('JustifyRight');
}


//List & Create Link.
function OnOrderedList()
{
richeditor.ExecRichEditCommand('InsertOrderedList');
}

function OnUnOrderedList()
{
richeditor.ExecRichEditCommand('InsertUnorderedList');
}

function OnCreateLink(Id)
{
v = null;
    
    if(richeditor.EditMode == false)
    return;
 
    if(RteHyperLink.State == Microsoft.js.ui.ToolbarButton.ButtonState.Greyed)
    return;
    
    var elNode = null;
    
    if (browser.isMicrosoft)
    {
        sel = this.GetSelection();    
        
        if (sel.type=="Control")
            elNode = sel.item(0);
        else
            elNode = sel.parentElement();
    	    
        while (elNode!=null && elNode.tagName!="A")
            elNode = elNode.parentElement;
    }
    else 
    {
       var selectedRange = richeditor.GetWindow().getSelection();		
       elNode = selectedRange.focusNode;
    
       while (elNode!=null && elNode.tagName!="A")
            elNode = elNode.parentNode;
    }
         
    if (elNode)
    {
        v =  new Object();
        v.link = elNode.href;
        v.nofollow = (elNode.rel == "nofollow");
    }   

    HyperLinkDialog.Show(RteHyperLink.getId(),v);
}


function OnMarkCode()
{
    OnLeftJustify();
    getCursorPos();
    richeditor.MarkCode();
    editor = top.document.getElementById(editorId);
    editor.value = richeditor.toHtmlString();
    richeditor.Focus();
    setCaret(pos);
    
    
}

// send http request
function sendRequest(doc,params)
{
    // check for existing requests
    if(xmlobj!=null&&xmlobj.readyState!=0&&xmlobj.readyState!=4)
    {
        xmlobj.abort();
    }
    try
    {
        // instantiate object for Mozilla, Nestcape, etc.
        xmlobj=new XMLHttpRequest();
    }
    catch(e)
    {
        try{
            // instantiate object for Internet Explorer
            xmlobj=new ActiveXObject('Microsoft.XMLHTTP');
        }
        catch(e)
        {
            // Ajax is not supported by the browser
            xmlobj=null;
            return false;
        }
    }
    
    // assign state handler
    xmlobj.onreadystatechange=stateChecker;
    
    // open socket connection
    xmlobj.open('POST',doc,true);
    
    //Set the content type for POST
    xmlobj.setRequestHeader('Content-type','application/x-www-form-urlencoded;charset=UTF-8;');
    
    // send request wotj parameters
    xmlobj.send(params);
}

// check request status
function stateChecker()
{
    // if request is completed
    if(xmlobj.readyState==4)
    {
        // if status == 200 display text file
        if(xmlobj.status==200)
        {
            // read XML data
            data=xmlobj.responseXML.getElementsByTagName('string');
               // display XML data
               
            // create data container
            DisplayPreview();
           
        }
        else
        {
            alert('Failed to get response :'+ xmlobj.statusText);
        }
    }
}

String.prototype.trim = function() { return this.replace(/^\s+|\s+$/, ''); };

function DisplayPreview()
{
  PreviewDlg.SetContent(data[0].firstChild.nodeValue.trim());
  
  PreviewDlg.Show();
}


function mouseCoords(ev){
	if(ev.pageX || ev.pageY){
		return {x:ev.pageX, y:ev.pageY};
	}
	return {
		x:ev.clientX + document.body.scrollLeft - document.body.clientLeft,
		y:ev.clientY + document.body.scrollTop  - document.body.clientTop
	};
}


function getMouseOffset(target, ev){
	ev = ev || window.event;

	var docPos    = getPosition(target);
	var mousePos  = mouseCoords(ev);
	
	return {x:mousePos.x - docPos.x, y:mousePos.y - docPos.y};
}

function getPosition(e){
	var left = 0;
	var top  = 0;

	while (e.offsetParent){
		left += e.offsetLeft;
		top  += e.offsetTop;
		e     = e.offsetParent;
	}

	left += e.offsetLeft;
	top  += e.offsetTop;

	return {x:left, y:top};
}

function mouseMove(ev){
	ev           = ev || window.event;
	var mousePos = mouseCoords(ev);

	if(dragObject){
		dragObject.style.position = 'absolute';
		dragObject.style.top      = mousePos.y - mouseOffset.y;
		dragObject.style.left     = mousePos.x - mouseOffset.x;

		if(dragObject.className == "TitleBar")
			dragObject.style.filter = "alpha(opacity=50)";
		else if(dragObject.className == "Window")
                {
			dragObject.style.filter = "alpha(opacity=50)";
                }
		return false;
	}
}

function mouseUp()
{

		if(dragObject != null && dragObject.className == "TitleBar")
			dragObject.style.filter = "alpha(opacity=100)";
		else if(dragObject != null && dragObject.className == "Window")
                {
			dragObject.style.filter = "alpha(opacity=100)";
                }
                
	dragObject = null;
}

function makeDraggable(item){
	if(!item) return;
	item.onmousedown = function(ev){
		dragObject  = this;
		mouseOffset = getMouseOffset(this, ev);
		return false;
	}
}

function SetHyperLink(v)
{
var elNode = null;

	if (v)
	{
		if (browser.isMicrosoft)
		{
		    SetSelection();
			
			if (v.link && v.link!="")
			{
				richeditor.ExecRichEditCommand("CreateLink",v.link);					
				if (v.nofollow)
				{
					sel.collapse();
					elNode = sel.parentElement();
					while (elNode!=null && elNode.tagName!="A")
						elNode = elNode.parentElement
					if (elNode)
						elNode.rel = "nofollow";
				}
			}
			else
				 richeditor.ExecRichEditCommand("Unlink",null);
		}
		else
		{
			if (v.link && v.link!="")
			{
                var selectedRange = richeditor.GetWindow().getSelection();			    
              
				richeditor.ExecRichEditCommand("CreateLink",v.link);	
				    					
                elNode = selectedRange.focusNode.parentNode;
                
				while (elNode!=null && elNode.tagName!="A")
					elNode = elNode.parentNode;

				 if(elNode != null)
				 {
				      var nextElement = elNode.nextSibling;
				      var spcNode =richeditor.GetDocument().createTextNode(" ");
                      
                      if(nextElement == null)
                      {
                        elNode.parentNode.appendChild(spcNode);
                      }
                      else if(nextElement.nodeType != 3)
                      {
                        nextElement.parentNode.insertBefore(spcNode,nextElement);
                      }   
                      var range = richeditor.GetDocument().createRange();
    				  range.selectNode(elNode.nextSibling);
				      range.collapse(true);

                      selectedRange.removeAllRanges();
                      selectedRange.addRange(range);
                 }
		    }
			else
				 richeditor.ExecRichEditCommand("Unlink",null);
		}
	}
	
	if (browser.isMicrosoft && richeditor.EditMode == true)
	selCurrent = null;
}