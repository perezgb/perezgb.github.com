---
layout: post
title: "OCIEnvNlsCreate failed with return code -1"
description: ""
date: 2009-08-17 12:00:00 UTC
category: 
tags: [.net, oracle]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>&quot;OCIEnvNlsCreate failed with return code -1 but error message text was not available&quot;. This was the error that was really anoying my last week. I've had this problem before on one of our servers and this time it showed up in one of my co-workers machine.</p>
<p>Once again <a href="http://technet.microsoft.com/en-us/sysinternals/default.aspx">Sysinternals </a>came to the rescue. When I used <a href="http://technet.microsoft.com/en-us/sysinternals/bb896645.aspx">ProcessMonitor </a>to watch the application running I noticed that it couldn't load some of Oracles dlls. The really strange part was that the dlls it was complaining about were from Oracle 10 and the client we were using were from version 11. I then decide to use <a href="http://technet.microsoft.com/en-us/sysinternals/bb896653.aspx">ProcessExplorer </a>to checkout out application process in order to find out what dlls were really loaded. Imagine my surprise when I saw that the application had the oci.dll from version oracle client 10 running. At some point someone had put a copy of this dll on the windows/system32 folder, and that was the dll that was being loaded instead of the correct one that was in the Oracle install directory.</p>
<p>After we deleted the incorrect dll our program still had the same error. Turns out that the ORACLE_HOME environment variable was not set either. Once we set the variable and restared IIS it was all fine! So, to sum up:</p>
<h3>Error:</h3>
<p><strong>OCIEnvNlsCreate failed with return code -1 but error message text was not available</strong></p>
<h3>Solution:</h3>
<p>&nbsp;1 - Check if the ORACLE_HOME environment variable is set correctly. <a href="http://sabdarsyed.blogspot.com/2008/09/setting-oracle-environment-variable.html">Here is how to do it</a>.</p>
<p>&nbsp;2 - Use Process Explorer to check out what versions of Oracle's dlls are loaded. If it's a different version from the version of the client you have installed, remove these incorrect versions from your machine. They may have been there from a previous client installation.</p>
<p>I hope this will help someone else!</p>
</div>