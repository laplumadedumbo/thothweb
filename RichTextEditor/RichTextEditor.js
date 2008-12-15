// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


// Constructor for RichTextEditor
// The id is of an IFrame.
function RichTextEditor(Id)
{
  this.Id = Id ;
  this.Helper = new Util();
  this.GetDocument().designMode = "On";
  this.EditMode = true;
  this.SelectCBMsg = "Highlight code in the editor window to create a code block";
  this.ProcessingCB = false;
  this.CodeBlockStr = "Code Block";
}

// toString method which gives you the text of your document.
RichTextEditor.prototype.toString = function() {
    return this.GetDocument().body.innerText;
}

// toHtmlString method which gives you the rich text of your document as html.
RichTextEditor.prototype.toHtmlString = function() {
    return this.GetDocument().body.innerHTML;
}

// Get element returns a HTML element given its Id within the Editor Document.
RichTextEditor.prototype.GetElement = function(elemId)
{ 
  return (this.GetDocument().getElementById) ? this.GetDocument().getElementById(elemId)
                                     : this.GetDocument().all[elemId];
}

// Returns the Document (DOM) of the Editor.
RichTextEditor.prototype.GetDocument = function() 
{
  var rv = null; 

  if (document.getElementById(this.Id).contentDocument)
  {
    rv = document.getElementById(this.Id).contentDocument;
  } 
  else 
  {
    // IE
    rv = document.frames[this.Id].document;
  }
  return rv;
}

// Returns the content window of the RichTextEditor document.
RichTextEditor.prototype.GetWindow = function()
{
    return document.getElementById(this.Id).contentWindow;
}

// Set focus to the editor.
RichTextEditor.prototype.Focus = function()
{
   this.GetWindow().focus();
}

// Set the contents of the editor document.
RichTextEditor.prototype.SetContent = function(htmltxt)
{
   var editdoc = this.GetDocument()
   editdoc.open();
   editdoc.write(htmltxt);
   editdoc.close();
}

// Set the contents of the editor document body.
RichTextEditor.prototype.SetContentBody = function(htmltxt)
{
    this.GetDocument().body.innerHTML = htmltxt;
}

// Execute RichEdit commands like setting text to bold, italic, etc.
RichTextEditor.prototype.ExecRichEditCommand = function(aName, aArg)
{
  if(!this.EditMode)
  return null;
  
  try {
   this.GetDocument().execCommand(aName,false, aArg);
   this.Focus();
  }
  catch(err)
  {
     
  }
}

// Query the command state. Returns true or false based on if its enabled or not enabled.
RichTextEditor.prototype.QueryRichEditCommandState = function(aName)
{
 if(!this.EditMode)
  return null;
  
  try {
 
    return this.GetDocument().queryCommandState(aName);
  }
  catch(err)
  {
    
  }
}

// Query the command value. 
RichTextEditor.prototype.QueryRichEditCommandValue = function(aName)
{
 if(!this.EditMode)
  return null;
  
  try {
   var val = this.GetDocument().queryCommandValue(aName);
  }
  catch(err)
  {
    val = "";
  }
  
  return val;
}

