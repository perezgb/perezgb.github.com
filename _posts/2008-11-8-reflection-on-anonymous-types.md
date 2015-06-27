---
layout: post
title: "Reflection on Anonymous Types"
description: ""
date: 2008-11-08 12:00:00 UTC
category: 
tags: [reflection, c#]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Using reflection on anonymous types is just like reflection on any other  type.</p>
<p>Consider this simple anonymous object being instantiated:</p>
<pre title="code" class="brush: csharp">
var anonyObject = new
{
    Name = &quot;Gabriel&quot;,
    Age = 31
};
</pre>
<p>If you want to know all the properties, it&rsquo;s values and types you can query the  object with the reflection classes like this:</p>
<pre title="code" class="brush: csharp">
//get the type of the anonymous object
Type anonyType = anonyObject.GetType();
//get all the properties info
PropertyInfo[] props = anonyType.GetProperties();
//display each of the properties info
foreach (var prop in props)
{
    Console.WriteLine(&quot;Name:{0}, Value:{1}, Type:{2}&quot;,prop.Name, 
        prop.GetValue(anonyObject, null), prop.PropertyType);
}
</pre>
<p>I just wanted to share this code because I&rsquo;m going to use it in my next article  in which I will talk about creating a helper method to transform the result from  a Linq query that returns anonymous types to a DataTable.</p>
</div>