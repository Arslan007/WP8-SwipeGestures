using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using AP.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using AP.Interactions;
using System.Diagnostics;

namespace AP
{
    public partial class MainPage : PhoneApplicationPage
    {
        //Observable Collection of ToDoItemViewModel//
        private ObservableCollection<ToDoItemViewModel> _todoItems = new ObservableCollection<ToDoItemViewModel>();
        //Interaction Manager Object//
        private InteractionManager _interactionManager = new InteractionManager();

        /// <summary>
        /// Default Constructor 
        /// Author: Arslan Pervaiz
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            _todoItems.Add(new ToDoItemViewModel("Item 1"));
            _todoItems.Add(new ToDoItemViewModel("Item 2"));
            _todoItems.Add(new ToDoItemViewModel("Item 3"));
            _todoItems.Add(new ToDoItemViewModel("Item 4"));
            _todoItems.Add(new ToDoItemViewModel("Item 5"));
            _todoItems.Add(new ToDoItemViewModel("Item 6"));
            _todoItems.Add(new ToDoItemViewModel("Item 7"));
            _todoItems.Add(new ToDoItemViewModel("Item 8"));
            _todoItems.Add(new ToDoItemViewModel("Item 9"));
            _todoItems.Add(new ToDoItemViewModel("Item 10"));
            _todoItems.Add(new ToDoItemViewModel("Item 11"));
            _todoItems.Add(new ToDoItemViewModel("Item 12"));
            _todoItems.Add(new ToDoItemViewModel("Item 13"));
            _todoItems.Add(new ToDoItemViewModel("Item 14"));
            _todoItems.Add(new ToDoItemViewModel("Item 15"));

            this.DataContext = _todoItems;

            var swipeInteraction = new SwipeInteraction();
            swipeInteraction.Initialise(todoList, _todoItems);
            _interactionManager.AddInteraction(swipeInteraction);
            FrameworkDispatcher.Update();
        }

        /// <summary>
        /// Border Loaded Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            //Interaction Manager Registerd with Border to Get All Elements and Manipluations
            _interactionManager.AddElement(sender as FrameworkElement);
        }

        /// <summary>
        /// onClickTick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onClickTickEvent(object sender, RoutedEventArgs e)
        {
            //Get the framework Element from Sender
            FrameworkElement fe = sender as FrameworkElement;
            //Get the Tapped Item From Framework Data Context
            ToDoItemViewModel tappedItem = fe.DataContext as ToDoItemViewModel;
            //Dispaly Name of Tapped Item OR you do your custom code
            MessageBox.Show("Ticked " + tappedItem.Text);
        }

        /// <summary>
        /// OnClickEdit Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickEditEvent(object sender, RoutedEventArgs e)
        {
            //Get the framework Element from Sender
            FrameworkElement fe = sender as FrameworkElement;
            //Get the Tapped Item From Framework Data Context
            ToDoItemViewModel tappedItem = fe.DataContext as ToDoItemViewModel;
            //Dispaly Name of Tapped Item OR you do your custom code
            MessageBox.Show("Edit " + tappedItem.Text);
        }

        /// <summary>
        /// OnClickMusic Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickMusicEvent(object sender, RoutedEventArgs e)
        {
            //Get the framework Element from Sender
            FrameworkElement fe = sender as FrameworkElement;
            //Get the Tapped Item From Framework Data Context
            ToDoItemViewModel tappedItem = fe.DataContext as ToDoItemViewModel;
            //Dispaly Name of Tapped Item OR you do your custom code
            MessageBox.Show("Music " + tappedItem.Text);
        }
    }
}