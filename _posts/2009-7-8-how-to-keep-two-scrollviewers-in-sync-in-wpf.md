---
layout: post
title: "How to keep two ScrollViewers in Sync in WPF"
description: ""
date: 2009-07-08 12:00:00 UTC
category: 
tags: [wpf]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>As I got to know WPF, one of the features that I really like was the ability to bind the value of one control to the value of another one. Well, the other day I had a situation where I had 2 <a href="http://msdn.microsoft.com/en-us/library/system.windows.controls.scrollviewer.aspx">ScrollViewers </a>side by side and I wanted to the two of them to be in sync. So, if I used the scrollbar one the left one I'd like the one on the right to scroll by the same increment. My first thought was then to bind the value of the offset of the scrollbars, that would be simple right? Not. The offset property is readonly. The way around this is really simple, you need you use the <a href="http://msdn.microsoft.com/en-us/library/system.windows.controls.scrollviewer.scrollchangedevent.aspx">ScrollChanged </a>event and in that event you use the method <a href="http://msdn.microsoft.com/en-us/library/system.windows.controls.scrollviewer.scrolltoverticaloffset.aspx">ScrollToVerticalOffset </a>to move the ScrollBar of the other ScrollViewer.</p>
<p>Here is an example where I have a window with two ScrollViewers, each one has a ListBox with 7 items inside. As you scroll on the left side scroll viewer to see all the items in the ListBox you will notice that the ScrollViewer on the right will follow.</p>
<pre class="brush: xhtml" title="code">
&lt;Window x:Class=&quot;WpfScrollbarSync.Window1&quot;
    xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot;
    xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot;
    Title=&quot;Window1&quot; Height=&quot;100&quot; Width=&quot;300&quot;&gt;
    &lt;Grid&gt;
        &lt;Grid.ColumnDefinitions&gt;
            &lt;ColumnDefinition Width=&quot;1*&quot; /&gt;
            &lt;ColumnDefinition Width=&quot;1*&quot; /&gt;
        &lt;/Grid.ColumnDefinitions&gt;
        &lt;ScrollViewer Grid.Column=&quot;0&quot; Name=&quot;scrollViewerLeft&quot; ScrollChanged=&quot;scrollViewerLeft_ScrollChanged&quot;&gt;
            &lt;ListBox&gt;
                &lt;ListBoxItem&gt;1&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;2&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;3&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;4&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;5&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;6&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;7&lt;/ListBoxItem&gt;
            &lt;/ListBox&gt;
        &lt;/ScrollViewer&gt;
        &lt;ScrollViewer Grid.Column=&quot;1&quot; Name=&quot;scrollViewerRight&quot;&gt;
            &lt;ListBox&gt;
                &lt;ListBoxItem&gt;1&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;2&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;3&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;4&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;5&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;6&lt;/ListBoxItem&gt;
                &lt;ListBoxItem&gt;7&lt;/ListBoxItem&gt;
            &lt;/ListBox&gt;
        &lt;/ScrollViewer&gt;
    &lt;/Grid&gt;
&lt;/Window&gt;</pre>
<p>And here is the codebehind:</p>
<pre class="brush: csharp" title="code">
public partial class Window1 : Window
{
    public Window1()
    {
        InitializeComponent();
    }

    private void scrollViewerLeft_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        scrollViewerRight.ScrollToVerticalOffset((sender as ScrollViewer).VerticalOffset);
    }
}
</pre>
<p>I hope this can save someone one minute or two. :-)</p>
</div>