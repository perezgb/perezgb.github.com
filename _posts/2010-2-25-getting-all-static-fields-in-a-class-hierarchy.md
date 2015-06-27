---
layout: post
title: "Getting All Static Fields In a Class Hierarchy"
description: ""
date: 2010-02-25 12:00:00 UTC
category: 
tags: [reflection, silverlight, wpf]
comments: true
---
{% include JB/setup %}

<div id="post">
<div>Using reflection is pretty straight forward and that's why I found it weird that I was trying to get all the Dependency Properties for the Silverlight&nbsp;ComboBox but I was only getting the fields defined in the class itself. None of the fields found in the base classes where being retrieved. Only then&nbsp;I learned something new about reflection and static fields. Here is the code I expected to work but doesn't.</div>
<pre title="code" class="brush: csharp">
FieldInfo[] fields =
     typeof (ComboBox).GetFields(BindingFlags.Static | BindingFlags.Public);</pre>
<div>When reflecting over a class to get all it's static fields you will find out that static fields from base classes are really not retrieved by default.&nbsp;And since all Dependency Properties are static fields you can't get the DP's from the base classes. If you want to retrieve all&nbsp;the static fields in a class hierarchy you need a special BindingFlag: BindingFlags.FlattenHierarchy. This will bring all your static fields including the one from the base classes.</div>
<pre title="code" class="brush: csharp">
 FieldInfo[] fields =
     typeof (ComboBox).GetFields(BindingFlags.Static | BindingFlags.Public |
                                 BindingFlags.FlattenHierarchy);</pre>
<p>&nbsp;Hope this is a useful tip.</p>
</div>