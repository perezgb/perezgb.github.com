---
layout: post
title: "MVVM Multiselect Listbox"
description: ""
date: 2010-01-02 12:00:00 UTC
category: 
tags: [wpf, mvvm]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Although MVVM is a great pattern you have to learn to work with it somethings are hard to do until you get the hang of it.</p>
<p>One of these things is doing a Multiselect Listbox. My first idea Google it, of course. The best solution I found was<a href="http://marlongrech.wordpress.com/2009/06/02/sync-multi-select-listbox-with-viewmodel/"> this one</a> by Marlon Grech. Marlon has a lot of good stuff about WPF in his blog and he definitely knows what he is talking about. This solution however became a little slow when I wanted to perform a Select All and Unselect All on the list.</p>
<p>I decided to implement a specialized list for this kind of situation. I call it a SelectionList and it is a list of SelectionItems. The idea is to have a collection of items that have a IsSelected property and a Item property that contains the real value you want. I think the code speaks for itself.</p>
<pre title="code" class="brush: csharp">
public class SelectionItem&lt;T&gt; : INotifyPropertyChanged
{
    #region Fields

        private bool isSelected;

        private T item;

        #endregion

    #region Properties

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value == isSelected) return;
                isSelected = value;
                OnPropertyChanged(&quot;IsSelected&quot;);
                OnSelectionChanged();
            }
        }

        public T Item
        {
            get { return item; }
            set
            {
                if (value.Equals(item)) return;
                item = value;
                OnPropertyChanged(&quot;Item&quot;);
            }
        }

        #endregion

    #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler SelectionChanged;

        #endregion

    #region ctor

        public SelectionItem(T item)
            : this(false, item)
        {
        }

        public SelectionItem(bool selected, T item)
        {
            this.isSelected = selected;
            this.item = item;
        }

        #endregion

    #region Event invokers

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSelectionChanged()
        {
            EventHandler changed = SelectionChanged;
            if (changed != null) changed(this, EventArgs.Empty);
        }

        #endregion
}</pre>
<p>&nbsp;The SelectionItem class is what really makes things happen. It takes an ordinary class like an string and wraps it adding a IsSelected property. This is the class of the objects that will be bound to each ListItem of the ListBox.</p>
<pre title="code" class="brush: csharp">
public class SelectionList&lt;T&gt; : 
    ObservableCollection&lt;SelectionItem&lt;T&gt;&gt; where T : IComparable&lt;T&gt;
{
    #region Properties

        /// &lt;summary&gt;
        /// Returns the selected items in the list
        /// &lt;/summary&gt;
        public IEnumerable&lt;T&gt; SelectedItems
        {
            get { return this.Where(x =&gt; x.IsSelected).Select(x =&gt; x.Item); }
        }

        /// &lt;summary&gt;
        /// Returns all the items in the SelectionList
        /// &lt;/summary&gt;
        public IEnumerable&lt;T&gt; AllItems
        {
            get { return this.Select(x =&gt; x.Item); }
        }

        #endregion

    #region ctor

        public SelectionList(IEnumerable&lt;T&gt; col)
            : base(toSelectionItemEnumerable(col))
        {

        }

        #endregion

    #region Public methods

        /// &lt;summary&gt;
        /// Adds the item to the list
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;item&quot;&gt;&lt;/param&gt;
        public void Add(T item)
        {
            int i = 0;
            foreach (T existingItem in AllItems)
            {
                if (item.CompareTo(existingItem) &lt; 0) break;
                i++;
            }
            Insert(i, new SelectionItem&lt;T&gt;(item));
        }

        /// &lt;summary&gt;
        /// Checks if the item exists in the list
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;item&quot;&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        public bool Contains(T item)
        {
            return AllItems.Contains(item);
        }

        /// &lt;summary&gt;
        /// Selects all the items in the list
        /// &lt;/summary&gt;
        public void SelectAll()
        {
            foreach (SelectionItem&lt;T&gt; selectionItem in this)
            {
                selectionItem.IsSelected = true;
            }
        }

        /// &lt;summary&gt;
        /// Unselects all the items in the list
        /// &lt;/summary&gt;
        public void UnselectAll()
        {
            foreach (SelectionItem&lt;T&gt; selectionItem in this)
            {
                selectionItem.IsSelected = false;
            }
        }

        #endregion

    #region Helper methods

        /// &lt;summary&gt;
        /// Creates an SelectionList from any IEnumerable
        /// &lt;/summary&gt;
        /// &lt;param name=&quot;items&quot;&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        private static IEnumerable&lt;SelectionItem&lt;T&gt;&gt; toSelectionItemEnumerable(IEnumerable&lt;T&gt; items)
        {
            List&lt;SelectionItem&lt;T&gt;&gt; list = new List&lt;SelectionItem&lt;T&gt;&gt;();
            foreach (T item in items)
            {
                SelectionItem&lt;T&gt; selectionItem = new SelectionItem&lt;T&gt;(item);
                list.Add(selectionItem);
            }
            return list;
        }

        #endregion
}</pre>
<p>The SelectionList is basically an <a href="http://msdn.microsoft.com/en-us/library/ms668604.aspx">ObservableCollection </a>of SelectionItem.</p>
<p>Now that you have a list of items that can be bound to each ListBoxItem I needed to figure out how to bind the IsSelected property of my items to the is IsSelected property of the ListBoxItem. I found the solution in</p>
<p><a href="http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/219b9da1-87bc-4ba3-a820-c9b8c50d28b1"> this post</a></p>
<p>in the MSDN Forums. You need to use this style:</p>
<pre class="brush: xhtml" title="code">
&lt;ListBox.ItemContainerStyle&gt;
    &lt;Style TargetType=&quot;{x:Type ListBoxItem}&quot;&gt;
        &lt;Setter Property=&quot;IsSelected&quot; 
                Value=&quot;{Binding IsSelected}&quot;/&gt;
    &lt;/Style&gt;
