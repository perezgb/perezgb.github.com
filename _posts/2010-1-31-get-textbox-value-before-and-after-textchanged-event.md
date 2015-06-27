---
layout: post
title: "Get TextBox Value Before and After TextChanged Event"
description: ""
date: 2010-01-31 12:00:00 UTC
category: 
tags: [asp.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>The other day my team was doing some maintenance on a bunch of dynamically loaded UserControls and something weird started to happen, the TextChanged event of a control was firing all the time. I was sure someone had messed with the EnableViewState of the control but that is not the point, the point is that I wanted to prove it to them by looking at the value of the TextBox that was being loaded from the ViewState and then the value that was being loaded from the Form. As it turns out it is harder to do this than I thought.</p>
<p>My first idea was to debug through the .NET Framework source code. Sadly the version of the assembly System.Web (2.0.50727.4918) has no source code available yet. The Symbols have been published by Microsoft but no source code so far. So this option was out.</p>
<p>I have however the downloaded source code from a previous version of the TextBox class. Looking through it you can see the exact moment when the current value (loaded from ViewState) is replaced by the new value from the form. That is in the LoadPostData method, which is a protected virtual method. The only solution I could come up with was to create a new TextBox derived for the TextBox class and override the LoadPostData method and read the Text value before and after the change. Here is the example:</p>
<pre title="code" class="brush: csharp">
public class TextBoxExtended : TextBox
{
    protected override bool LoadPostData(string postDataKey, 
System.Collections.Specialized.NameValueCollection postCollection)
    {
        Debug.WriteLine(&quot;Before Load:&quot;+this.Text);
        Debug.WriteLine(&quot;Loading&quot;);
        bool changed = base.LoadPostData(postDataKey, postCollection);
        Debug.WriteLine(&quot;After Load:&quot;+this.Text);
        Debug.WriteLine(&quot;Changed? &quot;+changed);
        return changed;
    }
}</pre>
<p>Now if you put this TextBox in your form instead of the regular TextBox you want to monitor you will get the messages with the values in the Output window. If you want to you can take this further and create a custom event that fires with both values in the event args. If any one has an alternative method of doing this please do leave a comment.</p>
<p>Oh, if you are curious the problem was that the ViewState had been disabled on an outer panel but as I said before this wasn't the point of the exercise :-)</p>
</div>