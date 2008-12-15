// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

Microsoft.js.ui.TabStrip = function(Id)
{
  this.uiElement = document.getElementById(Id);
  this.tabs = new Array();
  this.activeTab = null;
}

Microsoft.js.ui.TabStrip.prototype.AddTab = function(tab)
{
  this.tabs[tab.Id] = tab;
  
  if(tab.isActive == "True")
    this.activeTab = tab;
}
 

Microsoft.js.ui.Tab = function(parent,Id,tabName,isActive)
{
  this.Id = Id;
  this.uiElement = document.getElementById(Id);
  this.tabName = tabName;
  this.isActive = isActive;
  this.parent = parent;  
  
  this.Click = new Microsoft.js.event.EventHandler(this.uiElement, 'click', 'SetActive', this, false);
  Microsoft.js.event.EventManager.add(this.Click);
}

Microsoft.js.ui.Tab.prototype.SetActive = function()
{
  if(this.parent.activeTab != null)
  { 
    this.parent.activeTab.uiElement.className = "InActiveTab";
    this.parent.activeTab.uiElement.firstChild.className = "InActiveTab";
  }
  
  this.uiElement.className = "ActiveTab";
  this.uiElement.firstChild.className = "ActiveTab";  
  this.parent.activeTab = this;
}


