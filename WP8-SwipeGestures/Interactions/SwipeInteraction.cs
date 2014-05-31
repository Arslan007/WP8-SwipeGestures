using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AP.ViewModel;
using LinqToVisualTree;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Windows.Input;

namespace AP.Interactions
{
    /// <summary>
    /// Adds an interaction that allows the user to swipe an item to mark it as complete
    /// or delete it
    /// </summary>
    public class SwipeInteraction : InteractionBase, IInteraction
    {
        private static readonly double FlickVelocity = 2000.0;

        // Drag distance required to consider this a swipe interaction
        private static readonly double DragStartedDistance = 5.0;
        double offset;

        private FrameworkElement _tickAndCrossContainer;
        private FrameworkElement _completedItemContainer;
        private SoundEffect _completeSound;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SwipeInteraction()
        {
            _completeSound = SoundEffect.FromStream(TitleContainer.OpenStream("Sounds/Windows XP Exclamation.wav"));
        }

        /// <summary>
        /// Override Method AddElement
        /// </summary>
        /// <param name="element"></param>
        public override void AddElement(FrameworkElement element)
        {
            //Registered Framework Element ManiulationDelta Event
            element.ManipulationDelta += Element_ManipulationDelta;
            //Registered Framework Element ManiulationCompleted Event
            element.ManipulationCompleted += Element_ManipulationCompleted;
        }

        /// <summary>
        /// Framework Element Manipulation Completed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (!IsActive)
                return;

            FrameworkElement fe = sender as FrameworkElement;
            if ((Math.Abs(e.TotalManipulation.Translation.X) > fe.ActualWidth / 2 ||
              Math.Abs(e.FinalVelocities.LinearVelocity.X) > FlickVelocity) && offset >= 200)
            {
                //Item is in Completed State. Call ItemCompletedAction
                ItemCompletedAction(fe);
            }
            else
            {
                //Not Enough Drag. Boucne Back Item
                ItemBounceBack(fe);
            }

            IsActive = false;
        }

        /// <summary>
        /// Framework Element ManipulationDelta Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // Is Enabled ??
            if (!IsEnabled)
                return; //No

            // IsActive ??
            if (!IsActive) //No
            {
                 // initialize FrameworkElement
                FrameworkElement fe = sender as FrameworkElement;
                
                // If Right To Left Gesture and Item is Already Dragged
                if (e.CumulativeManipulation.Translation.X < 0 && IsCheckItemDragged(fe))
                {
                    //Bounce Back Item As Its Already Dragged And Its Right To Left Translation
                    ItemBounceBack(fe);
                    return;
                }

                // If Left To Right Gesture But Less Than Default Dragged Value.
                if (e.CumulativeManipulation.Translation.X < DragStartedDistance) //Yes ??No
                    return; //No

                IsActive = true;

                // Initialize the drag
                fe.SetHorizontalOffset(0);

                // find the container of Custom Control That Needs To Be Dragged Out
                _tickAndCrossContainer = fe.Descendants()
                                           .OfType<FrameworkElement>()
                                           .Single(i => i.Name == "tickAndCross");
            }
            else
            {
                // Handle the drag to offset the element
                FrameworkElement fe = sender as FrameworkElement;

                // Get FrameworkElement Horizontal Off Set  With Translation Value From Left To Right
                offset = fe.GetHorizontalOffset().Value + e.DeltaManipulation.Translation.X;

                // Set This Value to Container
                fe.SetHorizontalOffset(offset);

                _tickAndCrossContainer.Opacity = TickAndCrossOpacity(offset);
            }
        }

        /// <summary>
        /// Set The Opacity of Custom Control
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private double TickAndCrossOpacity(double offset)
        {
            offset = Math.Abs(offset);
            if (offset < 50)
                return 0;

            offset -= 50;
            double opacity = offset / 100;

            opacity = Math.Max(Math.Min(opacity, 1), 0);
            return opacity;
        }

        /// <summary>
        /// Bounce Back The Item
        /// </summary>
        /// <param name="fe"></param>
        private void ItemBounceBack(FrameworkElement fe)
        {
            var trans = fe.GetHorizontalOffset().Transform;
            trans.Animate(trans.X, 0, TranslateTransform.XProperty, 300, 0, new BounceEase()
            {
                Bounciness = 5,
                Bounces = 2
            });
        }

        /// <summary>
        /// Dragged Item Completed Action
        /// </summary>
        /// <param name="fe"></param>
        private void ItemCompletedAction(FrameworkElement fe)
        {
            //If There is Any Item Already Dragged Then Bounce Back That Item
            CompletedItemBounceBack();

            // set the mode object to complete
            ToDoItemViewModel completedItem = fe.DataContext as ToDoItemViewModel;
            completedItem.Completed = true; // Set Vale Completed
            completedItem.Color = Colors.Green; //Change Color Green
            _completeSound.Play(); // Play Sound
            _completedItemContainer = fe; //Capture Completed Item Container FrameworkElement
        }

        /// <summary>
        /// CompletedItemBounceBack Event
        /// </summary>
        private void CompletedItemBounceBack()
        {
             // find the items in view, and the location of the deleted item in this list
            var itemsInView = _todoList.GetItemsInView().ToList();

            //Check Each List For Completed Item
            itemsInView.ForEach((item) =>
            {
                // Get ItemViewModel From Item
                ToDoItemViewModel temp = item.DataContext as ToDoItemViewModel;
                if (temp.Completed) // If Completed
                {
                    temp.Completed = false; // Set false as it will be bounced back
                    if (_completedItemContainer != null) // Check CompletedItem Container
                    {
                        ItemBounceBack(_completedItemContainer); // Bounce Back This Framework Element
                        _completedItemContainer = null; // Set Null 
                    }
                }
            });
        }

        /// <summary>
        /// Check Item Dragged
        /// </summary>
        /// <param name="fe"></param>
        /// <returns></returns>
        private bool IsCheckItemDragged(FrameworkElement fe)
        { 
            // Get Completed Item
            ToDoItemViewModel completedItem = fe.DataContext as ToDoItemViewModel;
            if (completedItem.Completed) // If Complted Mean Already Dragged
            {
                completedItem.Completed = false; // Set Value False
                _completedItemContainer = null; // Set Value null
                return true; // return True
            }
         
            else // Not Dragged
                return false; // Return False
        }
    }
}