RichTextEditor.prototype.MarkCode = function()
{
if(this.EditMode == false)
 return;

if(this.ProcessingCB == true)
 return;



var selected = window.getSelection || this.GetDocument().selection ||this.GetDocument().getSelection;


    if(selected == 'undefined' || selected.type == 'None')
    {
        alert(this.SelectCBMsg);
        return;

    }
  try {         
        this.ProcessingCB = true;

        var fragment =this.GetDocument().createDocumentFragment();
        
        var codeSeg =this.GetDocument().createElement('div');
        codeSeg.className = 'codeseg';
        
        fragment.appendChild(codeSeg);
        
        var codeContent =this.GetDocument().createElement('div');
        codeContent.className = 'codecontent';
        
        codeSeg.appendChild(codeContent);
        
        var codeTitle =this.GetDocument().createElement('div');
        codeTitle.className = 'codesniptitle';
        
        codeContent.appendChild(codeTitle);
        
        var span =this.GetDocument().createElement('span');
        span.style.width = '100%';
        
        codeTitle.appendChild(span);
        
        var caption =this.GetDocument().createTextNode(this.CodeBlockStr);
        span.appendChild(caption);
            
        
        
        //For browser compatability
        // Mozilla 1.75,Safari 1.3
        if (window.getSelection)
        {
	      var selectedRange = this.GetWindow().getSelection();
    	  
	      //Get the parent node for the first Node.
	      var parent = selectedRange.anchorNode.parentNode;
	      var nextElement;
    	  
	      var pNode =this.GetDocument().createElement("P");
	      codeContent.appendChild(pNode);
    	   
    	   
	      // Collect all the siblings in the selected Range.
	      if(selectedRange.anchorNode != selectedRange.focusNode ||
	         (selectedRange.anchorNode == selectedRange.focusNode && selectedRange.anchorNode.nodeName == 'BODY'))
	      {
	        var startIndex = selectedRange.anchorOffset;
            var endIndex = selectedRange.focusOffset;
            
            var startNode;
            var endNode;
            
            if(startIndex > endIndex)
            {
              startNode = selectedRange.focusNode;
              endNode = selectedRange.anchorNode;
              var tmp = startIndex;
              startIndex = endIndex;
              endIndex = tmp;
            }
            else if(selectedRange.anchorNode.nodeName == 'BODY')
            {
              startNode = selectedRange.anchorNode.firstChild;
              endNode = selectedRange.focusNode.lastChild;
              parent = selectedRange.anchorNode; //Body node
              
              // To allow user to add further to the post after the code segment is created.
              brElem =this.GetDocument().createElement("br");
              parent.appendChild(brElem);
            }
            else
            {
               startNode = selectedRange.anchorNode;
               endNode = selectedRange.focusNode;
             }
            
             // There is one more scenario here. On multi line selected you could have partial selections.
             if(startIndex > 0)
             {
               var beforetext = startNode.textContent.substring(0,startIndex);
               var aftertext = startNode.textContent.substring(startIndex,startNode.textContent.length);
               startNode.textContent = beforetext;
               nextElement =this.GetDocument().createTextNode(aftertext);
               
               if(startNode.nextSibling != null)
               {
                parent.insertBefore(nextElement,startNode.nextSibling);
                parent.insertBefore(codeSeg,startNode.nextSibling);
               }
               else
               {
                parent.appendChild(nextElement);      
                parent.appendChild(codeSeg);      
               }
             }
             else 
             {
	           nextElement = startNode;
	           parent.insertBefore(codeSeg,nextElement);      
	         }
	         
	         
	         var elements = new Array();
	         var i=0;
	         while(nextElement != null && nextElement.isSameNode(endNode) == false)
	         { 
	            elements[i] = nextElement;
                nextElement = nextElement.nextSibling;
                i++;
	         }
    	     
    	     // There is one more scenario here. On multi line selected you could have partial selections.
             if(endIndex > 0 && selectedRange.anchorNode.nodeName != 'BODY')
             {
               var beforetext = endNode.textContent.substring(0,endIndex);
               var aftertext = endNode.textContent.substring(endIndex,endNode.textContent.length);
               endNode.textContent = beforetext;
               nextElement =this.GetDocument().createTextNode(aftertext);
               
               if(endNode.nextSibling != null)
               {
                parent.insertBefore(nextElement,endNode.nextSibling);
               }
               else
                parent.appendChild(nextElement);
               
               elements[i] = endNode;
             }
             else if(nextElement != null)
             {
	            elements[i] = nextElement;
	         }
	         
	         i++;
    	     
	         // Add the collected siblings to the DIV code block.
	         var index;
	         for(index in elements)
	         {
	            // NOTES appendChild
                // If child is a reference to an existing node in the document, 
                // appendChild moves it from its current postion to the new position 
                // (i.e. there is no requirement to remove the node from its parent node 
	            // before appending it some other node). 
	            pNode.appendChild(elements[index]);
	         }
	      }
	      else 
	      {
	        var startIndex = selectedRange.anchorOffset;
            var endIndex = selectedRange.focusOffset;
            
            var seltext;
            var beforetext;
            var aftertext;
            
            // The selection was made in the reverse direction.
            // Keep the convention of beforetext &text after as from left to right always independent of how the user
            // selected.
            if(startIndex > endIndex)
            {
              seltext = selectedRange.anchorNode.textContent.substring(startIndex,endIndex);
              beforetext = selectedRange.anchorNode.textContent.substring(0,endIndex);
              if(startIndex == selectedRange.anchorNode.textContent.length)
              {
                aftertext = ""; 
              }
              else
              {
               aftertext = selectedRange.anchorNode.textContent.substring(startIndex,selectedRange.anchorNode.textContent.length);
              }
            }
            else
            {
                seltext = selectedRange.anchorNode.textContent.substring(startIndex,endIndex);
                beforetext = selectedRange.anchorNode.textContent.substring(0,startIndex);
                aftertext = selectedRange.anchorNode.textContent.substring(endIndex,selectedRange.anchorNode.length);
            }
            
            // Note that there are 3 cases here.
            //
            // 1. You make a selection in between.
            // 2. You make a selection from start to somewhere in the middle
            // 3. You make a selection from middle to end.
            // 4. You select the entire text node.
            //
            // Common things are parent node, total length of the string, start,end index and the codesegment div
            // The variable is the selected text.
            
            //Case 4:
            if(selectedRange.anchorNode == selectedRange.focusNode &&
               seltext.length == selectedRange.anchorNode.length)
            {
                nextElement = selectedRange.anchorNode.nextSibling;
                pNode.appendChild(selectedRange.anchorNode);
                
                if(nextElement == null)
                {
                    parent.appendChild(codeSeg);
                    // To allow user to add further to the post after the code segment is created.
                    brElem =this.GetDocument().createElement("br");
                    parent.appendChild(brElem);
                }
                else
                    parent.insertBefore(codeSeg,nextElement);
            }
            // Middle selection case: 1
            else if(beforetext != "" && aftertext != "")
            {
              //Set the current text to be the before text.
              selectedRange.anchorNode.textContent = beforetext;
              
              var seltxtnode =this.GetDocument().createTextNode(seltext);
              pNode.appendChild(seltxtnode);
              
              nextElement = selectedRange.anchorNode.nextSibling;
              parent.insertBefore(codeSeg,nextElement);
              
              var aftertxtnode =this.GetDocument().createTextNode(aftertext);
              parent.insertBefore(aftertxtnode,nextElement);
              
            }
            // Case 2:You make a selection from start to somewhere in the middle
            else if(beforetext == "" && aftertext != "")
            {
              //Set the current text to be the before text.
              selectedRange.anchorNode.textContent = aftertext;
              
              var seltxtnode =this.GetDocument().createTextNode(seltext);
              pNode.appendChild(seltxtnode);
              
              if(startIndex > endIndex)
              {
                nextElement = selectedRange.anchorNode.nextSibling;    
              }
              else
                nextElement = selectedRange.anchorNode;
              
              if(nextElement == null)  
                parent.appendChild(codeSeg);
              else
                parent.insertBefore(codeSeg,nextElement);
            }
            // Case 3:
            else if(beforetext != "" && aftertext == "")
            {
              //Set the current text to be the before text.
              selectedRange.anchorNode.textContent = beforetext;
              
              var seltxtnode =this.GetDocument().createTextNode(seltext);
              pNode.appendChild(seltxtnode);
              
              nextElement = selectedRange.anchorNode.nextSibling;
              
              if(nextElement == null)
              {
                parent.appendChild(codeSeg);
                
                // To allow user to add further to the post after the code segment is created.
                brElem =this.GetDocument().createElement("br");
                parent.appendChild(brElem);
              }
              else
                parent.insertBefore(codeSeg,nextElement);
            }
           }
           selectedRange.removeAllRanges(); 
	    }
	    //Mozilla 1.75,IE 5.2 Mac,Opera 8,Netscape 4,
	    else if (this.GetDocument().getSelection)
        {
          var selectedText =this.GetDocument().getSelection();
        }
        // IE 5,IE 6,IE 7
	    else if(this.GetDocument().selection)
	    {
	        var selectedText =this.GetDocument().selection;   
	        var newRange = selectedText.createRange();
	        var modifiedHtml = fragment.firstChild.outerHTML.substring(0,fragment.firstChild.outerHTML.length-12) + newRange.htmlText + "<P align=left>&nbsp;</P></DIV></DIV><P align=left>&nbsp;</P>";
	        modifiedHtml = this.Helper.replace(modifiedHtml,"\r","");
	        modifiedHtml = this.Helper.replace(modifiedHtml,"\n","");
	        modifiedHtml = this.Helper.replace(modifiedHtml,"face=Arial","size=2");
	        newRange.text = modifiedHtml;
      
	    } 
	    else 
	    {
		    alert('select a text in the page and then press this button');
		    return;
        }
    
        
        if(this.Helper.IsIE() == true)
        {
         var htmlstr = this.GetDocument().body.innerHTML;

         htmlstr = this.Helper.replace(htmlstr,"&lt;","<");  
         htmlstr = this.Helper.replace(htmlstr,"&gt;",">");
         htmlstr = this.Helper.replace(htmlstr,"&amp;","&");
         this.GetDocument().body.innerHTML = htmlstr;
        }
       }
       catch(err)
       {    
       }
       
       if (window.getSelection)
            selectedRange.removeAllRanges(); 
            
       this.ProcessingCB = false;    
       this.Focus();
}

function Util() { }

Util.prototype.replace = function(inputstr,pattern,by)
{
  var re = new RegExp(pattern,"gim");

  var replacedstr = inputstr.replace(re,by);
  
  return replacedstr;
}

Util.prototype.ToggleDisplay = function(elemId) 
{
	var el = document.getElementById(elemId);
	
	if ( el.style.display != "none" ) 
	{
		el.style.display = 'none';
	}
	else {
		el.style.display = '';
	}
}

Util.prototype.IsIE = function()
{
  if(top.window.clientInformation && top.window.clientInformation.appName == 'Microsoft Internet Explorer')
    return true;
  else 
    return false;
}
