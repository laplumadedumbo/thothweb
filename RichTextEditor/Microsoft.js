// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


//** Global object for all Javascript functions.
if (typeof Microsoft == "undefined") 
{

var Microsoft = {};

}

//* Function to support creation of namespace
Microsoft.namespace = function() 
{
var arg = arguments; // variable arguments to this function.
var obj=null;
var i, j;
var namespaceStr;

for (i=0; i <arg.length; ++i) 
{

namespaceStr = arg[i].split(".");
obj=Microsoft;

// Ignore Microsoft if its included in namespace string that is passed in
for (j=(namespaceStr[obj] == "Microsoft") ? 1 : 0; j < namespaceStr.length; ++j) 
{

    obj[namespaceStr[j]] = obj[namespaceStr[j]] || {};
    obj=obj[namespaceStr[j]];
}

}

return obj;

};

//* Function to support extending a baseclass.
Microsoft.extend = function(subClass, baseClass,overrides) 
{

function inheritance() {}


inheritance.prototype = baseClass.prototype;
subClass.prototype = new inheritance();
subClass.prototype.constructor = subClass;
subClass.baseClass = baseClass.prototype;


if (baseClass.prototype.constructor == Object.prototype.constructor) 
{
   baseClass.prototype.constructor=baseClass;
}

if (overrides) 
{

    for (var ovrMethod in overrides) 
    {
        baseClass.prototype[ovrMethod]=overrides[ovrMethod];
    }

}

}

// Add the following namespaces by default to Microsoft

Microsoft.namespace("js","js.ui","js.util","js.event");
// JScript source code

