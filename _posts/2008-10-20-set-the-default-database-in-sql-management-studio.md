---
layout: post
title: "Set the Default Database in SQL Management Studio"
description: ""
date: 2008-10-20 12:00:00 UTC
category: 
tags: [sqlserver]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Hi, this one is for the lazy programmers (like I). If you work a lot of time  on the same project chances are that you know that every time you create a new  query on <span class="caps">SQL</span> Server Management Studio it always sets  your connection to the Master database. For those who are annoyed like me, you  can configure the default database of your user to whatever you want.</p>
<ol>
    <li>On the Object Explorer expand the Security folder.</li>
    <li>Expand the Logins folder.</li>
    <li>Right click your user and select Properties.</li>
    <li>Now select the Default Database you want.</li>
</ol>
<p>That&rsquo;s it!</p>
<p><img src="http://www.perezgb.com/upload/SQLServer.jpg" alt="" /></p>
</div>