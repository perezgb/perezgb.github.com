---
layout: post
title: "Problem with FormView inside UpdatePanel"
description: ""
date: 2008-06-06 12:00:00 UTC
category: 
tags: [updatepanel, .net, asp.net, formview]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Today I struggled for a while with a FormView that for no apparent reason  wasn&rsquo;t updating some of my fields to the database. After sometime I noticed that  the fields that were not getting inserted into the database where the ones I had  wrapped inside an UpdatePanel. Coincidence? I don&rsquo;t think so&hellip;</p>
<p>I did a few tests and noticed that that was the problem indeed. I can&rsquo;t say  exactly what is the problem but it seems like that the FormView doesn&rsquo;t like to  have it&rsquo;s fields inside an UpdatePanel.</p>
<p>The way I found to get around this issue is to populate the parameters  manually in the events of the ObjectDataSource (ODS). So if you&rsquo;re trying to  insert a record you might using the Inserting event of the <span class="caps">ODS</span> to populate the problematic paramters:</p>
<pre title="code" class="brush: csharp">
protected void ObjectDataSource1_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        e.InputParameters[&quot;FirstName&quot;] = ((TextBox)FormView1.FindContro(&quot;TextBoxFirstName&quot;)).Text;
        e.InputParameters[&quot;LastName&quot;] = ((TextBox)FormView1.FindContro(&quot;TextBoxLastName&quot;)).Text;
        e.InputParameters[&quot;City&quot;] = ((TextBox)FormView1.FindContro(&quot;TextBoxCity&quot;)).Text;
    }

</pre>
<p>In the Inserting event of the <span class="caps">ODS</span> the InputParamters  have already been populated (at least the ones outside the UpdatePanel) but the  record hasn&rsquo;t been inserted yet, so you are intercepting the parameters,  adjusting it&rsquo;s values and then letting it continue with the insertion.</p>
<p>I hope this code spares someone to have to go through the tests I had to do.</p>
</div>