---
layout: post
title: "Bind DropDownList to Enum"
description: ""
date: 2008-07-02 12:00:00 UTC
category: 
tags: [enum, databind, asp.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Unfortunately you can&rsquo;t Databind an Enum to a DropDownList or to any control for that matter. You can however transform the Enum into something<span class="Apple-converted-space">&nbsp;</span><b>&ldquo;bindable&rdquo;</b><span class="Apple-converted-space">&nbsp;</span>like a HashTable.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">First let&rsquo;s define an Enum to use in our example:</p>
<pre title="code" class="brush: csharp">
public enum Cars
{
    Ford = 1,
    Kia = 2, 
    Mitsubishi = 3,
    Volkswagen = 4
}</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">Now that we have a method that will allow an enum to a HashTable all we need to do is bind the hash to the DropDownList.</span></p>
<pre title="code" class="brush: csharp">
//call our previously defined method to create the HashTable
        Hashtable ht = GetEnumForBind(typeof(Cars));

        //set the HashTable as the DataSource
        DropDownList1.DataSource = ht;
        //set the value of the Hash as the display
        DropDownList1.DataTextField = &quot;value&quot;;
        //set the key of the Hash as the value
        DropDownList1.DataValueField = &quot;key&quot;;

        DropDownList1.DataBind();</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">That is it, it should now bind without problems. There are alternate solutions that do not involve binding but instead adding each item of the enum to the DropDownList. You can take a look at the<span class="Apple-converted-space">&nbsp;</span><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://forums.asp.net/p/1269514/2395348.aspx#2395348">asp.net forums</a><span class="Apple-converted-space">&nbsp;</span>to see alternate solutions proposed by other members.</span></p>
</span></p>
</div>