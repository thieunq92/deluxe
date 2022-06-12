// Copyright (C) 2005-2008 Ilya S. Lyubinskiy. All rights reserved.
// Technical support: http://www.php-development.ru/
//
// YOU MAY NOT
// (1) Remove or modify this copyright notice.
// (2) Re-distribute this code or any part of it.
//     Instead, you may link to the homepage of this code:
//     http://www.php-development.ru/javascripts/dropdown.php
//
// YOU MAY
// (1) Use this code on your website.
// (2) Use this code as part of another product.
//
// NO WARRANTY
// This code is provided "as is" without warranty of any kind.
// You expressly acknowledge and agree that use of this code is at your own risk.


// ***** Popup Control *********************************************************

// ***** at_show_aux *****
// Detect if the browser is IE or not.
// If it is not IE, we assume that the browser is NS.
var IE = document.all?true:false

// If NS -- that is, !IE -- then set up for mouse capture
if (!IE) document.captureEvents(Event.MOUSEMOVE)

// Set-up to use getMouseXY function onMouseMove
document.onmousemove = getMouseXY;

// Temporary variables to hold mouse x-y pos.s
var tempX = 0
var tempY = 0

// Main function to retrieve mouse x-y pos.s

function getMouseXY(e) {
  if (IE) { // grab the x-y pos.s if browser is IE
    tempX = event.clientX + document.body.scrollLeft
    tempY = event.clientY + document.body.scrollTop
  } else {  // grab the x-y pos.s if browser is NS
    tempX = e.pageX
    tempY = e.pageY
  }  
  // catch possible negative values in NS4
  if (tempX < 0){tempX = 0}
  if (tempY < 0){tempY = 0}  
  // show the position values in the form named Show
  // in the text fields named MouseX and MouseY
  // document.Show.MouseX.value = tempX
  // document.Show.MouseY.value = tempY
  // return true
  return tempX;
}

  function getElementsByClassName(classname, node) {
  if(!node) node = document.getElementsByTagName("body")[0];
  else node= document.getElementById(node);
  var a = [];
  var re = new RegExp('\\b' + classname + '\\b');
  var els = node.getElementsByTagName("*");
  for(var i=0,j=els.length; i<j; i++)
  if(re.test(els[i].className))a.push(els[i]);
  return a;
  }

function at_show_aux(parent, child)
{
  var p = document.getElementById(parent);
  var c = document.getElementById(child );

  var top  = (c["at_position"] == "y") ? p.offsetHeight+2 : 0;
  var left = (c["at_position"] == "x") ? p.offsetWidth +2 : 0;    
  
  for (; p; p = p.offsetParent)
  {
        top  += p.offsetTop;
        left += p.offsetLeft;
  }
  
  top = (c["at_position"] == "b") ? top+2 : top;
  left = (c["at_position"] == "b") ? tempX+10 : left;

  c.style.position   = "absolute";
  c.style.top        = top +'px';
  c.style.left       = left+'px';
  c.style.visibility = "visible";
}

// ***** at_show *****

function at_show()
{
  a =getElementsByClassName("hover_content","");
      	for (var i=0,j=a.length; i<j; i++)
      	{
      		a[i].style.visibility = 'hidden';
      	}

  var p = document.getElementById(this["at_parent"]);
  var c = document.getElementById(this["at_child" ]);

  at_show_aux(p.id, c.id);
  clearTimeout(c["at_timeout"]);
}

// ***** at_show_if
function at_show_if(parentId, childId)
{
  a =getElementsByClassName("hover_content","");
      	for (var i=0,j=a.length; i<j; i++)
      	{
      		a[i].style.visibility = 'hidden';
      	}
      	  
  var p = document.getElementById(parentId);
  var c = document.getElementById(childId);
  
  if (p["at_parent"]==undefined)
  {
  	  at_attach(p.id,c.id,"hover", "b", "pointer");
  }
  
  /*at_show();*/
  at_show_aux(p.id, c.id);
  clearTimeout(c["at_timeout"]);
}

// ***** at_hide *****

function at_hide()
{
  var p = document.getElementById(this["at_parent"]);
  var c = document.getElementById(this["at_child" ]);

  c["at_timeout"] = setTimeout("document.getElementById('"+c.id+"').style.visibility = 'hidden'", 333);
}

// ***** at_click *****

function at_click()
{
  var p = document.getElementById(this["at_parent"]);
  var c = document.getElementById(this["at_child" ]);

  if (c.style.visibility != "visible") at_show_aux(p.id, c.id); else c.style.visibility = "hidden";
  return false;
}

// ***** at_attach *****

// PARAMETERS:
// parent   - id of the parent html element
// child    - id of the child  html element that should be droped down
// showtype - "click" = drop down child html element on mouse click
//            "hover" = drop down child html element on mouse over
// position - "x" = display the child html element to the right
//            "y" = display the child html element below
// cursor   - omit to use default cursor or specify CSS cursor name

function at_attach(parent, child, showtype, position, cursor)
{
  var p = document.getElementById(parent);
  var c = document.getElementById(child);

  p["at_parent"]     = p.id;
  c["at_parent"]     = p.id;
  p["at_child"]      = c.id;
  c["at_child"]      = c.id;
  p["at_position"]   = position;
  c["at_position"]   = position;

  c.style.position   = "absolute";
  c.style.visibility = "hidden";

  if (cursor != undefined) p.style.cursor = cursor;
  
  if (position=='b')
  {
  	  p.onmousemove = at_show;
  }

  switch (showtype)
  {
    case "click":
      p.onclick     = at_click;
      p.onmouseout  = at_hide;
      c.onmouseover = at_show;
      c.onmouseout  = at_hide;
      break;
    case "hover":
      p.onmouseover = at_show;
      p.onmouseout  = at_hide;
      c.onmouseover = at_show;
      c.onmouseout  = at_hide;      
      break;
  }
}
