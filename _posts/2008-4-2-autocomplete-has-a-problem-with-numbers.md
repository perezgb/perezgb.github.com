---
layout: post
title: "Autocomplete has a problem with numbers"
description: ""
date: 2008-04-02 12:00:00 UTC
category: 
tags: [.net, controltoolkit, ajax, asp.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>When using Ajax ControlTookit Autocomplete Extender I noticed that when the  value I wanted to dislplay was a number starting with 0 (zero), like 0012, this  initiating zero is removed and the result would show as 12. I didn&rsquo;t want this  beahavior so after googling for a few minutes I found a post that had a  solution.</p>
<p>The each string of the list that is returned to the page should have escaped  double quotes before and after the string. In the following example I take a  DataTable and loop through it, creating a new List<string> in which I add the  element 0 (zero) of each row sorrounding it with the escaped double quotes.</string></p>
<pre class="brush: csharp" title="code">
List&lt;string&gt; list = new List&lt;string&gt;(10); 

 for (int i = 0; i &lt; dt.Rows.Count; i++)
 {
     list.Add(&quot;\&quot;&quot;+ dt.Rows[i][0].ToString() + &quot;\&quot;&quot;);
 }

 string[] arrayString = list.ToArray();

</pre>
<p>I can only assume that when the result is rendered on the page ajax library  try to convert the results to numbers. Surrounding each number with escaped  double quotes forces them to be treated as strings.</p>
<p>Thanks to my friend Allison Bertoloto who brought this problem to my  attention.</p>
</div>