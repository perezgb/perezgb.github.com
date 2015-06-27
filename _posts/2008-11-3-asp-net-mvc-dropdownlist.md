---
layout: post
title: "ASP.NET MVC: DropDownList"
description: ""
date: 2008-11-03 12:00:00 UTC
category: 
tags: [asp.net, .net, mvc]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Here is an example of how to use the Html.DropDownList helper method  available in the <span class="caps">MVC</span> framework to generate a combo in  you page.</p>
<p>Let&rsquo;s use the Northwind example and say that you want to create a product and  in the product for you need to select a category to which this product belongs  in a dropdownlist.</p>
<p>The first step is in the select the list of categories in the appropriate  controller action. In my case I&rsquo;m going to use the new Action.</p>
<pre title="code" class="brush: csharp">
public ActionResult New()
{
    ViewData[&quot;CatList&quot;] = new SelectList(_db.Categories.ToList(), &quot;CategoryID&quot;, &quot;CategoryName&quot;);
    return View();
}
</pre>
<p>Notice that I used the SelectList class. In this case I used the constructor  that takes 3 parameters:</p>
<ol>
    <li>The list of categories I got from my Linq DataContext</li>
    <li>The name of the property of the Category class that has the ID value that is  going to be stored in the select list and which will be the value that actually  is passed to the Product class</li>
    <li>The name of the property of the Category class that has the value I want to  display to the user</li>
</ol>
<p>The SelectList class will be used by the DropDownList helper to generate the  html code. Here is part of the View code:</p>
<pre title="code" class="brush: xhtml">
...
&lt;tr&gt;
       &lt;td&gt;Category:&lt;/td&gt;
       &lt;td&gt;&lt;%= Html.DropDownList(&quot;CategoryID&quot;,(SelectList)ViewData[&quot;CatList&quot;]) %&gt;&lt;/td&gt;
&lt;/tr&gt;
...</pre>
<p>&nbsp;</p>
</div>