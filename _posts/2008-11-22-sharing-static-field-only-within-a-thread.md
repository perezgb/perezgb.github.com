---
layout: post
title: "Sharing Static Field Only Within a Thread"
description: ""
date: 2008-11-22 12:00:00 UTC
category: 
tags: [c#]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>When you use a static field it&rsquo;s data is shared among any classes using it.  That&rsquo;s something that everyone should know. When programming web applications,  every request from the browser will span a new Thread. If you have a static  field in a class that is used in a page&rsquo;s code behind all it&rsquo;s value will be  shared by all requests.</p>
<p>This is great but sometimes you want to share the static field between a  bunch of classes that do some business logic when the browser sends a request  and you want the static field to share data among all these business logic  classes. But when you think that there will be several requests to your site  simultaneously (hopefully) you notice that the static field is not only being  shared between all business classes in the request but between all business  classes in all the requests. You might or might not want this behavior.</p>
<p>In order for a field to have a different value for each thread running, the  simplest way is to decorate the static field with a ThreadStaticAttribute  attribute.</p>
<pre title="code" class="brush: csharp">
[ThreadStaticAttribute]
public static int ThreadInfo;
</pre>
<p>If you never used this attribute here is piece of code that will prove that this  really works:</p>
<pre title="code" class="brush: csharp">
class Program
    {
        static void Main(string[] args)
        {
            ParentClass.MySharedInfo = 5;

            for (int i = 1; i &lt; 3; i++)
            {
                ParameterizedThreadStart pts = new ParameterizedThreadStart(RunTest);
                Thread t = new Thread(pts);

                t.Start(i * 10); 
        }

        }

        public static void RunTest(object val)
        {
            ParentClass.MySharedInfo = (int)val;

            ParentClass cp = new ParentClass();
            cp.Write();
            cp.ChildClass.Write();

            Thread.Sleep(1000);
            cp = new ParentClass();
            cp.Write();
            cp.ChildClass.Write();
        }
    }

    class ParentClass
    {
        [ThreadStaticAttribute]
        public static int MySharedInfo;

        public ChildClass ChildClass { get; set; }

        public ParentClass()
        {
            ChildClass = new ChildClass();    
        }

        public void Write()
        {
            Console.WriteLine(&quot;ParentClass ThreadId:{0} / MySharedInfo:{1}&quot;,
                Thread.CurrentThread.ManagedThreadId, MySharedInfo);
        }
    }

    class ChildClass
    {
        public void Write()
        {
            Console.WriteLine(&quot;ChildClass ThreadId:{0} / MySharedInfo:{1}&quot;, 
                Thread.CurrentThread.ManagedThreadId, ParentClass.MySharedInfo);
        }
    }</pre>
<p>The code above defines two classes: ParentClass and ChildClass. ParentClass has  a static field decorated with the ThreadStaticAttribute attribute and an  instance of a ChildClass. The Main method set&rsquo;s the static filed to 5. Then a  loop will start two Threads and create a new ParentClass in each of them. The  static field value is set to different values. The class will print the static  field value from the ParentClass and from the ChildClass two times. Between the  first and the second time the value is printed there is a Thread.Sleep of 1  second. This pause shows that each thread maintains it&rsquo;s own value.</p>
</div>