---
layout: post
title: "MVVM ProgressBar"
description: ""
date: 2010-01-13 12:00:00 UTC
category: 
tags: [mvvm, wpf]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>There are a lot of options for doing a ProgressBar in WPF using MVVM you can Google it and see for yourself. In this article I'll show a hybrid way of doing MVVM and having a ProgressBar that does not conform to the MVVM premiss (but it is really simple to use).</p>
<p>&nbsp;The idea is that you can do all the things you are used to do with MVVM but when you get to the ProgressBar you use events and a little code-behind.</p>
<p>Here is the code for the ViewModel:</p>
<pre class="brush: csharp" title="code">
public class MainWindowViewModel
{
    private BackgroundWorker _backgroundWorker;

    public event EventHandler TaskStarting = (s,e) =&gt; { };

    public event ProgressChangedEventHandler ProgressChanged
    {
        add { _backgroundWorker.ProgressChanged += value; }
        remove { _backgroundWorker.ProgressChanged -= value; }
    }

    public event RunWorkerCompletedEventHandler TaskCompleted
    {
        add { _backgroundWorker.RunWorkerCompleted += value; }
        remove { _backgroundWorker.RunWorkerCompleted -= value; }
    }

    private ICommand _executeLongTask;

    public ICommand ExecuteLongTask
    {
        get
        {
            if (_executeLongTask == null)
            {
                _executeLongTask = new RelayCommand(param =&gt; _backgroundWorker.RunWorkerAsync());
            }
            return _executeLongTask;
        }
    }

    public MainWindowViewModel()
    {
        _backgroundWorker = new BackgroundWorker();
        _backgroundWorker.WorkerReportsProgress = true;
        _backgroundWorker.DoWork += executeTask;
    }

    private void executeTask(object sender, DoWorkEventArgs e)
    {
        OnTaskStarting();
        for (int i = 0; i &lt; 100; i++)
        {
            Thread.Sleep(100);
            _backgroundWorker.ReportProgress(i + 1);
        }
    }

    private void OnTaskStarting()
    {
        TaskStarting(this, EventArgs.Empty);
    }
}</pre>
<p>&nbsp;This ViewModel class has three events: TaskStartiing, ProgressChanged and TaskCompleted. The last two are just events that I exposed from the BackgroundWorker that will execute my long runing task. The code-behind for the Window will subscribe to these events:</p>
<pre title="code" class="brush: csharp">
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MainWindowViewModel vm = new MainWindowViewModel();
        vm.TaskStarting += TaskStarted;
        vm.ProgressChanged += ProgressChanged;
        vm.TaskCompleted += TaskCompleted;

        DataContext = vm;
    }

    void TaskStarted(object sender, EventArgs e)
    {
        this.Dispatcher.Invoke(new Action(() =&gt; ProgressPopup.IsOpen = true));
    }

    void TaskCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
    {
        this.Dispatcher.Invoke(new Action(() =&gt; ProgressPopup.IsOpen = false));
    }

    void ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
        this.Dispatcher.Invoke(new Action(() =&gt; ProgressBar.Value = e.ProgressPercentage));
    }
}
</pre>
<p>Notice that since my long running task is being executed in another thread I need to use the Dispatcher to update any user interface elements.</p>
<p>My Window has a Popup with a ProgressBar inside it. I use the Starting and Completed events to show and hide the Popup. The ProgressBar itself is only updated in the ProgessChanged event. I also added an animation to show a blinking progress message during the execution.</p>
<pre title="code" class="brush: xhtml">
&lt;Window x:Class=&quot;MvvmProgressBar.MainWindow&quot;
        xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot;
        xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot;
        Title=&quot;MainWindow&quot; Height=&quot;300&quot; Width=&quot;400&quot;&gt;
    &lt;StackPanel&gt;
        &lt;Button Command=&quot;{Binding ExecuteLongTask}&quot;&gt;Run Long Task&lt;/Button&gt;
        &lt;Popup Name=&quot;ProgressPopup&quot; 
               Placement=&quot;Center&quot; 
               Width=&quot;300&quot;
               IsOpen=&quot;False&quot;&gt;
            &lt;Border BorderThickness=&quot;10&quot;
                    BorderBrush=&quot;Black&quot;
                    Background=&quot;Gray&quot;
                    Padding=&quot;30,50&quot;&gt;
                &lt;StackPanel&gt;
                    &lt;TextBlock Foreground=&quot;White&quot;
                           FontWeight=&quot;Bold&quot;
                           FontSize=&quot;16&quot;
                           Name=&quot;txt&quot;
                           Text=&quot;Processing...&quot;&gt;
                    &lt;TextBlock.Triggers&gt;
                        &lt;EventTrigger RoutedEvent=&quot;TextBlock.Loaded&quot;&gt;
               &lt;BeginStoryboard&gt;
                  &lt;Storyboard&gt;
                     &lt;DoubleAnimation
                        AutoReverse=&quot;True&quot;
                        Duration=&quot;0:0:1&quot;
                        From=&quot;1.0&quot;
                        RepeatBehavior=&quot;Forever&quot;
                        Storyboard.TargetName=&quot;txt&quot;
                        Storyboard.TargetProperty=&quot;Opacity&quot;
                        To=&quot;0.0&quot;/&gt;
                  &lt;/Storyboard&gt;
               &lt;/BeginStoryboard&gt;
            &lt;/EventTrigger&gt;
                    &lt;/TextBlock.Triggers&gt;
                    &lt;/TextBlock&gt;
                    &lt;ProgressBar Name=&quot;ProgressBar&quot;
                                 Height=&quot;30&quot; 
                                 BorderThickness=&quot;2&quot; /&gt;
                &lt;/StackPanel&gt;
            &lt;/Border&gt;
        &lt;/Popup&gt;
    &lt;/StackPanel&gt;
&lt;/Window&gt;
</pre>
<p>This example is really simple but it goes to show you that you may, from time to time, do some non-MVVM code and it won't make your app suck. </p>
<p>&nbsp;</p>
</div>