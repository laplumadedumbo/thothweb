// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


Microsoft.js.event.EventHandler = function(element, eventType, fn, obj, useCapture)
{
    this.useCapture = useCapture || false;
    this.element = element;
    this.eventType = eventType;
    this.fnSig = fn;
    this.args = new Array();
    this.resetArgs = true;
    
    var pattern = "\\(";
    var resb = new RegExp(pattern);
    var m = resb.exec(fn);
        
    if(m != null)
    {
      var reeb = new RegExp("\\)");
      var em = reeb.exec(fn);
      this.resetArgs = false;     
      var params = fn.substring(m.index + 1, em.index);   
      var temp = new Array();
      temp = params.split(',');
       
      for(var i=0; i < temp.length; i++)
      {
         this.args.push(temp[i]);
      }
      
      this.fnSig = fn.substring(0,m.index);
   }
      
    
    var self = this;
    
    this.getEvent = function(event)
    {
        if (!event) event = window.event;

        if (event.target)
            if (event.target.nodeType == 3) event.target = event.target.parentNode;
        else if (event.srcElement)
            event.target = event.srcElement;

        return event;
    }

    this.callback = function(e) 
    {
       e = self.getEvent(e);
       
       var fnArgs = new Array();
       fnArgs.concat(self.args);
       
       // call the eventhandler      
       fnArgs.push(e);
       obj[self.fnSig].apply(obj, fnArgs);
       fnArgs = null;
    }
}

Microsoft.js.event.EventManager = 
{
    add: function (eventHandler)
    {
        if (eventHandler.element.addEventListener)
        {
            eventHandler.element.addEventListener(eventHandler.eventType, eventHandler.callback, eventHandler.useCapture);
            return true;
        }
        else if (eventHandler.element.attachEvent)
            return eventHandler.element.attachEvent("on" + eventHandler.eventType, eventHandler.callback);
    },

    remove: function (eventHandler)
    {
        if (eventHandler.element.removeEventListener)
        {
            eventHandler.element.removeEventListener(eventHandler.eventType, eventHandler.callback, eventHandler.useCapture);
            return true;
        }
        else if (eventHandler.element.detachEvent)
            return eventHandler.element.detachEvent("on" + eventHandler.eventType, eventHandler.callback);
    }
}