&lt;/ListBox.ItemContainerStyle&gt;
</pre>
<p><strong>Example:</strong></p>
<p>Imagine you need a window with a list of sports from which you have to select the sports you like. In this window you need to be able to add items to the list and select and unselect all items at once. Here is the code for the window and the ViewModel:</p>
<pre title="code" class="brush: xhtml">
&lt;Window x:Class=&quot;MvvmChecklistBox.MultiSelectWindow&quot;
        xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot;
        xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot;
        xmlns:local=&quot;clr-namespace:MvvmChecklistBox&quot;
        Title=&quot;MultiSelectWindow&quot; Height=&quot;300&quot; Width=&quot;300&quot;&gt;
    &lt;Window.Resources&gt;
        &lt;local:MainWindowViewModel x:Key=&quot;viewModel&quot;/&gt;
    &lt;/Window.Resources&gt;
    &lt;StackPanel Margin=&quot;5&quot; DataContext=&quot;{StaticResource viewModel}&quot;&gt;
        &lt;TextBlock&gt;Sport:&lt;/TextBlock&gt;
        &lt;TextBox Name=&quot;textBoxNewSport&quot;
                 Text=&quot;{Binding NewSport}&quot;/&gt;
        &lt;Button Content=&quot;Add new sport&quot;
                Command=&quot;{Binding AddCommand}&quot;/&gt;
        &lt;ListBox Name=&quot;checkboxList&quot;
                 ItemsSource=&quot;{Binding Sports}&quot;
                 DisplayMemberPath=&quot;Item&quot;
                 Margin=&quot;0,5&quot; 
                 SelectionMode=&quot;Multiple&quot;&gt;
            &lt;ListBox.ItemContainerStyle&gt;
                &lt;Style TargetType=&quot;{x:Type ListBoxItem}&quot;&gt;
                    &lt;Setter Property=&quot;IsSelected&quot; 
                            Value=&quot;{Binding IsSelected}&quot;/&gt;
                &lt;/Style&gt;
            &lt;/ListBox.ItemContainerStyle&gt;
        &lt;/ListBox&gt;
        &lt;Button Content=&quot;Select All&quot;
                Margin=&quot;0,5&quot;
                Command=&quot;{Binding SelectAllCommand}&quot;/&gt;
        &lt;Button Content=&quot;Unselect All&quot;
                Margin=&quot;0,5&quot;
                Command=&quot;{Binding UnselectAllCommand}&quot;/&gt;
    &lt;/StackPanel&gt;
&lt;/Window&gt;</pre>
<pre title="code" class="brush: csharp">
public class MainWindowViewModel : INotifyPropertyChanged
{
    #region Fields

        private ICommand _selectAllCommand;

        private ICommand _unselectAllCommand;

        private ICommand _addCommand;

        private string _newSport;

        #endregion

    #region Properties

        public SelectionList&lt;string&gt; Sports { get; set; }

