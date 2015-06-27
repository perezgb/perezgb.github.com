---
layout: post
title: "Set a property of a server tag dynamically"
description: ""
date: 2008-09-28 12:00:00 UTC
category: 
tags: [asp.net, .net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Setting a property of a server tag is pretty standard stuff:</p>
<pre class="brush: csharp" title="code">

&lt;asp:TextBox ID=&quot;TextBox1&quot; runat=&quot;server&quot; Text=&quot;My name is Gabriel&quot; /&gt;
</pre>
<p>Now, what if you want to use a class to set the value?</p>
<pre class="brush: csharp" title="code">

&lt;asp:TextBox ID=&quot;TextBox1&quot; runat=&quot;server&quot; Text='&lt;%= DateTime.Now %&gt;' /&gt;
</pre>
<p>This will give you an error like this:</p>
<p style="margin: 30px; color: red;">Server tags cannot contain &lt;% ... %&gt;  constructs</p>
<p>If you ever need to solve this kind of problem I found a very interesting  solution in the <a href="http://weblogs.asp.net/infinitiesloop/archive/2006/08/09/The-CodeExpressionBuilder.aspx">Infinites  Loop Blog</a></p>
<p>It&rsquo;s a very clever solution to use the Expression Builders, introduced in the  Asp.Net 2.0 feature, to let you execute your custom code within the server tags.</p>
</div>