---
layout: post
title: "Ajax.BeginForm: Clear Form After Submit"
description: ""
date: 2008-11-26 12:00:00 UTC
category: 
tags: [mvc, ajax, jquery, asp.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>If you are using <span class="caps">ASP</span>.NET <span class="caps">MVC</span>,  here is a simple tip on how to clear an ajax form after successfully submitting  it with a little help from <a href="http://www.jquery.com/">jQuery</a>.</p>
<p>The first step is to get the <a href="http://malsup.com/jquery/form">jQuery  Form Plugin</a>, add it to your Scripts folder and then add a script reference  to it in your masterpage. <a href="http://haacked.com/archive/2008/09/30/jquery-and-asp.net-mvc.aspx">jQuery  is already included in the projects since the release of the <span class="caps">MVC</span></a> so you don&rsquo;t need to download and include it.</p>
<pre class="brush: csharp" title="code">
&lt;script src=&quot;../../Scripts/jquery-1.2.6.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
&lt;script src=&quot;../../Scripts/jquery.form.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
</pre>
<p>You can then create a function that will call the clearForm function from the  Form Plugin.</p>
<pre class="brush: jscript" title="code">
&lt;script type=&quot;text/javascript&quot;&gt;
   function done() {
       $('form').clearForm();
   }
&lt;/script&gt;
</pre>
<p>Now you can use the OnSuccess callback from the Ajax.BeginForm helper to point  to the done() function we just created. This function will be called  automatically after the form submit completed successfully.</p>
<pre class="brush: csharp" title="code">
&lt;% using (Ajax.BeginForm(&quot;SaveComment&quot;, new AjaxOptions { 
         UpdateTargetId = &quot;feedback&quot;, 
         InsertionMode = InsertionMode.Replace, 
         OnSuccess = &quot;done&quot;}))
{ %&gt;
</pre>
<p>Here is the complete code:</p>
<pre class="brush: xhtml" title="code">
&lt;script type=&quot;text/javascript&quot;&gt;
            function done() {
                $('form').clearForm();
            }
    &lt;/script&gt;
    &lt;h2 id=&quot;feedback&quot;&gt;&lt;/h2&gt;
    &lt;fieldset&gt;
        &lt;% using (Ajax.BeginForm(&quot;SaveComment&quot;, new AjaxOptions { 
               UpdateTargetId = &quot;feedback&quot;, 
               InsertionMode = InsertionMode.Replace, 
               OnSuccess = &quot;done&quot;}))
           { %&gt;
            &lt;dl&gt;
                &lt;dd&gt;&lt;%= Html.TextBox(&quot;Comment.Author&quot;)%&gt;&lt;/dd&gt;
                &lt;dd&gt;&lt;input type=&quot;submit&quot; title=&quot;Submit&quot; /&gt;&lt;/dd&gt;
            &lt;/dl&gt;
        &lt;% } %&gt;
    &lt;/fieldset&gt;

</pre>
<p>&nbsp;</p>
</div>