        public string NewSport
        {
            get { return _newSport; }
            set
            {
                if (value == _newSport) return;
                _newSport = value;
                OnPropertyChanged(&quot;NewSport&quot;);
            }
        }

        #endregion

    #region Commands

        public ICommand SelectAllCommand
        {
            get
            {
                if (_selectAllCommand == null)
                {
                    _selectAllCommand = new RelayCommand(param =&gt; Sports.SelectAll());
                }
                return _selectAllCommand;
            }
        }

        public ICommand UnselectAllCommand
        {
            get
            {
                if (_unselectAllCommand == null)
                {
                    _unselectAllCommand = new RelayCommand(param =&gt; Sports.UnselectAll());
                }
                return _unselectAllCommand;
            }
        }

        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(param =&gt;
                    {
                        Sports.Add(NewSport);
                        NewSport = string.Empty;
                    });
                }
                return _addCommand;
            }
        }

        #endregion

    #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    #region ctor

        public MainWindowViewModel()
        {
            string[] sports = { &quot;Baseball&quot;, &quot;Basketball&quot;, &quot;Football&quot;, &quot;Handball&quot;, &quot;Soccer&quot;, &quot;Volleyball&quot; };
            Sports = new SelectionList&lt;string&gt;(sports);
        }

        #endregion

    #region Event invokers

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
}</pre>
<p>Here is the result:</p>
<p><img src="http://www.gbogea.com/upload/MultiSelectWindow.png" alt="MultiSelectWindow" /></p>
<p>The solution was really simple after I though of the SelectionList. I wanted however to do a little bit more. Instead of the simple Listbox I had to build a list of CheckBoxes. To good thing is that as far as the ViewModel is concerned we are all set, there's no need to change anything. All we need to change is the View (which is a Window in our case) using a DataTemple in the ListBox ItemTemplate to place a Checkbox instead of regular item.</p>
<pre class="brush: xhtml" title="code">
&lt;ListBox.ItemTemplate&gt;
    &lt;DataTemplate&gt;
        &lt;CheckBox IsChecked=&quot;{Binding IsSelected}&quot; 
                  Content=&quot;{Binding Item}&quot;/&gt;
    &lt;/DataTemplate&gt;
&lt;/ListBox.ItemTemplate&gt;</pre>
<p>Did it work? Kind of... It works but something weird happens because you can select all the checkboxes you want and then select the ListItem which gets highlighted. So now you have the checkbox selection and the item highlight selection and the two of them do not match. I don't want the ListBox item to get highlighted. Once again Google helped and I found a style to do just what I wanted in <a href="http://alexshed.spaces.live.com/blog/cns!71C72270309CE838!133.entry">Alex Filo's blog</a>. Here is the code:</p>
<pre class="brush: xhtml" title="code">
&lt;ListBox.ItemContainerStyle&gt;
    &lt;Style&gt;
        &lt;Setter Property=&quot;ListBoxItem.Background&quot; Value=&quot;Transparent&quot;/&gt;
        &lt;Setter Property=&quot;ListBoxItem.Template&quot;&gt;
            &lt;Setter.Value&gt;
                &lt;ControlTemplate TargetType=&quot;{x:Type ListBoxItem}&quot;&gt;
                    &lt;Border x:Name=&quot;Bd&quot; 
                    SnapsToDevicePixels=&quot;true&quot; 
                    Background=&quot;{TemplateBinding Background}&quot; 
                    BorderBrush=&quot;{TemplateBinding BorderBrush}&quot; 
                    BorderThickness=&quot;{TemplateBinding BorderThickness}&quot; 
                    Padding=&quot;{TemplateBinding Padding}&quot;&gt;
                        &lt;ContentPresenter HorizontalAlignment=&quot;{TemplateBinding HorizontalContentAlignment}&quot; 
                                  VerticalAlignment=&quot;{TemplateBinding VerticalContentAlignment}&quot; 
                                  SnapsToDevicePixels=&quot;{TemplateBinding SnapsToDevicePixels}&quot;/&gt;
                    &lt;/Border&gt;
                    &lt;ControlTemplate.Triggers&gt;
                        &lt;Trigger Property=&quot;IsSelected&quot; Value=&quot;true&quot;&gt;
                            &lt;Setter Property=&quot;Background&quot; TargetName=&quot;Bd&quot; Value=&quot;Transparent&quot; /&gt;
                        &lt;/Trigger&gt;
                    &lt;/ControlTemplate.Triggers&gt;
                &lt;/ControlTemplate&gt;
            &lt;/Setter.Value&gt;
        &lt;/Setter&gt;
    &lt;/Style&gt;
