---
layout: post
title: "ELMAH: Error logging doesn't get any simpler"
description: ""
date: 2009-05-03 12:00:00 UTC
category: 
tags: [speakout, asp.net, logging]
comments: true
---
{% include JB/setup %}

<div id="post">
<p><a href="http://code.google.com/p/elmah/">ELMAH </a>stands for Error Logging Modules and Handlers. I don't love the name but everything else about it is great!! In it's site it is described as:</p>
<p>&quot;ELMAH (Error Logging Modules and Handlers) is an application-wide error logging facility that is completely pluggable. It can be dynamically added to a running <a href="http://www.asp.net/" rel="nofollow">ASP.NET</a> web application, or even all ASP.NET web applications on a machine, without any need for re-compilation or re-deployment.&quot;</p>
<p>I'm using <a href="http://speakoutblog.codeplex.com/">this blog</a> as I write it and if I had to classify it I'd say it is a <em>pre pre alpha release</em> not suitable for anyone else but me. The only use I see for other people right now is to look around the code (what should be quick). Given my choice to put the application online in this early stage it seamed pretty obvious that that I needed to start logging the errors my users are getting so that I can work on them as soon as possible. As usual I was deciding between using <a href="http://www.nlog-project.org">NLog </a>or <a href="http://logging.apache.org/log4net/index.html">Log4Net</a> when I saw a new <a href="http://www.hanselman.com/blog/ELMAHErrorLoggingModulesAndHandlersForASPNETAndMVCToo.aspx">post by Scott Hanselman</a> talking about ELMAH.</p>
<p>As usual Scott's post saved me a lot of time! Setting up ELMAH was really as simple as he said. In a matter of minutes I was logging all errors in my application. Now I can sleep well knowing that every weekend I'll have a list of errors to work on :-)</p>
<p>I highly recommend you use this in your application even if you are already using another logging framework. The cool thing about ELMAH is that if you missed something forgot to place logging on some part of your app, ELMAH won't. It will log every error you have and store it in XML, SQLServer, Oracle...</p>
</div>