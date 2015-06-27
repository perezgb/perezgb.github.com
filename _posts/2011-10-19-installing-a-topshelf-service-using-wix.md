---
layout: post
title: "Installing a Topshelf Service using Wix"
description: ""
date: 2011-10-19 12:00:00 UTC
category: 
tags: [.net, topshelf, wix]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>The idea of this post is not to introduce <a href="http://topshelf-project.com/">Topshelf </a>or <a href="http://wix.codeplex.com/">Wix</a>. If you are here I assume you figured out how cool Topshelf is, you created your service but when it came time to install it using Wix your service is not starting. That&rsquo;s what happened to me.</p>
<div>
<h2>The Solution</h2>
<p>Basically what you need to do is add an installer class derived from System.Configuration.Install.Installer.</p>
<p>To make it as simple as possible I'm taking the sample from the Topshelf documentation page (<a href="http://topshelf-project.com/documentation/getting-started/">topshelf-project.com/documentation/getting-started/</a>) and I'm defining the necessary Installer class so it can be installed from Wix or InstallUtil.exe.</p>
<p>This is the main class that will be run from the Windows Service:</p>
<pre title="code" class="brush: csharp">
internal class Program
{
    private static void Main(string[] args)
    {
        log4net.Config.XmlConfigurator.Configure();
        HostFactory.Run(
            x =&gt;
                {
                    x.Service&lt;TownCrier&gt;(
                        s =&gt;
                            {
                                s.SetServiceName(&quot;tc&quot;);
                                s.ConstructUsing(name =&gt; new TownCrier());
                                s.WhenStarted(tc =&gt; tc.Start());
                                s.WhenStopped(tc =&gt; tc.Stop());
                            });
                    x.RunAsLocalSystem();

                    x.SetDescription(&quot;Sample Topshelf Host&quot;);
                    x.SetDisplayName(&quot;TownCrierService&quot;);
                    x.SetServiceName(&quot;TownCrierService&quot;);
                });
    }
}</pre>
<p>This is the Service class itself:</p>
<pre title="code" class="brush: csharp">
public class TownCrier
{
    private readonly Timer _timer;

    public TownCrier()
    {
        ILog log = LogManager.GetLogger(typeof(TownCrier));
        log.Debug(&quot;User:&quot;+WindowsIdentity.GetCurrent().Name);
        _timer = new Timer(1000) { AutoReset = true };
        _timer.Elapsed += (sender, eventArgs) =&gt; Console.WriteLine(&quot;It is {0} an all is well&quot;, DateTime.Now);
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }
}</pre>
<p>This is the required installer class.</p>
<pre title="code" class="brush: csharp">
[RunInstaller(true)]
public class ProjectInstaller : Installer
{
    private ServiceInstaller _serviceInstaller;

    private ServiceProcessInstaller _serviceProcessInstaller;

    public ProjectInstaller()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        this._serviceInstaller = new ServiceInstaller();
        _serviceProcessInstaller = new ServiceProcessInstaller();

        this._serviceProcessInstaller.Account = ServiceAccount.LocalService;

        this._serviceInstaller.DisplayName = &quot;TownCrierService&quot;;
        this._serviceInstaller.ServiceName = &quot;TownCrierService&quot;;

        this.Installers.AddRange(new Installer[] { this._serviceProcessInstaller, this._serviceInstaller });
    }
}</pre>
<p>Notice that the Installer class and the main program class use the same ServiceName, if you don't use the same name your service will not start.</p>
<p>Finally this is a sample Wix installer for the service:</p>
<pre title="code" class="brush: xhtml">
&lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
&lt;Wix xmlns=&quot;http://schemas.microsoft.com/wix/2006/wi&quot;&gt;
	&lt;Product Id=&quot;4bafff3b-b638-4e81-ae2c-97a13c50631d&quot; 
           Name=&quot;TownCrierInstaller&quot; 
           Language=&quot;1033&quot; 
           Version=&quot;1.0.0.0&quot; 
           Manufacturer=&quot;TownCrierInstaller&quot; 
           UpgradeCode=&quot;57c76289-42b4-40b2-95f0-1652b703cefa&quot;&gt;
    
		&lt;Package InstallerVersion=&quot;200&quot; Compressed=&quot;yes&quot; /&gt;

		&lt;Media Id=&quot;1&quot; Cabinet=&quot;media1.cab&quot; EmbedCab=&quot;yes&quot; /&gt;

		&lt;Directory Id=&quot;TARGETDIR&quot; Name=&quot;SourceDir&quot;&gt;
			&lt;Directory Id=&quot;ProgramFilesFolder&quot;&gt;
				&lt;Directory Id=&quot;INSTALLLOCATION&quot; Name=&quot;TownCrierInstaller&quot; 
                   FileSource=&quot;C:\YourPath\TopShelfWix\TownCrierService\bin\Debug\&quot;&gt;
          &lt;Component Id=&quot;Service&quot; Guid=&quot;{B5DC8DF4-1B45-47F4-A2B2-791CEB3E97DF}&quot; &gt;
            &lt;File Id=&quot;TCFile&quot; KeyPath=&quot;yes&quot; Name=&quot;TownCrierService.exe&quot; /&gt;
            &lt;File Id=&quot;a1ac9979e3f2f48e39252e024a9aa75bb&quot; Name=&quot;TownCrierService.exe.config&quot; /&gt;
            &lt;File Id=&quot;ad345d1dff94646dc81e54e095d1aa7c7&quot; Name=&quot;log4net.dll&quot; /&gt;
            &lt;File Id=&quot;a6f1bc7613d5449e7a3d539422299b580&quot; Name=&quot;Topshelf.dll&quot; /&gt;
            &lt;ServiceInstall Id=&quot;TCInstall&quot;
                     Type=&quot;ownProcess&quot;
                     Name=&quot;TownCrierService&quot;
                     DisplayName=&quot;TownCrierService&quot;
                     Start=&quot;demand&quot;
                     Account=&quot;yourdomain\youruser&quot;
                     Password=&quot;password&quot;
                     ErrorControl=&quot;normal&quot; /&gt;
            &lt;ServiceControl Id=&quot;TCControl&quot;
                     Stop=&quot;both&quot;
                     Remove=&quot;uninstall&quot;
                     Name=&quot;TownCrierService&quot;
                     Wait=&quot;no&quot;/&gt;
          &lt;/Component&gt;
				&lt;/Directory&gt;
			&lt;/Directory&gt;
		&lt;/Directory&gt;

		&lt;Feature Id=&quot;ProductFeature&quot; Title=&quot;TownCrierInstaller&quot; Level=&quot;1&quot;&gt;
			 &lt;ComponentRef Id=&quot;Service&quot; /&gt; 		
			&lt;ComponentGroupRef Id=&quot;Product.Generated&quot; /&gt;
		&lt;/Feature&gt;
	&lt;/Product&gt;
&lt;/Wix&gt;&nbsp;</pre>
<h2>Why?</h2>
<p>To install a regular (non Topshelf) Windows Service you use InstallUtil.exe. When decompiling it I found out that what it does is scan the service assembly looking for classes derived from the System.Configuration.Install.Installer. Topshelf does have such a class but it&rsquo;s in the Topshelf assembly which is not the Service assembly itself hence InstallUtil can&rsquo;t find it.</p>
<p>I'll probably add more information on this issue later, for now I just want to document the solution for me and for anyone else going through the same issue.</p>
<p>I hope this helps.</p>
</div>
</div>