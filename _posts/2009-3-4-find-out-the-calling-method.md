---
layout: post
title: "Find out the calling method"
description: ""
date: 2009-03-04 12:00:00 UTC
category: 
tags: [.net, reflection, c#]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>This is one that I found really interesting. I needed to log which methods were calling a certain method, I knew that reflection was the answer but I never thought it would be so easy.</p>
<p>Have you used the CallStack window on your VisualStudio when you are debugging? Well, all that information is available through the <a href="http://msdn.microsoft.com/pt-br/library/system.diagnostics.stacktrace.aspx">System.Diagnostics.StatckTrace class</a>. This means that you have access to method calling stack to do whatever you like.</p>
<p>The StackTrace is composed of an array of StackFrames which represent each method on the stack.</p>
<p>To demonstrate this I&rsquo;ll create a class with four methods (Method1, Method2, Method3, Method4) which will be called in the following order:</p>
<p>Main -&gt; Method1 -&gt; Method2 -&gt; Method3 -&gt; Method4</p>
<pre class="brush: csharp" title="code">
class Program
{
    static void Main(string[] args)
    {
        StackTraceTest stt = new StackTraceTest();
        stt.Method1();
    }
}

class StackTraceTest
{
    public void Method1()
    {
        Console.Out.WriteLine(&quot;Inside Method1&quot;);
        Method2();    
    }

    private void Method2()
    {
        Console.Out.WriteLine(&quot;Inside Method2&quot;);
        Method3();
    }

    private void Method3()
    {
        Console.Out.WriteLine(&quot;Inside Method3&quot;);
        Method4();
    }

    private void Method4()
    {
        Console.Out.WriteLine(&quot;Inside Method4&quot;);
        StackTrace st = new StackTrace();
        StackFrame[] frames = st.GetFrames();
        for (int i = 0; i &lt; frames.Count(); i++)
        {
            Console.Out.WriteLine(&quot;StackTrace info: Index: {0}/Method: {1}&quot;, 
                i, frames[i].GetMethod().Name);
        }
    }
}
</pre>
<p>Each method displays a message on the console letting you know that it has been invoked. When the program gets to Method4 it will print the name of the methods on the stack.</p>
<p>Here is the output:</p>
<p><img src="http://www.gbogea.com/upload/StackTraceOutput.jpg" alt="" /></p>
<p>Put a break point on the end of the Method4 and you can take a look at VisualStudios CallStack window to compare with the console output.</p>
<p><img src="http://www.gbogea.com/upload/CallStack.jpg" alt="" /></p>
<p>As a test you might want to move the StackTrace code to the other methods to see that it will show all the method that led to method currently executing.</p>
<p>For more information look at the <a href="http://msdn.microsoft.com/pt-br/library/system.diagnostics.stackframe.aspx">StackFrame</a> and <a href="http://msdn.microsoft.com/pt-br/library/system.reflection.methodbase.aspx">MethodBase</a> classes at <span class="caps">MSDN</span>.</p>
</div>