﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MediaBrowser.Common;
using MediaBrowser.Common.Updates;
using MediaBrowser.Theater.Interfaces.Configuration;
using MediaBrowser.Theater.Interfaces.Navigation;
using MediaBrowser.Theater.Interfaces.Presentation;
using MediaBrowser.Theater.Interfaces.Session;
using MediaBrowser.Theater.Interfaces.System;
using MediaBrowser.Theater.Presentation.Controls;

namespace MediaBrowser.Theater.DirectShow.Configuration
{
    /// <summary>
    /// Interaction logic for MadVrSettingsPage.xaml
    /// </summary>
    public partial class MadVrSettingsPage : Page
    {
        private readonly INavigationService _nav;
        private readonly ITheaterConfigurationManager _config;
        private readonly IMediaFilters _mediaFilters;
        private readonly IPresentationManager _presentation;

        public MadVrSettingsPage(INavigationService nav, ITheaterConfigurationManager config, IPresentationManager presentation, IMediaFilters mediaFilters)
        {
            _nav = nav;
            _config = config;
            _presentation = presentation;
            _mediaFilters = mediaFilters;
            

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Loaded += GeneralSettingsPage_Loaded;
            Unloaded += GeneralSettingsPage_Unloaded;


            SelectSmoothMode.Options = new List<SelectListItem>
            {
                 new SelectListItem{ Text = "Avoid Judder", Value="avoidJudder"},
                 new SelectListItem{ Text = "Almost Always", Value="almostAlways"},
                 new SelectListItem{ Text = "always", Value="always"}
            };

            SelectScalingMode.Options = new List<SelectListItem>
            {
                 new SelectListItem{ Text = "Half Size", Value="0"},
                 new SelectListItem{ Text = "Original Size", Value="1"},
                 new SelectListItem{ Text = "Double Size", Value="2"},
                 new SelectListItem{ Text = "Stretch", Value="3"},
                 new SelectListItem{ Text = "Touch Inside", Value="4"},
                 new SelectListItem{ Text = "Touch Outside", Value="5"},
                 new SelectListItem{ Text = "Zoom 1", Value="6"},
                 new SelectListItem{ Text = "Zoom 2", Value="7"}
            };
        }

        void GeneralSettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var config = _config.Configuration.InternalPlayerConfiguration;

            ChkEnableSmoothMotion.IsChecked = config.VideoConfig.UseMadVrSmoothMotion;
            SelectSmoothMode.SelectedValue = config.VideoConfig.MadVrSmoothMotionMode;
            SelectScalingMode.SelectedValue = config.VideoConfig.ScalingMode.ToString();
        }

        void GeneralSettingsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            var config = _config.Configuration.InternalPlayerConfiguration;

            config.VideoConfig.UseMadVrSmoothMotion = ChkEnableSmoothMotion.IsChecked ?? false;
            config.VideoConfig.MadVrSmoothMotionMode = SelectSmoothMode.SelectedValue;
            config.VideoConfig.ScalingMode = int.Parse(SelectScalingMode.SelectedValue);

            _config.SaveConfiguration();
        }
    }
}
