---
layout: post
title: "Windows 7 x64 and Oracle Client"
description: ""
date: 2009-07-07 12:00:00 UTC
category: 
tags: [visualstudio, oracle]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>I installed my Windows 7 x64 this weekend and so far everything is great. Much better than when I was running on Vista.</p>
<p>I thought I had run into my first problem with 7 when was setting up my Oracle connection. I installed the Oracle Client 11g x64 successfully, I could even connect to the database using SqlPlus. My Visual Studio on the other hand disagreed. When I tried to configure a connection to my Oracle server I got the following error:</p>
<p><em>Exception: Attempt to load Oracle client libraries threw <br />
BadImageFormatException. This problem will occur when running in 64 <br />
bit mode with the 32 bit Oracle client components installed</em></p>
<p>This was weird, after all I could connect to the database through SqlPlus, right? The problem is that VS2008 is 32 bit and as such it needs a 32 Oracle Client in order to connect to the db. When I installed the 32 bit client VS was able to connect to the db on the spot. So the only problem I had with Windows 7 so far had nothing to do with Windows 7, in fact you would run into the same situation in any other 64 bit version of Windows.</p>
<p>&nbsp;</p>
</div>