&lt;/ListBox.ItemContainerStyle&gt;</pre>
<p><img src="http://www.gbogea.com/upload/CheckBoxListError.png" alt="CheckBoxListError" /></p>
<p>Now we're done! Here is the final code for the view:</p>
<pre class="brush: xhtml" title="code">
&lt;Window x:Class=&quot;MvvmChecklistBox.MainWindow&quot;
        xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot;
        xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot;
        Title=&quot;MainWindow&quot; Height=&quot;350&quot; Width=&quot;300&quot;&gt;
    &lt;StackPanel Margin=&quot;5&quot;&gt;
        &lt;TextBlock&gt;Sport:&lt;/TextBlock&gt;
        &lt;TextBox Name=&quot;textBoxNewSport&quot;
                 Text=&quot;{Binding NewSport}&quot;/&gt;
        &lt;Button Content=&quot;Add new sport&quot;
                Command=&quot;{Binding AddCommand}&quot;/&gt;
        &lt;ListBox Name=&quot;checkboxList&quot;
                 ItemsSource=&quot;{Binding Sports}&quot;
                 Margin=&quot;0,5&quot;&gt;
            &lt;ListBox.ItemContainerStyle&gt;
                &lt;Style&gt;
                    &lt;Setter Property=&quot;ListBoxItem.Background&quot; Value=&quot;Transparent&quot;/&gt;
                    &lt;Setter Property=&quot;ListBoxItem.Template&quot;&gt;
                        &lt;Setter.Value&gt;
                            &lt;ControlTemplate TargetType=&quot;{x:Type ListBoxItem}&quot;&gt;
                                &lt;Border x:Name=&quot;Bd&quot; 
                                SnapsToDevicePixels=&quot;true&quot; 
                                Background=&quot;{TemplateBinding Background}&quot; 
                                BorderBrush=&quot;{TemplateBinding BorderBrush}&quot; 
                                BorderThickness=&quot;{TemplateBinding BorderThickness}&quot; 
                                Padding=&quot;{TemplateBinding Padding}&quot;&gt;
                                    &lt;ContentPresenter HorizontalAlignment=&quot;{TemplateBinding HorizontalContentAlignment}&quot; 
                                              VerticalAlignment=&quot;{TemplateBinding VerticalContentAlignment}&quot; 
                                              SnapsToDevicePixels=&quot;{TemplateBinding SnapsToDevicePixels}&quot;/&gt;
                                &lt;/Border&gt;
                                &lt;ControlTemplate.Triggers&gt;
                                    &lt;Trigger Property=&quot;IsSelected&quot; Value=&quot;true&quot;&gt;
                                        &lt;Setter Property=&quot;Background&quot; TargetName=&quot;Bd&quot; Value=&quot;Transparent&quot; /&gt;
                                    &lt;/Trigger&gt;
                                &lt;/ControlTemplate.Triggers&gt;
                            &lt;/ControlTemplate&gt;
                        &lt;/Setter.Value&gt;
                    &lt;/Setter&gt;
                &lt;/Style&gt;
            &lt;/ListBox.ItemContainerStyle&gt;
            &lt;ListBox.ItemTemplate&gt;
                &lt;DataTemplate&gt;
                    &lt;CheckBox IsChecked=&quot;{Binding IsSelected}&quot; 
                              Content=&quot;{Binding Item}&quot;/&gt;
                &lt;/DataTemplate&gt;
            &lt;/ListBox.ItemTemplate&gt;
        &lt;/ListBox&gt;
        &lt;Button Content=&quot;Select All&quot;
                Margin=&quot;0,5&quot;
                Command=&quot;{Binding SelectAllCommand}&quot;/&gt;
        &lt;Button Content=&quot;Unselect All&quot;
                Margin=&quot;0,5&quot;
                Command=&quot;{Binding UnselectAllCommand}&quot;/&gt;
    &lt;/StackPanel&gt;
&lt;/Window&gt;</pre>
<p>I'm sure that someone has had this problem and the SelectionList seems like a straight forward solution. I haven't found anything like it but I'm sure I can't be the first to think of it. Well, I hope this helps you and if you find of think of a better solution please leave a comment.</p>
</div>