using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AP
{
    public partial class TickAndCross : UserControl
    {
        //Public RoutedArgs Event Handler//
        public event EventHandler<RoutedEventArgs> OnClickTick;
        public event EventHandler<RoutedEventArgs> OnClickEdit;
        public event EventHandler<RoutedEventArgs> OnClickMusic;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public TickAndCross()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Tick Textblock Tap Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTick_Tap(object sender, GestureEventArgs e)
        {
            // Passing Tick Tap EventArgs to our Custom Routed EventArgs To Get Tapped Event on Main View.
            if (OnClickTick != null)
                OnClickTick(sender, e);
        }

        private void tbEdit_Tap(object sender, GestureEventArgs e)
        {
            // Passing Edit Tap EventArgs to our Custom Routed EventArgs To Get Tapped Event on Main View.
            if (OnClickEdit != null)
                OnClickEdit(sender, e);
        }

        private void tbMusic_Tap(object sender, GestureEventArgs e)
        {
            // Passing Music Tap EventArgs to our Custom Routed EventArgs To Get Tapped Event on Main View.
            if (OnClickMusic != null)
                OnClickMusic(sender, e);
        }
    }
}
