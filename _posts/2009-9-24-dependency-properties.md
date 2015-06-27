---
layout: post
title: "Dependency Properties"
description: ""
date: 2009-09-24 12:00:00 UTC
category: 
tags: [.net, wpf]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Dependency Properties are quite different from regular CLR properties. Let's take an Age property as a simple example of a normal property. We usually define a private field of type int and then define a Age property wrapping this field with get and set accessors. The code should be something like this:</p>
<pre class="brush: csharp" title="code">
public class Person
{
    private int _age;
    public int Age
    {
        get { return _age; }
        set { _age = value; }
    }
}</pre>
<p>If instead we wanted to create age as a dependency property instead the code would look like this:</p>
<pre class="brush: csharp" title="code">
public class Person : DependencyObject
{
    public static readonly DependencyProperty AgeProperty = DependencyProperty.Register(&quot;Age&quot;, typeof (int),
                                                                                        typeof (Person));
    public int Age
    {
        get { return (int)GetValue(Person.AgeProperty); }
        set { SetValue(Person.AgeProperty, value); }
    }
}</pre>
<p>&nbsp;</p>
<p>Wow, that is really different isn't it? Actually it is so different that I rather take it apart so that we can understand each piece of this code.</p>
<h4>Part 1: The class</h4>
<p>Dependency properties can only be used in classes that derive from DependencyObject. As you can see in the first example the Person class is a simple class derived from the Object class. In the second example Person is derived from DependencyObject.</p>
<p>DependencyObject is the class that has all the necessary infrastructure that make DP's work. As we go through this article I'll mention more about this class but for now just keep in mind that if you want or need to use DP's your class must derive from DependencyObject.</p>
<h4>Part 2: Declaring the Property</h4>
<p>As you may have noticed the two examples are totally different. By convention the DP is a public static readonly field of the type DependencyProperty. So the field that will store the actual value of age is not an int? Yep, that is it. As you can see the field if of type DependecyProperty. This is all part of the DP way of life. The actual type of the age (which we know is int) will only be know when instantiating the DP.</p>
<p>Another thing that you may be wondering about is that the filed was named <i>AgeProperty </i>instead of just <i>Age</i>. The suffix Property is included as a convention to denote that this is the static field to the DP.</p>
<h4>Part 3: Instantiation</h4>
<p>The DependencyProperty class doesn't have a public constructor. To create an instance of this class we need to use the use the Register static method.</p>
<p>&nbsp;</p>
<p>The simpler overload of this method takes three parameters, the first is a string that sets the name of the DP (Age in our case), the second expects the type of the property, which is int, and the last one represents the type of the class to which the DP is associated (in our example is the class Person).</p>
<p>&nbsp;</p>
<p>The creation of the DP may be done inline as we've done in our example or it may be move in the static constructor like this:</p>
<pre title="code" class="brush: csharp">
public static readonly DependencyProperty AgeProperty;
static Person()
{
    AgeProperty = DependencyProperty.Register(&quot;Age&quot;, typeof(int), typeof(Person));
}</pre>
<p>This code is more than a Factory to instantiate the DependencyProperty object, it also (as the name very well describes) registers the new property within the DP infrastructure. If you are interested in low level details, the actual registration occurs a private method called RegisterCommon of the DependencyProperty class. Download the framework source code or use <a id="ypfn" href="http://www.red-gate.com/products/reflector/" target="_blank" title="Reflector">Reflector</a> to look inside.</p>
<h4>Part 4: Wrapping the DP</h4>
<p>Now that you saw what is really behind a DP you may be asking yourself. Ok, so my property is age of type int, but the DP is actually of type DependencyProperty, so how do I assign a value of 30 to the object that is not really an int? Cool, you are on the right track then! <br />
The work of getting and setting the values of DPs goes through the DependencyObject class. It has a SetValue method for setting the value of the property and a GetValue method to retrieve the value. These methods are public you may use them directly like in the example below:</p>
<pre class="brush: csharp" title="code">
Person person = new Person();
person.SetValue(Person.AgeProperty, 31);
int myAge = (int)person.GetValue(Person.AgeProperty);
</pre>
<p>Since these methods are part of the DependencyObject class is clear that they are general purpose methods to retrieve or set values of properties. It is pretty straight forward then that you have to tell which property you want to get the value from. That is when you use your static DependencyProperty field that was already registered. Also the GetValue returns an instance of the class Object so you need to cast it to your type.<br />
This is all good but I bet you are saying that this syntax is too verbose, right? Don't worry I think so too and the WPF team as well. In order to provide easy access to the DPs you may use property wrappers, like in our example. Using this strategy you code will become easier to work with and people don't even have to know if the property is a DP or not.</p>
<pre class="brush: csharp" title="code">
Person person = new Person();
person.Age = 31;
int myAge = person.Age;</pre>
<p><u><b>IMPORTANT</b></u><br />
When using properties you should be advised that you should not use any other code within the get or set accessors. This warning is documented in every book or piece of documentation I've read so far! Doing anything but using GetValue or SetValue in the accessors may really mess up your program. The thing is that WPF will use in many parts of it's code the GetValue and SetValue methods instead of your property wrapper. If your wrapper adds any thing to the mix you may have inconsistent results through your application, so watch out for this.</p>
<h4>But wait, there's more</h4>
<p>Now that we have the base idea of DP, let's keep going and try to understand few more concepts.</p>
<h4>LocalValue vs EffectiveValue</h4>
<p>Understanding this distinction really important in the process of learning WPF. The actual distinction is not even visible at first, but as soon as you begin to study coercion you notice that there is something more than just a value to the property. <br />
The LocalValue is the &quot;real&quot; value, or the value that you set to the property. This value is stored but it may not be the one displayed if you try to use the GetValue method. But why would the property show a value other than the one you set? Let's imagine that in our example we could have a MinAge and a MaxAge properties and we set the values as in the following code:</p>
<pre class="brush: csharp" title="code">
person.MinAge = 21;
person.MaxAge = 150;
person.Age = 15;
int age = person.Age;</pre>
<p>Setting the Age to 15 would not be allowed since the minimum value is 21. The DP infrastructure code has was to enforce these constraints (I'll show it a bit latter). When the code reads the property reads the Age it will return 21 which is the EffectiveValue. Does this implies that the 15 is gone? Nope! The 15 is still stored as the LocalValue (or real value if you will), if you don't believe me just use the ReadLocalValue method to read the Age property.<br />
So the EffectiveValue is the result of applying constraints (or corrections) over the LocalValue. More often than not these values will actually be different.<br />
One last thing about Local vs Effective is how the default value of the property is set. Age is an int, right? What is the default value for an int? Yep, 0 (zero) is the correct answer! So if you do person.Age you expect it to return 0 if you have not done any other initialization to this property. By now you probably figured out that the EffectiveValue has to be initialized to the default value of the property type. So EffectiveValue stores 0. The LocalValue on the other hand only gets a value when it set explicitly. But what is it's value mean while? Null? No, when the LocalValue is not set, the ReadLocalValue will return DependencyProperty.UnsetValue which is a public static field on the DependecyProperty class.</p>
<h4>FrameworkPropertyMetadata</h4>
<p>This class provides way to pass more parameters for the DependencyProperty. So far our property works fine, but we could do a little more with the FrameworkPropertyMetadata. Let's see some of the important properties that you can pass to the DP through the metadata:</p>
<ul>
    <li>DefaultValue: Using this property you can set the default value for the DP</li>
    <li>CoerceValueCallback: This callback will try to make adjustments (or correct) to the value of the property. Coercion is a concept that we'll discuss a little latter.</li>
    <li>PropertyChangedCallback: This callback will be fired when the value of the property is changed.</li>
</ul>
<h6>DefaultValue</h6>
<p>Let's you pass in a default value for the DP. When you create an instance of the class Person the Age will be equals to 0 (zero). Now a challenge: When the DefaultValue is set where is this value stored, in the LocalValue or EffectiveValue? If you said EffectiveValue you are right. Try to use the ReadLocalValue and you'll see that the value is defined as <i>DependecyProperty.UnsetValue</i>.</p>
<pre title="code" class="brush: csharp">
Person person = new Person();
object o1 = person.Age; //returns 0
object o2 = person.GetValue(Person.AgeProperty); //is the same thing as the line above
object o3 = person.ReadLocalValue(Person.AgeProperty); // returns DependencyProperty.UnsetValue</pre>
<h6>CoerceValueCallback</h6>
<p>This callback is set to a method that is going to take the value that is attributed to the property and apply some kind of correction to it. Setting a value to the property will change the value of the LocalValue and the EffectiveValue but if the CoerceValueCallback makes some change to the value this modified value is stored in the EffectiveValue while the original, unchanged attributed value is stored in the LocalValue. Let's say that any Age greater than 100 needs to be set as 100. The method to this is really simple:</p>
<p>&nbsp;</p>
<pre title="code" class="brush: csharp">
private static object CoerceAgeValue(DependencyObject d, object baseValue)
{
    int age = (int)baseValue;
    return Math.Min(100, age);
} </pre>
<p>Now let's see how an instance of the class Person would behave when having it's value set:</p>
<pre title="code" class="brush: csharp">
Person person = new Person();
person.Age = 105; //will cause the value to be coerced
object o2 = person.GetValue(Person.AgeProperty); //returns 100 (this is the coerced value)
object o3 = person.ReadLocalValue(Person.AgeProperty); //returns 105 (this is the original value)</pre>
<p>Another great example of the power given by this resource is the use of a ScrollBar:</p>
<pre title="code" class="brush: csharp">
ScrollBar bar = new ScrollBar();
//value = 0 (LV=Unset, EV=0), min = 0, max = 1</pre>
<p><br />
The ScrollBar has just been created and it's values are still the default values. Now let's change the Value to 100.</p>
<pre title="code" class="brush: csharp">
bar.Value = 100;
//value = 1 (LV=100, EV=1), min = 0, max = 1</pre>
<p><br />
The LocalValue is now 100 but the EffectiveValue was coerced to respect the Min and Max values. Though the Value shows 1 the actual value is still stored and was not lost.</p>
<pre title="code" class="brush: csharp">
bar.Minimum = 1;
//value = 1 (LV=100, EV=1), min = 1, max = 1</pre>
<p><br />
The Min value is set but it doesn't change the outcome for us.</p>
<pre title="code" class="brush: csharp">
bar.Maximum = 200;
//value = 100 (LV=100, EV=100), min = 1, max = 200
</pre>
<p><br />
Now that the Max was set to 200 the coercion will not need to change the value stored in the Value property. Now it finally shows as 100 as we had originally defined. This means that the order in which you set the properties will not affect the final result. If it was not for the was DPs work you would need to first define the Min and Max and only then you could the set Value. This is awesome!!</p>
<h6>PropertyChangedCallback</h6>
<p>This has one simple mission: Notify that a change was made to the property. In the ScrollBar whenever the Minimum or Maximum are changed a callback is fired, this callback is responsible for coercing the Value of the control using the new Maximum or Minimum.</p>
<h4>ValidateValueCallback</h4>
<p>The ValidateValueCallback is the last parameter of the DepenencyProperty.Register method. This callback points to a method that will validate the value being attributed to the property. Here is an example of a validation of the Age property:</p>
<pre class="brush: csharp" title="code">
private static bool ValidateAgePropertyValue(object value)
{
    int age = (int)value;
    return age &gt;= 0;
}</pre>
<p>In this method we validate that the Age is greater than zero after all there is no negative ages. I know that you are thinking that this is very similar to the coercion concept, right? They way I perceive the difference is that the Validation is trying to identify an incorrect value while the the Coercion takes a valid value and makes some adjustments according to other properties in the object like the Minimum or Maximum in the ScrollBar example.<br />
<br />
Another difference between the two is that the Validate method has one single parameter which is the new value that is being set. Since the method is a static method you do not have access to any other properties in the object which imposes a limitation on the kind of validation that could be made. The Coercion callback has two parameters: the dependency object that has the value and the new value for the property. Since this callback has access to the dependency object it is the ideal place to make the kind of tests that will try to limit the value between a minimum and a maximum.</p>
<h4>Putting it all together</h4>
<p>I know this is a lot to digest so the best way to really understand it all is to see a full example.</p>
<p>The following code is for a Car class with 3 Dependency Properties: Speed, MaxSpeed and MinSpeed. The speed has to be between the minimum and maximum. Every time the one of these properties is changed the others must me adjusted in order to keep the constraints we imposed.</p>
<pre class="brush: csharp" title="code">
public class Car : DependencyObject
    {
        #region Dependency Properties

        public static readonly DependencyProperty SpeedProperty;

        public static readonly DependencyProperty MinSpeedProperty;

        public static readonly DependencyProperty MaxSpeedProperty;

        #endregion


        #region Dependency Properties Wrappers

        /// &lt;summary&gt;
        /// Wrapper for the Speed dependency property
        /// &lt;/summary&gt;
        public int Speed
        {
            get { return (int)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        /// &lt;summary&gt;
        /// Wrapper for the MinSpeed dependency property
        /// &lt;/summary&gt;
        public int MinSpeed
        {
            get { return (int)GetValue(MinSpeedProperty); }
            set { SetValue(MinSpeedProperty, value); }
        }

        /// &lt;summary&gt;
        /// Wrapper for the MaxSpeed dependency property
        /// &lt;/summary&gt;
        public int MaxSpeed
        {
            get { return (int)GetValue(MaxSpeedProperty); }
            set { SetValue(MaxSpeedProperty, value); }
        }

        #endregion

        /// &lt;summary&gt;
        /// Static constructor where all dependecy properties are being initialized
        /// &lt;/summary&gt;
        static Car()
        {
            Trace.WriteLine(&quot;Static Constructor&quot;);
            //configure and register speed property
            FrameworkPropertyMetadata speedMetadata = new FrameworkPropertyMetadata(0, null, CoerceSpeed);
            SpeedProperty = DependencyProperty.Register(&quot;Speed&quot;, typeof (int), typeof (Car), speedMetadata,
                                                        ValidateSpeed);

            //configure and register min speed property
            FrameworkPropertyMetadata minMetadata = new FrameworkPropertyMetadata(0, OnMinSpeedChanged);
            MinSpeedProperty = DependencyProperty.Register(&quot;MinSpeed&quot;, typeof (int), typeof (Car), minMetadata,
                                                           ValidateSpeed);

            //configure and register max speed property
            FrameworkPropertyMetadata maxMetadata = new FrameworkPropertyMetadata(1,OnMaxSpeedChanged, CoerceMaxSpeed);
            MaxSpeedProperty = DependencyProperty.Register(&quot;MaxSpeed&quot;, typeof (int), typeof (Car), maxMetadata,
                                                           ValidateSpeed);
        }

        #region Validate methods

        public static bool ValidateSpeed(object value)
        {
            Trace.WriteLine(&quot;ValidateSpeed&quot;);
            int speed = (int)value;
            return speed &gt;= 0;
        }

        #endregion

        #region Coerce methods

        /// &lt;summary&gt;
        /// method for adjusting the speed according to the min and max
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;d&quot;&gt;&lt;/param&gt;
        /// &lt;param name=&quot;baseValue&quot;&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static object CoerceSpeed(DependencyObject d, object baseValue)
        {
            Trace.WriteLine(&quot;CoerceSpeed&quot;);
            Car car = (Car)d;
            int speed = (int)baseValue;
            //the speed can't be lower than the min speed
            speed = Math.Max(car.MinSpeed, speed);
            //the speed can't be greater than the max speed
            speed = Math.Min(car.MaxSpeed, speed);
            return speed;
        }

        /// &lt;summary&gt;
        /// method for adjusting the max speed according to me min speed.
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;d&quot;&gt;&lt;/param&gt;
        /// &lt;param name=&quot;baseValue&quot;&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public static object CoerceMaxSpeed(DependencyObject d, object baseValue)
        {
            Trace.WriteLine(&quot;CoerceMaxSpeed&quot;);
            Car car = (Car)d;
            int maxSpeed = (int) baseValue;
            //the max speed can't be lower than the main speed
            return Math.Max(car.MinSpeed, maxSpeed);
        }

        #endregion

        #region Property changed methods

        /// &lt;summary&gt;
        /// this method is fired when the MaxSpeedProperty is changed
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;d&quot;&gt;&lt;/param&gt;
        /// &lt;param name=&quot;e&quot;&gt;&lt;/param&gt;
        private static void OnMaxSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Trace.WriteLine(&quot;OnMaxSpeedChanged&quot;);
            Car car = (Car)d;
            car.CoerceValue(SpeedProperty);
        }

        /// &lt;summary&gt;
        /// this method is fired when the MinSpeedProperty is changed
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;d&quot;&gt;&lt;/param&gt;
        /// &lt;param name=&quot;e&quot;&gt;&lt;/param&gt;
        private static void OnMinSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Trace.WriteLine(&quot;OnMinSpeedChanged&quot;);
            Car car = (Car)d;
            //coerce the max speed to ajdust to the new min speed
            car.CoerceValue(MaxSpeedProperty);
            //coerce the speed to adjust to the new min speed
            car.CoerceValue(SpeedProperty);
        }

        #endregion

    }</pre>
<p>We can create an instance of the Car class in the initialization method of any window you like. Here is the my test code:</p>
<pre class="brush: csharp" title="code">
Car car = new Car();
Trace.WriteLine(string.Format(&quot;Speed:{0} Min:{1} Max:{2}&quot;,car.Speed,car.MinSpeed,car.MaxSpeed));
Trace.WriteLine(string.Format(&quot;Speed LocalValue:{0} EffectiveValue:{1}&quot;, car.ReadLocalValue(Car.SpeedProperty),
                car.GetValue(Car.SpeedProperty)));
car.Speed = 60;
Trace.WriteLine(string.Format(&quot;Speed: {0} Min:{1} Max:{2}&quot;,car.Speed,car.MinSpeed,car.MaxSpeed));
Trace.WriteLine(string.Format(&quot;Speed LocalValue:{0} EffectiveValue:{1}&quot;, car.ReadLocalValue(Car.SpeedProperty),
                car.GetValue(Car.SpeedProperty))); 
car.MinSpeed = 40;
Trace.WriteLine(string.Format(&quot;Speed: {0} Min:{1} Max:{2}&quot;,car.Speed,car.MinSpeed,car.MaxSpeed));
Trace.WriteLine(string.Format(&quot;Speed LocalValue:{0} EffectiveValue:{1}&quot;, car.ReadLocalValue(Car.SpeedProperty),
                car.GetValue(Car.SpeedProperty))); 
car.MaxSpeed = 80;
Trace.WriteLine(string.Format(&quot;Speed: {0} Min:{1} Max:{2}&quot;,car.Speed,car.MinSpeed,car.MaxSpeed));
Trace.WriteLine(string.Format(&quot;Speed LocalValue:{0} EffectiveValue:{1}&quot;, car.ReadLocalValue(Car.SpeedProperty),
                car.GetValue(Car.SpeedProperty)));</pre>
<p>When you run this code you get the following output showing the order in which the methods were called:</p>
<p>Static Constructor<br />
ValidateSpeed<br />
ValidateSpeed<br />
ValidateSpeed<br />
ValidateSpeed<br />
ValidateSpeed<br />
ValidateSpeed<br />
Speed:0 Min:0 Max:1<br />
Speed LocalValue:{DependencyProperty.UnsetValue} EffectiveValue:0<br />
ValidateSpeed<br />
CoerceSpeed<br />
ValidateSpeed<br />
Speed: 1 Min:0 Max:1<br />
Speed LocalValue:60 EffectiveValue:1<br />
ValidateSpeed<br />
OnMinSpeedChanged<br />
CoerceMaxSpeed<br />
ValidateSpeed<br />
OnMaxSpeedChanged<br />
CoerceSpeed<br />
ValidateSpeed<br />
CoerceSpeed<br />
ValidateSpeed<br />
Speed: 40 Min:40 Max:40<br />
Speed LocalValue:60 EffectiveValue:40<br />
ValidateSpeed<br />
CoerceMaxSpeed<br />
OnMaxSpeedChanged<br />
CoerceSpeed<br />
Speed: 60 Min:40 Max:80<br />
Speed LocalValue:60 EffectiveValue:60</p>
<p>I hope this was usefull.</p>
</div>