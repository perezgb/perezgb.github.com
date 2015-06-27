---
layout: post
title: "DataSet Designer (Editor) Missing"
description: ""
date: 2008-06-14 12:00:00 UTC
category: 
tags: [visual studio, .net, dataset]
comments: true
---
{% include JB/setup %}

<div id="post">
<p><span class="Apple-style-span" style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">his week one of my co-workers had a strange problem with his VisualStudio 2008. All the sudden he couldn&rsquo;t view the DataSet designer in any applications. When the DataSet file was clicked all that it showed was the<span class="Apple-converted-space">&nbsp;</span><span class="caps">XML</span><span class="Apple-converted-space">&nbsp;</span>file, no sign of the Designer.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">We don&rsquo;t know what happened but as I tried to figure out the problem I found that there&rsquo;s very few information about this problem on the Internet, so I decide to blog about it so that it might help someone in distress.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">If you&rsquo;re having this problem you might first try to right click the DataSet file and then select the<span class="Apple-converted-space">&nbsp;</span><b>Open With&hellip;</b><span class="Apple-converted-space">&nbsp;</span>option. A list of editors will show, if you don&rsquo;t see the<span class="Apple-converted-space">&nbsp;</span><b>DataSet Editor</b><span class="Apple-converted-space">&nbsp;</span>then you you have the same problem we did.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">&nbsp;<img src="http://www.gbogea.com/upload/dataseteditor.jpg" alt="" /></p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">I have found a few posts about it, here are the solutions proposed:
<ul>
    <li>Run the following command in the VisualStudio prompt: devenv /resetsettings</li>
    <li>Run the following command in the VisualStudio prompt: devenv /setup</li>
    <li>With Visual Studio setup disc select the Repair option.</li>
    <li>Re-install Visual Studio.</li>
</ul>
All these are valid solutions and I found people on the web who claimed that this helped them but for my unfortunate teammate it didn&rsquo;t help. The only thing that did it for us was:
<ol>
    <li>Remove VisualStudio</li>
    <li>Delete any folder left from VS installation</li>
    <li>Install VisualStudio</li>
</ol>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Finally, problem solved! I know this is not an optimal solution, far from it but at the end of the day it works. Nothing else did. I hope this post helps some else.</p>
</span></p>
</span></p>
</div>