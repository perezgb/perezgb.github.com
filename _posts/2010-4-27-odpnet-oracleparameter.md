---
layout: post
title: "ODP.NET OracleParameter"
description: ""
date: 2010-04-27 12:00:00 UTC
category: 
tags: [oracle, ado.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<div style="background-color: rgb(255, 255, 255); padding-top: 5px; padding-right: 5px; padding-bottom: 5px; padding-left: 5px; margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; font-family: Arial, Verdana, sans-serif; font-size: 12px; ">
<p>The past weeks I've been doing a lot of work to improve performance at our application at work. One of the things I've done is to replace the Microsoft's OracleClient for the ODP.NET provider. In my tests I've been getting data access times up to 3 times faster than with the OracleClient. I'll post more details on this in a latter article, for now I wanted to focus on the following error I got when switching providers:</p>
<p><span style="color: rgb(255, 0, 0); "><strong>Oracle.DataAccess.Client.OracleException: ORA-01008: not all variables bound</strong></span></p>
<p>&nbsp;I found this strange since we have a data layer that abstracts the provider and we already have implementations for Oracle and SQLServer and everything was working great. It took me a while but I found out that this is was only happening in queries where I used the same parameter more than once. Here is an example:</p>
<pre title="code" class="brush: sql">
SELECT NAME, AGE FROM EMPLOYEE WHERE :AGE &gt; 18 AND :AGE &lt; 40</pre>
<p>&nbsp;What I found out is that the default behavior for the ODP.NET provider is different than the OracleClient provider. The ODP.NET sets the parameters (<a href="http://download.oracle.com/docs/html/E15167_01/OracleParameterClass.htm#i1010814">OracleParameter</a>) on the <a href="http://download.oracle.com/docs/html/E15167_01/OracleCommandClass.htm">OracleCommand </a>using the order of the parameters instead of the name. When this happens you'd have to declare the parameter twice with the same value. This behavior can be changed using the <a href="http://download.oracle.com/docs/html/E15167_01/OracleCommandClass.htm#i997666">BindByName </a>property on the OracleCommand.</p>
<pre title="code" class="brush: csharp">
var parameter = new OracleCommand();
parameter.BindByName = true;</pre>
<p>The good part for us is that since we had an abstraction layer on top of our data layer all we needed to do was make a small change to the Factory class that creates the commands for us. Everything else remains untouched.</p>
<p>Hope this helps.</p>
<p>Oh, and by the way... on .NET 4 many types under the <a href="http://msdn.microsoft.com/en-us/library/system.data.oracleclient.aspx">System.Data.OracleClient</a> namespace is marked as <span style="color: rgb(255, 0, 0); "><strong>obsolete</strong></span>. So you'd better watch out.&nbsp;</p>
</div>
</div>