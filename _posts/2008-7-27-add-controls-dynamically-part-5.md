---
layout: post
title: "Add Controls Dynamically - Part 5"
description: ""
date: 2008-07-27 12:00:00 UTC
category: 
tags: [asp.net, dynamic, controls]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>In parts 1 to 4 I have talked about creating dynamic controls and adding them  to the page. I haven&rsquo;t talked about a important aspect of Asp.Net that is very  important for any developers and specially if you are dealing with dynamic  controls. So in this article I&rsquo;m going to talk about the Asp.Net Life  cycle.</p>
<p>Related posts:</p>
<ul>
    <li><a href="http://www.gbogea.com/2008/7/6/add-controls-dynamically">Add  Controls Dynamically &ndash; Part 1</a></li>
    <li><a href="http://www.gbogea.com/2008/7/14/add-controls-dynamically-part-2">Add  Controls Dynamically &ndash; Part 2</a></li>
    <li><a href="http://www.gbogea.com/2008/7/22/add-controls-dynamically-part-3">Add  Controls Dynamically &ndash; Part 3</a></li>
    <li><a href="http://www.gbogea.com/2008/7/22/add-controls-dynamically-part-4">Add  Controls Dynamically &ndash; Part 4</a></li>
    <li><a href="http://www.gbogea.com/2008/7/27/add-controls-dynamically-part-5">Add  Controls Dynamically &ndash; Part 5</a></li>
</ul>
<h2>Asp.Net Life Cycle</h2>
<p>As I said in previous articles the state of the page or any of it&rsquo;s  components is not stored on the sever. Everything is stored on the page. This is  due to the characteristics of <span class="caps">HTTP</span> protocol which is  stateless. Having this in mind, every time there is a postback the structure of  the page is recreated so that you can have access to the controls through code.  I&rsquo;m not sure if I made myself clear so here another way to put it:</p>
<p><span class="caps">HTML</span> is <span class="caps">HTML</span> and that is it.  When you look at the page in the browser all there is is <span class="caps">HTML</span>, there is no C# or VB.Net. Only when the page is  submitted to the server is that the <span class="caps">HTML</span> is going to be  read transformed in server side controls. The programmer then uses these server  side controls to manipulate the controls. At the end this controls will render  <span class="caps">HTML</span> again which will be sent to the page.</p>
<p>Ok, so why is this important? Well, the process of creating the server side  code from the <span class="caps">HTML</span> happens in several stages which fire  events that let you interact with the components in the right moment.</p>
<p>The life cycle has a lot of stages but the ones we are interested in are the  Initialization and Load. For more details on all stages check out <a href="http://msdn.microsoft.com/en-us/library/ms178472.aspx"><span class="caps">MSDN</span></a></p>
<h3>Initialization</h3>
<p>In this stage the controls available on the page (not the ones created  dynamically) become available. The ViewState has not been retrieved however and  therefore the control values cannot be loaded in the controls. The postback data  is also not available at this stage.</p>
<p>The Load stage has three events to help  the programmer:</p>
<ol>
    <li>PreInit</li>
    <li>Init</li>
    <li>InitComplete</li>
</ol>
<p><img src="http://www.gbogea.com/upload/Initialization.jpg" alt="" /></p>
<p>In the PreInit event the controls don&rsquo;t even exist yet. They have not been  created.</p>
<p>If you set a property like the Text of a TextBox in these events this value  will be lost in future stages when the control is loaded. If you want to use  these events you need to declare the following methods in your code:</p>
<pre title="code" class="brush: csharp">
void Page_PreInit(object sender, EventArgs e)
    {

    }

    void Page_Init(object sender, EventArgs e)
    {

    }

    void Page_InitComplete(object sender, EventArgs e)
    {

    }

</pre>
<h3>Load</h3>
<p>This is an important stage for us because it is here that the values of the  controls are loaded. Before this stage you might have the controls but it&rsquo;s  values have are not there yet.</p>
<p>The Load stage has three events to help the  programmer:</p>
<ol>
    <li>PreLoad</li>
    <li>Load</li>
    <li>LoadComplete</li>
</ol>
<p><img alt="" src="http://www.gbogea.com/upload/Load.jpg" /></p>
<p>We have used the Page_Load event in our previous examples to create our  dynamically created controls. The thing to watch is that when these controls are  created they don&rsquo;t have it&rsquo;s values from the page loaded yet. If you want to get  the values from the controls you can only do it in the Page_LoadComplete event.  Only then all the values of the controls will be loaded.</p>
<p>You can test this using creating the method for each of these events and  using the debugger to check the controls we the event is hit.</p>
<pre class="brush: csharp" title="code">
void Page_PreLoad(object sender, EventArgs e)
    {

    }

    void Page_Load(object sender, EventArgs e)
    {
        //when the user first enters the page set the count as zero
        if (!IsPostBack)
        {
            ViewState[&quot;count&quot;] = 0;
        }
        //on every subsequent postback, check the control count
        //and recreated all the controls
        else
        {
            int controlCount = (int)ViewState[&quot;count&quot;];
            for (int i = 0; i &lt; controlCount; i++)
            {
                //notice that now we pass the i as parameter
                createDynamicControls(i);
            }
        }
    }

    void Page_LoadComplete(object sender, EventArgs e)
    {

    }

</pre>
<p>Notice that I have used the Load event defined in the previous articles but  the methods for the other events are new. If you set a breakpoint and use the  Watch to inspect the controls you can see when the values have been loaded.</p>
<p>Ok, so if you need to create the controls you do it where we&rsquo;ve been doing up  to now: In the Page_Load</p>
<p>If you need to get the values from these controls right after you loaded them  you need to wait for the Page_LoadComplete event.</p>
<p><img src="http://www.gbogea.com/upload/PageLifecycle.jpg" alt="" /></p>
<p>Well, I hope this info was useful.</p>
</div>